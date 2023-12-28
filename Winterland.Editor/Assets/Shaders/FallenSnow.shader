Shader "Winterland/Fallen Snow Surface" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        [NoScaleOffset] _SinkDetail("Sink Detail (R)", 2D) = "white" {}
        _SinkDetailScale("Sink Detail Scale", float) = 1.0
        _SinkDetailDeformMultiplier("Sink Detail Deform Multiplier", Range(0,1)) = 1.0
        _SinkDetailShadeMultiplier("Sink Detail Shade Multiplier", Range(0,1)) = 1.0
        _SinkShadowStrength("Sink Shadow Strength", Range(0,1)) = 1.0
        _SinkColor("Sink Color", Color) = (1, 1, 1, 1)
        _Tess("Tessellation", Range(1,64)) = 4
        _Displacement("Displacement", float) = 1.0
        _TestDisplacement("Test Displacement", Range(0,1)) = 0.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf SimpleLambert addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap noambient
            #pragma target 4.6
            #include "Tessellation.cginc"
            #include "BRCCommon.cginc"

            BRC_LIGHTING_PROPERTIES

            struct SurfaceOutputSnow
            {
                fixed3 Albedo;
                fixed3 Normal;
                fixed3 Emission;
                fixed Alpha;
                fixed Shadow;
            };

            float2 CameraPosition;
            float DepthHalfRadius;

            float2 GetDepthUVAt(float2 worldPos) {
                float2 worldCoordsBegin = CameraPosition - float2(DepthHalfRadius, DepthHalfRadius);
                float2 coords = worldPos - worldCoordsBegin;
                return coords / (DepthHalfRadius * 2);
            }

            half4 LightingSimpleLambert(SurfaceOutputSnow s, half3 lightDir, half atten) {
              if (atten > SHADOW_THRESHOLD)
                atten = 1.0;
              else
                atten = 0.0;
              half NdotL = saturate(dot(s.Normal, lightDir) * atten);
              if (NdotL > LIGHT_THRESHOLD)
                  NdotL = 1.0;
              else
                  NdotL = 0.0;
              NdotL *= s.Shadow;
              // atten is shadows!
              float4 lightColor = lerp(ShadowColor, LightColor, NdotL);
              half4 c;
              c.rgb = s.Albedo * _LightColor0.rgb * lightColor;
              c.a = s.Alpha;
              return c;
          }

            struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            float _Tess;

            float4 tessDistance(appdata v0, appdata v1, appdata v2) {
                float minDist = 10.0;
                float maxDist = 50.0;
                float tessAmount = ((_Tess - 1) * v0.color.r) + 1;
                return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, tessAmount);
            }

            sampler2D _SinkDetail;
            float _SinkDetailScale;
            sampler2D _DispTex;
            sampler2D DepthTexture;
            float _Displacement;
            float _TestDisplacement;

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            float GetDisplacementAmount(float2 worldPos, float multiplier) {
                float2 depthuv = GetDepthUVAt(worldPos);
                float2 detailUv = worldPos * _SinkDetailScale;
                float detail = lerp(1.0, tex2Dlod(_SinkDetail, float4(detailUv, 0, 0)).r, multiplier);
                if (depthuv.x < 0 || depthuv.y < 0 || depthuv.x > 1 || depthuv.y > 1)
                    return _TestDisplacement * detail;
                return (tex2Dlod(DepthTexture, float4(depthuv, 0, 0)).r + _TestDisplacement) * detail;
            }

            float _SinkDetailDeformMultiplier;

            void disp(inout appdata v)
            {
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float d = GetDisplacementAmount(worldPos.xz, _SinkDetailDeformMultiplier) * _Displacement;
                v.vertex.y -= d;
            }

            sampler2D _MainTex;
            float4 _SinkColor;
            float _SinkShadowStrength;
            float _SinkDetailShadeMultiplier;

            void surf(Input IN, inout SurfaceOutputSnow o) {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                float d = saturate(GetDisplacementAmount(IN.worldPos.xz, _SinkDetailShadeMultiplier) * 1);
                if (d > 0.5)
                    d = 1.0;
                else if (d > 0.2)
                    d = 0.5;
                else
                    d = 0.0;
                o.Albedo = lerp(c.rgb, _SinkColor.rgb, d);
                d *= _SinkShadowStrength;
                o.Shadow = (-d)+1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}

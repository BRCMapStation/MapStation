Shader "Winterland/Fallen Snow Surface" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Tess("Tessellation", Range(1,64)) = 4
        _Displacement("Displacement", float) = 1.0
        _TestDisplacement("Test Displacement", Range(0,1)) = 0.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf SimpleLambert addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap
            #pragma target 4.6
            #include "Tessellation.cginc"
            #define LIGHT_MULTIPLY 150.0

            float4 LightColor;
            float4 ShadowColor;

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
              half NdotL = saturate(dot(s.Normal, lightDir) * atten * LIGHT_MULTIPLY) * s.Shadow;
              // atten is shadows!
              float4 lightColor = lerp(ShadowColor, LightColor, NdotL);
              half4 c;
              c.rgb = s.Albedo * lightColor;
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
                float maxDist = 30.0;
                float tessAmount = _Tess * v0.color.r;
                return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, tessAmount);
            }

            sampler2D _DispTex;
            sampler2D DepthTexture;
            float _Displacement;
            float _TestDisplacement;

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            float GetDisplacementAmount(float2 uv) {
                if (uv.x < 0 || uv.y < 0 || uv.x > 1 || uv.y > 1)
                    return _TestDisplacement;
                return tex2Dlod(DepthTexture, float4(uv, 0, 0)).r + _TestDisplacement;
            }

            void disp(inout appdata v)
            {
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float2 depthUv = GetDepthUVAt(worldPos.xz);
                float d = GetDisplacementAmount(depthUv) * _Displacement;
                v.vertex.y -= d;
            }

            sampler2D _MainTex;

            void surf(Input IN, inout SurfaceOutputSnow o) {
                float2 depthUv = GetDepthUVAt(IN.worldPos.xz);
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                float d = saturate(GetDisplacementAmount(depthUv) * 1);
                o.Albedo = c.rgb;
                o.Shadow = (-d)+1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}

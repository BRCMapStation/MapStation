Shader "Winterland/Fallen Snow Surface" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Tess("Tessellation", Range(1,32)) = 4
        _DepthTex("Depth Texture", 2D) = "black" {}
        _Displacement("Displacement", float) = 1.0
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
            };

            float _Tess;

            float4 tessDistance(appdata v0, appdata v1, appdata v2) {
                float minDist = 10.0;
                float maxDist = 25.0;
                return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
            }

            sampler2D _DispTex;
            sampler2D _DepthTex;
            float _Displacement;

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            void disp(inout appdata v)
            {
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float d = tex2Dlod(_DepthTex, float4(worldPos.xz,0,0)).r * _Displacement;
                v.vertex.y -= d;
            }

            sampler2D _MainTex;

            void surf(Input IN, inout SurfaceOutputSnow o) {
                half4 c = tex2D(_MainTex, IN.worldPos.xz);
                float d = saturate(tex2Dlod(_DepthTex, float4(IN.worldPos.xz, 0, 0)).r * 2);
                o.Albedo = c.rgb;
                o.Shadow = (-d)+1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}

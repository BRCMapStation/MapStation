Shader "Winterland/FX/Bloom Pass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0,1)) = 0
        _Multiply("Multiply", float) = 1
        _BlurSize("Blur Size", Vector) = (1920, 1080, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float _Threshold;
            float _Multiply;
            float2 _BlurSize;

            float4 blur(sampler2D sp, float2 U, float2 scale, int samples, int LOD)
            {
                float4 O = (float4) 0;
                int sLOD = 1 << LOD;
                float sigma = float(samples) * 0.25;
                int s = samples/sLOD;  
                for (int i = 0; i < s*s; i++)
                {
                    float2 d = float2(i%(uint)s, i/(uint)s) * float(sLOD) - float(samples)/2.;
                    float2 t = d;
                    O += exp(-0.5* dot(t/=sigma,t) ) / ( 6.28 * sigma*sigma ) * tex2Dlod( sp, float4(U + scale * d, 0, LOD));
                }
                return O / O.a;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = blur(_MainTex, i.uv, _BlurSize, 16, 0);
                col.rgb -= _Threshold;
                col.rgb = max(0, col.rgb);
                col.rgb *= _Multiply;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Winterland/Snow Gradient"
{
    Properties
    {
        _MainTex ("Gradient Texture", 2D) = "white" {}
        _DetailTex ("Detail Texture", 2D) = "black" {}
        _DetailStrength ("Detail Strength", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest+50" }
        Offset -1, -1
        LOD 100

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define LIGHT_THRESHOLD 0.5

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                SHADOW_COORDS(2) // put shadows data into TEXCOORD1
                float2 detailuv : TEXCOORD3;
            };

            float4 LightColor;
            float4 ShadowColor;
            sampler2D _MainTex;
            sampler2D _DetailTex;
            float4 _MainTex_ST;
            float4 _DetailTex_ST;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.detailuv = TRANSFORM_TEX(v.uv, _DetailTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o)
                return o;
            }

            float _DetailStrength;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed lighting = saturate(dot(i.normal, _WorldSpaceLightPos0)) * SHADOW_ATTENUATION(i);
                if (lighting > LIGHT_THRESHOLD)
                    lighting = 1.0;
                else
                    lighting = 0.0;
                float4 lightColor = lerp(ShadowColor, LightColor, lighting);
                fixed4 col = tex2D(_MainTex, i.uv) * lightColor * _LightColor0.a;
                float detail = tex2D(_DetailTex, i.detailuv).r * _DetailStrength;
                col.a = pow(col.a, (-detail)+1);
                return col;
            }
            ENDCG
        }
    }
}

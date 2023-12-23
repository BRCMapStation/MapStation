// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Winterland/Snow On Prop"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SnowColor("Snow Color", Color) = (1,1,1,1)
        _SnowDetail("Snow Detail", 2D) = "white" {}
        _SnowDetailStrength("Snow Detail Strength", Range(0,1)) = 1
        _AlphaCutoff("Alpha Cutoff", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define LIGHT_THRESHOLD 0.1

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
                float3 localNormal : TEXCOORD3;
                float2 snowuv : TEXCOORD4;
            };

            float4 LightColor;
            float4 ShadowColor;
            sampler2D _MainTex;
            sampler2D _SnowDetail;
            float4 _MainTex_ST;
            float4 _SnowDetail_ST;
            float _SnowDetailStrength;
            float _AlphaCutoff;
            float4 _SnowColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.snowuv = TRANSFORM_TEX(v.vertex.xy, _SnowDetail);
                o.localNormal = v.normal;
                o.normal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed lighting = saturate(dot(i.normal, _WorldSpaceLightPos0)) * SHADOW_ATTENUATION(i);
                if (lighting > LIGHT_THRESHOLD)
                    lighting = 1.0;
                else
                    lighting = 0.0;
                float4 lightColor = lerp(ShadowColor, LightColor, lighting);

                float4 diffuse = tex2D(_MainTex, i.uv);

                float snowAmount = max(0, dot(i.localNormal, float3(0, 0, 1)));
                float snowDetail = tex2D(_SnowDetail, i.snowuv).r;
                snowAmount *= lerp(1, snowDetail, _SnowDetailStrength);
                if (snowAmount > _AlphaCutoff)
                    snowAmount = 1;
                else
                    snowAmount = 0;

                snowAmount *= _SnowColor.a;
                diffuse = lerp(diffuse, _SnowColor, snowAmount);

                fixed4 col = diffuse * lightColor * _LightColor0.a;
                
                return col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}

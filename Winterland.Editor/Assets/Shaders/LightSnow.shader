// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Winterland/ Light Snow Layer"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Shade ("Shade", Range(0,1)) = 0
        _DetailTex ("Detail Texture (R)", 2D) = "white" {}
        _DetailTex2 ("Detail Texture 2 (R)", 2D) = "white" {}
        _DetailStrength ("Detail Strength", Range(0,1)) = 1
        _Detail2Strength ("Detail 2 Strength", Range(0,1)) = 1
        _Density ("Snow Density", Range(0,1)) = 1
        _VertexColorPower ("Vertex Color Power", float) = 1.0
        _NormalOffset("Normal Offset", float) = 0
        _FinalAlphaMultiply("Final Alpha Multiply", float) = 1
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
                float4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                SHADOW_COORDS(2)
                float2 uv2 : TEXCOORD3;
                float4 color : COLOR0;
            };

            float4 LightColor;
            float4 ShadowColor;
            sampler2D _DetailTex;
            float4 _DetailTex_ST;
            sampler2D _DetailTex2;
            float4 _DetailTex2_ST;
            float _NormalOffset;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xyz += v.normal * _NormalOffset;
                o.pos = UnityObjectToClipPos(v.vertex);
                float2 worldCoords = mul(unity_ObjectToWorld, v.vertex).xz;
                o.uv = TRANSFORM_TEX(worldCoords, _DetailTex);
                o.uv2 = TRANSFORM_TEX(worldCoords, _DetailTex2);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.color = v.color;
                TRANSFER_SHADOW(o)
                return o;
            }

            float4 _Color;
            float _Shade;
            float _Density;
            float _DetailStrength;
            float _Detail2Strength;
            float _VertexColorPower;
            float _FinalAlphaMultiply;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed lighting = saturate(dot(i.normal, _WorldSpaceLightPos0)) * SHADOW_ATTENUATION(i);
                if (lighting > LIGHT_THRESHOLD)
                    lighting = 1.0;
                else
                    lighting = 0.0;
                lighting = lerp(lighting, 0, _Shade);
                float4 lightColor = lerp(ShadowColor, LightColor, lighting);
                float detail = lerp(1, tex2D(_DetailTex, i.uv).r, _DetailStrength);
                float detail2 = lerp(1, tex2D(_DetailTex2, i.uv2).r, _Detail2Strength);
                //detail *= detail2;
                fixed4 col = lightColor * _LightColor0.a;
                col.a *= _Density;
                
                col.a *= detail2;
                col.a = pow(col.a * pow(i.color.r, _VertexColorPower), detail);
                col.a = min(1, col.a * _FinalAlphaMultiply);
                col *= _Color;
                
                return col;
            }
            ENDCG
        }
    }
}

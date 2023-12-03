// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRC/Ambient Environment Transparent With Shadows"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" }
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
            };

            float4 LightColor;
            float4 ShadowColor;
            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
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
                fixed4 col = tex2D(_MainTex, i.uv) * lightColor * _LightColor0.a;
                return col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}

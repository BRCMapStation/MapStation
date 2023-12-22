Shader "Winterland/Snow Particle"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _CutoutTex("Cutout Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define LIGHT_THRESHOLD 0.5

            #include "UnityCG.cginc"
            #include "Lighting.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float4 color : COLOR;
            };

            float4 _Color;
            float4 LightColor;
            float4 ShadowColor;
            sampler2D _MainTex;
            sampler2D _CutoutTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed lighting = saturate(dot(i.normal, _WorldSpaceLightPos0));
                if (lighting > LIGHT_THRESHOLD)
                    lighting = 1.0;
                else
                    lighting = 0.0;
                float4 lightColor = lerp(ShadowColor, LightColor, lighting);
                fixed4 col = tex2D(_MainTex, i.uv) * _Color * lightColor * _LightColor0.a;
                fixed4 cutoutCol = tex2D(_CutoutTex, i.uv);
                cutoutCol.r -= -(i.color.a - 1);
                clip(cutoutCol.r);
                return col;
            }
            ENDCG
        }
    }
}

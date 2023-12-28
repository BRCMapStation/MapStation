// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Winterland/Toy with Graffiti"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Graffiti ("Graffiti", 2D) = "white" {}
        _GraffitiVector("Graffiti Forward", Vector) = (0, -1, 0, 0)
        _GraffitiUpVector("Graffiti Up", Vector) = (0, 0, 1, 0)
        _GraffitiPosition("Graffiti Position", Vector) = (0, 0, 0, 0)
        _GraffitiMinDot("Graffiti Miniumm Dot Product", float) = 0
        _GraffitiSize("Graffiti Size", float) = 1
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

            #include "BRCCommon.cginc"
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
                float graffitiForwardDot : TEXCOORD3;
                float2 graffitiUv : TEXCOORD4;
            };

            BRC_LIGHTING_PROPERTIES;
            sampler2D _MainTex;
            sampler2D _Graffiti;
            float4 _MainTex_ST;
            float3 _GraffitiVector;
            float3 _GraffitiUpVector;
            float3 _GraffitiPosition;
            float _GraffitiMinDot;
            float _GraffitiSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                float3 forwardVector = normalize(-_GraffitiVector);
                float3 upVector = normalize(_GraffitiUpVector);
                float3 rightVector = cross(-forwardVector, upVector);

                o.graffitiForwardDot = dot(forwardVector, v.normal);
                float tv = (dot(rightVector, v.vertex) * _GraffitiSize) + dot(_GraffitiPosition, rightVector);
                float tu = (dot(upVector, v.vertex) * _GraffitiSize) + dot(_GraffitiPosition, upVector);
                o.graffitiUv = float2(tu, tv);
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                BRC_LIGHTING_FRAGMENT;
                fixed4 col = tex2D(_MainTex, i.uv) * BRCLighting;
                fixed4 tagCol = tex2D(_Graffiti, i.graffitiUv);
                float graffitiAmount = 0;
                if (i.graffitiForwardDot >= _GraffitiMinDot)
                    graffitiAmount = 1;
                graffitiAmount *= tagCol.a;
                if (i.graffitiUv.x < 0 || i.graffitiUv.x > 1 || i.graffitiUv.y < 0 || i.graffitiUv.y > 1)
                    graffitiAmount = 0;
                col = lerp(col, tagCol, graffitiAmount);
                return col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}

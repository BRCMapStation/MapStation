// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "BRC/Minimap"
{
    Properties
    {
        _ColorLow ("Color Low", Color) = (0.5754716,0,0,0.3490196)
		_ColorHigh ("Color High", Color) = (1,0,0,0.682353)
        _HeightLow ("Height Low", Float) = 0
        _HeightHigh ("Height High", Float) = 1

        [HideInInspector]
        _AnchorOffset("Anchor Offset", Float) = 0
        [HideInInspector]
        _AnchorScale("Anchor Scale", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent+1" }
        Cull Off
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float height : TEXCOORD0;
            };

            float4 _ColorLow;
            float4 _ColorHigh;
            float _HeightLow;
            float _HeightHigh;
            float _AnchorOffset;
            float _AnchorScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.height = ((mul(unity_ObjectToWorld, v.vertex).y - _AnchorOffset) * (1 / _AnchorScale));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float gradientMax = _HeightHigh - _HeightLow;
                float gradientLevel = (i.height - _HeightLow) / gradientMax;
                fixed4 col = lerp(_ColorLow, _ColorHigh, clamp(gradientLevel, 0, 1));
                return col;
            }
            ENDCG
        }
    }
}

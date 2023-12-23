Shader "Winterland/PickupAura"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness("Outline Thickness", Range(0,1)) = 0.1
        _OutlineAnimationLinger("Outline Animation Linger", float) = 2

        _AnimationSpeed("Animation Speed", float) = 1

        _RimPower("Rim Power", float) = 1

        _GrowMinimumDistance("Grow Minimum Distance", float) = 1.5
        _GrowMultiplier("Grow Multiplier", float) = 2
        _GrowMax("Maximum Growth", float) = 5
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _AnimationSpeed;
            float _RimPower;
            float4 _OutlineColor;
            float _OutlineThickness;
            float _OutlineAnimationLinger;

            float GetLinearAnimationMultiplier(){
                float time = _Time * _AnimationSpeed;

                float step = floor(time);

                float animationTime = time - step;

                return animationTime;
            }

            float _GrowMultiplier;
            float _GrowMax;
            float _GrowMinimumDistance;

            v2f vert (appdata v)
            {
                v2f o;
                float scaleAnimation = GetLinearAnimationMultiplier();
                v.vertex.xyz *= scaleAnimation;
                float4 clipPos = UnityObjectToClipPos(v.vertex);
                v.vertex.xyz += (min(_GrowMax, (max(0, max(0,clipPos.w - _GrowMinimumDistance)) * _GrowMultiplier)) * v.normal) * scaleAnimation;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.z += 0.01;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float opacityAnimation = GetLinearAnimationMultiplier();
                opacityAnimation *= opacityAnimation;
                fixed4 col = _Color;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float rim = dot(viewDir, i.normal);
                float finalRim = saturate((-pow(rim, _RimPower) * 1.1)+1);
                col.a = finalRim;
                col.a *= -(opacityAnimation - 1);
                if (rim <= _OutlineThickness)
                {
                    col.rgb = _OutlineColor.rgb;
                    col.a = min(1,col.a * _OutlineAnimationLinger);
                }
                return col;
            }
            ENDCG
        }
    }
}

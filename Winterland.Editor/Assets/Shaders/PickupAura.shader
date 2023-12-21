Shader "Winterland/PickupAura"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _OpacityAnimationSpeed ("Opacity Animation Speed", float) = 1
        _ScaleAnimationSpeed ("Scale Animation Speed", float) = 1

        _OpacitySubtraction("Opacity Animation Subtraction", Range(0,1)) = 1
        _ScaleSubtraction("Scale Animation Subtraction", float) = 1

        _RimPower("Rim Power", float) = 1
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
            float _OpacityAnimationSpeed;
            float _ScaleAnimationSpeed;

            float _OpacitySubtraction;
            float _ScaleSubtraction;

            float ease_in_out_quad(float x) {
	            float t = x; float b = 0; float c = 1; float d = 1;
	            if ((t/=d/2) < 1) return c/2*t*t + b;
	            return -c/2 * ((--t)*(t-2) - 1) + b;
            }

            float GetLinearAnimationMultiplier(float speed, float offset){
                float time = _Time + offset;
                time *= speed;

                float step = floor(time);
                bool even = step % 2 == 0;

                float animationTime = time - step;

                if (even){
                    animationTime = -(animationTime - 1);
                }

                return animationTime;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float scaleAnimation = GetLinearAnimationMultiplier(_ScaleAnimationSpeed, 0);
                v.vertex.xyz -= (v.normal.xyz * _ScaleSubtraction) * scaleAnimation;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.z *= 1.5;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float _RimPower;

            fixed4 frag(v2f i) : SV_Target
            {
                float opacityAnimation = GetLinearAnimationMultiplier(_OpacityAnimationSpeed, 0.5);
                opacityAnimation = ease_in_out_quad(opacityAnimation);
                fixed4 col = _Color;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                col.a = saturate((-pow(dot(viewDir, i.normal), _RimPower) * 1.1)+1);
                col.a = lerp(col.a, col.a - _OpacitySubtraction, opacityAnimation);
                col.a = max(0, col.a);
                return col;
            }
            ENDCG
        }
    }
}

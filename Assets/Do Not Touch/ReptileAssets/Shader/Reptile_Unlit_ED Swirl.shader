Shader "Reptile/Unlit/ED Swirl" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Swirl Color", Vector) = (0.9568627,0.827451,0.2588235,1)
		_BorderMultiplier ("Border Multiplier", Float) = 1.05
		_SwirlControl ("Swirl Control", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}
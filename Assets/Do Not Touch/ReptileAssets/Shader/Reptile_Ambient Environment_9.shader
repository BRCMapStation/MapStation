Shader "Reptile/Ambient Environment" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_Emission ("Emission", 2D) = "black" {}
		[HideInInspector] _Opacity ("Projected Shadow", Range(0, 1)) = 1
		[HideInInspector] _SelfShadowOpacity ("Self Shadow", Range(0, 1)) = 1
		[HideInInspector] _Tendency ("Tendency", Range(-1, 1)) = -0.2
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
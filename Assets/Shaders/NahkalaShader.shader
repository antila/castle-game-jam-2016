Shader "Custom/NahkalaShader" {
	Properties {
		_ColorLow("Color Low", COLOR) = (1,1,1,1)
		_ColorHigh("Color High", COLOR) = (1,1,1,1)
		_yPosLow("Y Pos Low", Float) = 0
		_yPosHigh("Y Pos High", Float) = 10
		_GradientStrength("Graident Strength", Float) = 1
		_EmissiveStrengh("Emissive Strengh ", Float) = 1
		_ColorX("Color X", COLOR) = (1,1,1,1)
		_ColorY("Color Y", COLOR) = (1,1,1,1)

		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#define WHITE3 fixed3(1,1,1)
		#define UP float3(0,1,0)
		#define RIGHT float3(1,0,0)


		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 normal;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _ColorLow;
		fixed4 _ColorHigh;
		fixed4 _ColorX;
		fixed4 _ColorY;
		half _yPosLow;
		half _yPosHigh;
		half _GradientStrength;
		half _EmissiveStrengh;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// gradient color at this height
			half3 gradient = lerp(_ColorLow, _ColorHigh, smoothstep(_yPosLow, _yPosHigh, IN.worldPos.y)).rgb;

			// lerp the 
			gradient = lerp(WHITE3, gradient, _GradientStrength);

			// add ColorX if the normal is facing positive X-ish
			half3 finalColor = _ColorX.rgb * max(0, dot(o.Normal, RIGHT))* _ColorX.a;

			// add ColorY if the normal is facing positive Y-ish (up)
			finalColor += _ColorY.rgb * max(0, dot(o.Normal, UP)) * _ColorY.a;

			// add the gradient color
			finalColor += gradient;

			// scale down to 0-1 values
			finalColor = saturate(finalColor);

			// Albedo comes from a texture tinted by color
			o.Emission = lerp(half3(0, 0, 0), finalColor, _EmissiveStrengh);
			fixed3 c = tex2D(_MainTex, IN.uv_MainTex) * (finalColor * saturate(1 - _EmissiveStrengh));

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

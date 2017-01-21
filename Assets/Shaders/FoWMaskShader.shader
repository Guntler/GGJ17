Shader "Custom/FoWMaskShader" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "LightMode" = "ForwardBase" }
		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting Lambert noambient alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten)
		{
			fixed4 color;
			color.rgb = s.Albedo;
			color.a = s.Alpha;
			return color;
		}

		fixed4 _Color;
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 baseColor = tex2D(_MainTex, IN.uv_MainTex);
			// Albedo comes from a texture tinted by color
			o.Albedo = _Color.rgb * baseColor.b;
			o.Alpha = _Color.a - baseColor.g;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

﻿Shader "SkinnedDecals/Packed Atlas" {
	Properties {
		_MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" {}
		_Color ("Main Color (RGB) Alpha (A)", Color) = (1, 1, 1, 1)
		_ParallaxMap ("Packed Normals (R) Height (G) Normal UP (B) Smoothness (A) Normal RIGHT", 2D) = "bump" {}
		_SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
		_Shininess ("Shininess", Range(0,1)) = 0.5

		_AtlasWidth ("Atlas width", Float) = 4
		_AtlasHeight ("Atlas height", Float) = 4
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+5" }
		Fog { Mode Off }
		Offset -1, -1
		LOD 200

		Blend One OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular finalgbuffer:finalGBuffer exclude_path:forward noshadow noforwardadd

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _ParallaxMap;

		float4 _Color;
		float4 _SpecularColor;
		half _Shininess;

		float _AtlasWidth, _AtlasHeight;

		struct Input {
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {

			// select tile from tileset
			float _Tile = floor((IN.color.a * 2) / (1.0 / 128.0));
			float2 uv = IN.uv_MainTex;
			float tileWidth = 1 / _AtlasWidth;
			float tileHeight = 1 / _AtlasHeight;
			float col = floor(_Tile / _AtlasWidth);
			float row = fmod(_Tile, _AtlasWidth);
			uv.x = row * tileWidth + uv.x * tileWidth;
			uv.y = col * -tileHeight + uv.y * tileHeight - tileHeight;

			// Albedo
			fixed4 c = tex2D (_MainTex, uv);
			o.Albedo = c.rgb * _Color.rgb;

			// Packed Normals
			//R= Height, G= Normal map up, B= Smoothness, A= Normal map right
			fixed4 packedNormals = tex2D(_ParallaxMap, uv);

			// Normal
			o.Normal = UnpackNormalDXT5nm(packedNormals);

			// Specularity & Smoothness
			o.Specular = _SpecularColor.rgb;
			o.Smoothness = packedNormals.b * _Shininess;

			// Alpha
			o.Alpha = c.a * _Color.a;
		}

		void finalGBuffer (Input IN, SurfaceOutputStandardSpecular o, inout half4 diffuse, inout half4 specSmoothness, inout half4 normal, inout half4 emission) {
			diffuse *= o.Alpha;
			specSmoothness.xyz *= o.Alpha;
			specSmoothness.w *= o.Alpha;
			normal *= o.Alpha;
			emission *= o.Alpha;
		}

		ENDCG
	}
	FallBack "Diffuse"
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Mask" {
properties{
	_MaskTex("Mask",2D) = "white"{}
	_MainTex("Main Texture",2D) = "white"{}
}
SubShader {
tags{"Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent"}
zwrite off
Blend SrcAlpha OneMinusSrcAlpha
	Pass {

		CGPROGRAM
		#pragma target 5.0

		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _MaskTex;

		struct vs_input {
			float4 pos : POSITION;
			float2 tex : TEXCOORD0;
		};


		struct ps_input {
			float4 pos : SV_POSITION;
		    float2 tex : TEXCOORD0;
		};

		ps_input vert (vs_input v)
		{
			ps_input o;
			o.pos = UnityObjectToClipPos (v.pos);
			o.tex = v.tex;
			return o;
		}

		fixed4 frag (ps_input i) : SV_Target
		{
			fixed4 color0 = tex2D(_MainTex,i.tex);
			fixed4 color1 = tex2D(_MaskTex,i.tex);
			return fixed4(color0.r,color0.g,color0.b,color1.a);
		}

		ENDCG

	}
}

Fallback Off
}
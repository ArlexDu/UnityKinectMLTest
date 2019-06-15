// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShowHead" {
properties{
	_MaskTex("Mask",2D) = "white"{}
}
SubShader {
tags{"Queue" = "Transparent"}
zwrite off
blend SrcAlpha OneMinusSrcAlpha
Pass {

CGPROGRAM
#pragma target 5.0

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

Texture2D _MainTex;
sampler2D _MaskTex;
sampler sampler_MainTex;

struct vs_input {
	float4 pos : POSITION;
	float2 tex : TEXCOORD0;
};

StructuredBuffer<float> key;

struct ps_input {
	float4 pos : SV_POSITION;
    float2 tex : TEXCOORD0;
    float2 mask: TEXCOORD1;
};

ps_input vert (vs_input v)
{
	ps_input o;
	o.pos = UnityObjectToClipPos (v.pos);
	o.tex = v.tex;
	// Flip x texture coordinate to mimic mirror.
	//o.tex.x = 1 - v.tex.x;
	o.tex.x = key[0]* (1-v.tex.x) + key[1];
	o.tex.y = key[2] * v.tex.y+ key[3];
	o.mask = v.tex;
	return o;
}

float4 frag (ps_input i, in uint id : SV_InstanceID) : COLOR
{
	fixed4 color0 = _MainTex.Sample(sampler_MainTex, i.tex);
	fixed4 color1 = tex2D(_MaskTex,i.mask);
	return fixed4(color0.r,color0.g,color0.b,color1.a);
}

ENDCG

}
}

Fallback Off
}
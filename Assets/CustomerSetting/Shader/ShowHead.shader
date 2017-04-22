Shader "Custom/ShowHead" {
SubShader {
//tags{"DisableBatching" = "True"}
Pass {

CGPROGRAM
#pragma target 5.0

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

Texture2D _MainTex;

sampler sampler_MainTex;

struct vs_input {
	float4 pos : POSITION;
	float2 tex : TEXCOORD0;
};

StructuredBuffer<float> key;

struct ps_input {
	float4 pos : SV_POSITION;
    float2 tex : TEXCOORD0;
};

ps_input vert (vs_input v)
{
	ps_input o;
	o.pos = mul (UNITY_MATRIX_MVP, v.pos);
	o.tex = v.tex;
	// Flip x texture coordinate to mimic mirror.
	o.tex.x = key[0]* (1-v.tex.x) + key[1];
	o.tex.y = key[2] * v.tex.y+ key[3];
	return o;
}

float4 frag (ps_input i, in uint id : SV_InstanceID) : COLOR
{
	float4 o;
	o = _MainTex.Sample(sampler_MainTex, i.tex);

	return o;
}

ENDCG

}
}

Fallback Off
}
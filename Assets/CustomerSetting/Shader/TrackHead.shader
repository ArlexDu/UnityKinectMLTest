Shader "Custom/TrackHead" {
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

StructuredBuffer<int> _Mask;

struct ps_input {
	float4 pos : SV_POSITION;
    float2 tex : TEXCOORD0;
};

ps_input vert (vs_input v)
{
	ps_input o;
	o.pos = mul (UNITY_MATRIX_MVP, v.pos);
	o.tex = v.tex;
	o.tex.x  = 1 - v.tex.x;
	return o;
}

float4 frag (ps_input i, in uint id : SV_InstanceID) : COLOR
{
	float4 o;
	
	int colorWidth = (int)(i.tex.x * (float)1920);
	int colorHeight = (int)(i.tex.y * (float)1080);
	int colorIndex = (int)(colorWidth + colorHeight * (float)1920);
	
	o = float4(0, 0, 0, 1);
	if (_Mask[colorIndex] == 1)
	{
		o = _MainTex.Sample(sampler_MainTex, i.tex);
	}

	return o;
}

ENDCG

}
}

Fallback Off
}
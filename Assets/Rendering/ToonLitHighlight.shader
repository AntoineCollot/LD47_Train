Shader "Toon/LitHighlight" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		
		_Highlight("Highlight", Range(0,1)) = 0
		_HighlightColThresholdLow("Highlight Color Threshold Low", Range(0,1)) = 0
		_HighlightColThresholdHigh("Highlight Color Threshold High", Range(0,1)) = 0
		_HighlightColHigh ("Highlight Color High", Color) = (0.5,0.5,0.5,1)
		_HighlightIntensityHigh ("Highlight Intensity High", Float) =1
		_HighlightColLow ("Highlight Color Low", Color) = (0.5,0.5,0.5,1)
				_NoiseSpeedX("NoiseSpeedX", Float) = 0
				_NoiseSpeedY("NoiseSpeedY", Float) = 0
				_NoiseSpeedZ("NoiseSpeedZ", Float) = 0
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp
		#include "noiseSimplex.cginc"
sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
	c.a = 0;
	return c;
}


sampler2D _MainTex;
float4 _Color;
half4 _HighlightColHigh;
half4 _HighlightColLow;
half _Highlight;
half _HighlightColThresholdHigh;
half _HighlightColThresholdLow;
half _HighlightIntensityHigh;
half _NoiseSpeedX;
half _NoiseSpeedY;
half _NoiseSpeedZ;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
	float3 worldPos;
};

void surf (Input IN, inout SurfaceOutput o) {	
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	
	//Highlight
	half3 noiseUV = IN.worldPos.xyz + float3(_NoiseSpeedX, _NoiseSpeedY,_NoiseSpeedZ) * _Time.y;
	half ns = snoise(noiseUV) / 2 + 0.5f;
	half highColThreshold = step(ns,_HighlightColThresholdLow)+step(_HighlightColThresholdHigh,ns);
	half4 highlightCol = (1-highColThreshold) * _HighlightColHigh * _HighlightIntensityHigh+ highColThreshold * _HighlightColLow;
	c += highlightCol * _Highlight;
	
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
}

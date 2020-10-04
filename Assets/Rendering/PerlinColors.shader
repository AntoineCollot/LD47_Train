// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PerlinColors" {
Properties {
	_Freq ("Frequency", Float) = 1
	
	_LightTreshold ("Light Treshold", Range(0,1)) =0.7
	_MiddleTreshold ("Middle Treshold", Range(0,1)) =0.3
	
	_LightColor("Light Color", Color) = (1,1,1,1)
	_MiddleColor("Middle Color", Color) = (1,1,1,1)
	_DarkColor("Dark Color", Color) = (1,1,1,1)
}

SubShader {
	Pass {
		CGPROGRAM
		
		#pragma target 3.0
		
		#pragma vertex vert
		#pragma fragment frag
		
		#include "noiseSimplex.cginc"
		
		struct v2f {
			float4 pos : SV_POSITION;
			float3 srcPos : TEXCOORD0;
		};
		
		uniform float _Freq;
		uniform float3 _Position;
		
		v2f vert(float4 objPos : POSITION)
		{
			v2f o;

			o.pos =	UnityObjectToClipPos(objPos);
			
			o.srcPos = mul(unity_ObjectToWorld, objPos).xyz;
			o.srcPos *= _Freq;
			o.srcPos += _Position;
			
			return o;
		}
		
		fixed4 _LightColor;
		fixed4 _MiddleColor;
		fixed4 _DarkColor;
		half _LightTreshold;
		half _MiddleTreshold;
		
		float4 frag(v2f i) : COLOR
		{
			float ns = snoise(i.srcPos) / 2 + 0.5f;
			
			fixed4 col = _DarkColor;
			col = step(_LightTreshold,ns) * col + step(ns,_LightTreshold) * _LightColor;
			col = step(_MiddleTreshold,ns) * col + step(ns,_MiddleTreshold) * _MiddleColor;

			
			return col;
		}
		
		ENDCG
	}
}

}
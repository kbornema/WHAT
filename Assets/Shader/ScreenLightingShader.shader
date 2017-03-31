Shader "ScreenEffect/ScreenLighting"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MultTex("MultTexture", 2D) = "white" {}
		_Saturation("Saturation", Range(0.0, 2.0)) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off 
		ZWrite Off 
		//used to make the "depth" of the cameras working:
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vertexShader
			#pragma fragment fragmentShader
			
			#include "UnityCG.cginc"

			struct vertInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct fragInput
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			fragInput vertexShader(vertInput input)
			{
				fragInput output;
				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
				output.uv = input.uv;
				return output;
			}
			
			sampler2D _MainTex;
			sampler2D _MultTex;
			float _Saturation;

			float grayout(float3 input)
			{
				return dot(input, float3(0.33, 0.33, 0.33));
			}

			float4 fragmentShader(fragInput input) : SV_Target
			{		
				float4 shadowColor = tex2D(_MultTex, input.uv);
				float4 worldColor = tex2D(_MainTex, input.uv);

				float4 finalColor = worldColor * shadowColor;

				float gray = grayout(finalColor.rgb);

				return (1.0 - _Saturation) * float4(gray, gray, gray, finalColor.a) + _Saturation * finalColor;
			}

			ENDCG
		}
	}
}

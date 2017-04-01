Shader "ScreenEffect/InvertColors"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Factor("Factor", Range(0.0, 1.0)) = 1.0
		_CutLeft("CutLeft", Range(0.0, 1.0)) = 0.0
		_CutRight("CutRight", Range(0.0, 1.0)) = 0.0
		_CutUp("CutUp", Range(0.0, 1.0)) = 0.0
		_CutDown("CutDown", Range(0.0, 1.0)) = 0.0
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
			float _Factor;
			float _CutLeft;
			float _CutRight;
			float _CutUp;
			float _CutDown;


			float3 invert(float3 inColor)
			{
				return abs(_Factor * float3(1.0, 1.0, 1.0) - inColor);
			}
			
			float4 fragmentShader(fragInput input) : SV_Target
			{		
				float4 worldColor = tex2D(_MainTex, input.uv);
				float4 finalColor = worldColor;

				if(input.uv.x < _CutLeft)
				{
					finalColor.rgb = invert(finalColor.rgb);
				}

				if(input.uv.x > _CutRight)
				{
					finalColor.rgb = invert(finalColor.rgb);
				}

				if(input.uv.y < _CutUp)
				{
					finalColor.rgb = invert(finalColor.rgb);
				}

				if(input.uv.y > _CutDown)
				{
					finalColor.rgb = invert(finalColor.rgb);
				}

				return finalColor;
			}

			ENDCG
		}
	}
}

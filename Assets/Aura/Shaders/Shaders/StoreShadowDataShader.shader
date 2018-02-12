///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                                                                                                                                             ///
///     MIT License                                                                                                                                             ///
///                                                                                                                                                             ///
///     Copyright (c) 2016 Raphaël Ernaelsten (@RaphErnaelsten)                                                                                                 ///
///                                                                                                                                                             ///
///     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),      ///
///     to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,                  ///
///     and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:              ///
///                                                                                                                                                             ///
///     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                          ///
///                                                                                                                                                             ///
///     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     ///
///     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER      ///
///     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS    ///
///     IN THE SOFTWARE.                                                                                                                                        ///
///                                                                                                                                                             ///
///     PLEASE CONSIDER CREDITING AURA IN YOUR PROJECTS. IF RELEVANT, USE THE UNMODIFIED LOGO PROVIDED IN THE "LICENSE" FOLDER.                                 ///
///                                                                                                                                                             ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Shader "Hidden/Aura/StoreShadowDataShader"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			inline float GetMask(inout float2 borders, double stepSize, float u)
			{
				borders += stepSize;
				return step(borders.x, u) - step(borders.y, u);
			}

			#define CORRECT_IF_INFINITY(value) (isinf(value) ? 0 : value) // BUG #922780 https://issuetracker.unity3d.com/issues/graphics-directional-shadow-data-are-set-to-infinity-when-unused

			fixed4 frag(v2f i) : SV_Target
			{
				// Texture must be power of 2 (for Texture2DArray) so it is hardcoded to 32 pixels wide
				const double stepSize = 1.0 / 32.0;
				float4 outColor = float4(0,0,0,0);
				float2 borders = float2(-stepSize, 0);

				//float4 unity_ShadowSplitSqRadii; // 1 pixel
				outColor += CORRECT_IF_INFINITY(unity_ShadowSplitSqRadii) * GetMask(borders,stepSize, i.uv.x);

				//float4 _LightSplitsNear; // 1 pixel
				outColor += CORRECT_IF_INFINITY(_LightSplitsNear) * GetMask(borders, stepSize, i.uv.x);

				//float4 _LightSplitsFar;  // 1 pixel
				outColor += CORRECT_IF_INFINITY(_LightSplitsFar) * GetMask(borders, stepSize, i.uv.x);

				//float4 unity_ShadowSplitSpheres[4]; // 4 pixels
				outColor += CORRECT_IF_INFINITY(unity_ShadowSplitSpheres[0]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_ShadowSplitSpheres[1]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_ShadowSplitSpheres[2]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_ShadowSplitSpheres[3]) * GetMask(borders, stepSize, i.uv.x);

				//float4x4 unity_WorldToShadow[4]; // 16 pixels
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[0][0]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[0][1]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[0][2]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[0][3]) * GetMask(borders, stepSize, i.uv.x);

				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[1][0]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[1][1]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[1][2]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[1][3]) * GetMask(borders, stepSize, i.uv.x);

				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[2][0]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[2][1]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[2][2]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[2][3]) * GetMask(borders, stepSize, i.uv.x);

				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[3][0]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[3][1]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[3][2]) * GetMask(borders, stepSize, i.uv.x);
				outColor += CORRECT_IF_INFINITY(unity_WorldToShadow[3][3]) * GetMask(borders, stepSize, i.uv.x);

				//float4 _LightShadowData;  // 1 pixel
				outColor += CORRECT_IF_INFINITY(_LightShadowData) * GetMask(borders, stepSize, i.uv.x);

				return outColor;
			}
			ENDCG
		}
	}
}

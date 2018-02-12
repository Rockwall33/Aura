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

Shader "Hidden/Aura/StorePointLightShadowMap"
{
	SubShader
	{
		Pass
		{
			ZTest Off
			Cull Front
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma shader_feature SHADOWS_CUBE
			#pragma shader_feature POINT

			#ifdef SHADOWS_DEPTH
			#define SHADOWS_NATIVE
			#endif
		
			#include "UnityCG.cginc"
			#include "UnityShadowLibrary.cginc"
			#include "../Aura.cginc"
		
			float4x4 _WorldViewProj;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(_WorldViewProj, v.vertex);
				o.uv = ComputeScreenPos(o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
			#undef DIRECTIONAL
			#undef DIRECTIONAL_COOKIE
			#undef SHADOWS_SCREEN
			#undef SPOT
			#undef SHADOWS_DEPTH
			#undef POINT_COOKIE

	#if defined(POINT) && defined(SHADOWS_CUBE)
				float2 uv = i.uv.xy / i.uv.w;
				float3 ray = GetNormalizedVectorFromNormalizedYawPitch(uv);
				float depth = SampleCubeDistance(ray);
				return float4(depth, _LightPositionRange.w, 0, 0);
	#else
				return float4(1, 0.5, 1, 1);
	#endif
			}
			ENDCG
		}
	}
}

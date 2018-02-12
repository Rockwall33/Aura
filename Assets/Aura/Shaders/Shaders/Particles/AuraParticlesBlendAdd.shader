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

Shader "Aura/Particles/Blend Add"
{
	Properties
	{
		[Header(Aura usage properties)][Space][KeywordEnum(Pixel, Vertex)] _UsageStage("Stage", Float) = 0
		[KeywordEnum(Light, Fog, Both)] _UsageType("Type", Float) = 0
		_LightingFactor("Lighting Factor", Float) = 1

		[Header(Properties)][Space]_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_SoftParticleDistanceFade("Soft Particles Distance Fade", Float) = 0.1
	}

	Category
	{
		SubShader
		{
			Pass
			{
			
				Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
				ColorMask RGB
				Cull Off Lighting Off ZWrite Off

				Blend One OneMinusSrcAlpha
					
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma shader_feature _USAGESTAGE_VERTEX _USAGESTAGE_PIXEL
					#pragma shader_feature _USAGETYPE_LIGHT _USAGETYPE_FOG _USAGETYPE_BOTH

					#define PREMULTIPLY_ALPHA(color) color.xyz *= color.w;

					float _LightingFactor;
					sampler2D _MainTex;
					float4 _MainTex_ST;
					fixed4 _TintColor;
					sampler2D_float _CameraDepthTexture;
					float _SoftParticleDistanceFade;
						
					#include "UnityCG.cginc"
					#include "../../Includes/AuraParticlesUsage.cginc"
				ENDCG 
			}
		}	
	}
}

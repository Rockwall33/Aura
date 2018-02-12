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

Shader "Aura/Standard Alpha Blend"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 200
   
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows alpha finalcolor:Aura_Fog
		#pragma target 5.0

		sampler2D _MainTex;
		sampler2D _CameraDepthTexture;

		// TODO : MAKE THIS PROPER WITH INCLUDES (SURFACE SHADERS ARE A NIGHTMARE)
		float4 Aura_FrustumRange;
		sampler3D Aura_VolumetricLightingTexture;
		float InverseLerp(float lowThreshold, float hiThreshold, float value)
		{
			return (value - lowThreshold) / (hiThreshold - lowThreshold);
		}
		float4 Aura_GetFogValue(float3 screenSpacePosition)
		{
			return tex3Dlod(Aura_VolumetricLightingTexture, float4(screenSpacePosition, 0));
		}
		void Aura_ApplyFog(inout fixed4 colorToApply, float3 screenSpacePosition)
		{    
			float4 fogValue = Aura_GetFogValue(screenSpacePosition);
			// Always apply fog attenuation - also in the forward add pass.
			colorToApply.xyz *= fogValue.w;
			// Alpha premultiply mode (used with alpha and Standard lighting function, or explicitly alpha:premul)
			#if _ALPHAPREMULTIPLY_ON
			fogValue.xyz *= colorToApply.w;
			#endif
			// Add inscattering only once, so in forward base, but not forward add.
			#ifndef UNITY_PASS_FORWARDADD
			colorToApply.xyz += fogValue.xyz;
			#endif
		}
		
		struct Input
		{
			float2 uv_MainTex;
			float4 screenPos;
		};
		
		// From https://github.com/Unity-Technologies/VolumetricLighting/blob/master/Assets/Scenes/Materials/StandardAlphaBlended-VolumetricFog.shader
		void Aura_Fog(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
				half3 screenSpacePosition = IN.screenPos.xyz/IN.screenPos.w;
				screenSpacePosition.z = InverseLerp(Aura_FrustumRange.x, Aura_FrustumRange.y, LinearEyeDepth(screenSpacePosition.z));
				Aura_ApplyFog(color, screenSpacePosition);
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
 
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Standard"
}
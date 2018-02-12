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

#include "Common.cginc"

#define USE_AURA

float4 Aura_FrustumRange;
sampler3D Aura_VolumetricDataTexture;
sampler3D Aura_VolumetricLightingTexture;

//////////// Helper functions
float Aura_RescaleDepth(float depth)
{
    half rescaledDepth = InverseLerp(Aura_FrustumRange.x, Aura_FrustumRange.y, depth);
    return GetBiasedNormalizedDepth(rescaledDepth);
}

float3 Aura_GetFrustumSpaceCoordinates(float4 inVertex)
{
    float4 clipPos = UnityObjectToClipPos(inVertex);

    float z = -UnityObjectToViewPos(inVertex).z;

    float4 cameraPos = ComputeScreenPos(clipPos);
    cameraPos.xy /= cameraPos.w;
    cameraPos.z = z;

    return cameraPos.xyz;
}

//////////// Lighting
float4 Aura_SampleLightingTexture(float3 position)
{
	return tex3Dlod(Aura_VolumetricDataTexture, float4(position, 0));
}
float4 Aura_GetLightingValue(float3 screenSpacePosition)
{
    return Aura_SampleLightingTexture(float3(screenSpacePosition.xy, Aura_RescaleDepth(screenSpacePosition.z)));
}

void Aura_ApplyLighting(inout float3 colorToApply, float3 screenSpacePosition, float lightingFactor)
{
    screenSpacePosition.xy += GetBlueNoise(screenSpacePosition.xy, 1).xy;

    float3 lightingValue = Aura_GetLightingValue(screenSpacePosition).xyz * lightingFactor;

	float3 noise = GetBlueNoise(screenSpacePosition.xy, 2).xyz;

	colorToApply = lightingValue + noise;
}

//////////// Fog
float4 Aura_GetFogValue(float3 screenSpacePosition)
{
    return tex3Dlod(Aura_VolumetricLightingTexture, float4(screenSpacePosition.xy, Aura_RescaleDepth(screenSpacePosition.z), 0));
}

void Aura_ApplyFog(inout float3 colorToApply, float3 screenSpacePosition)
{
    screenSpacePosition.xy += GetBlueNoise(screenSpacePosition.xy, 3).xy;

    float4 fogValue = Aura_GetFogValue(screenSpacePosition);
    float4 noise = GetBlueNoise(screenSpacePosition.xy, 4);

	colorToApply = colorToApply * (fogValue.w + noise.w) + (fogValue.xyz + noise.xyz);
}

// From https://github.com/Unity-Technologies/VolumetricLighting/blob/master/Assets/Scenes/Materials/StandardAlphaBlended-VolumetricFog.shader
void Aura_ApplyFog(inout float4 colorToApply, float3 screenSpacePosition)
{
    screenSpacePosition.xy += GetBlueNoise(screenSpacePosition.xy, 5).xy;
    
	float4 fogValue = Aura_GetFogValue(screenSpacePosition);
    
	float4 noise = GetBlueNoise(screenSpacePosition.xy, 6);

	// Always apply fog attenuation - also in the forward add pass.
    colorToApply.xyz *= (fogValue.w + noise.w);

	// Alpha premultiply mode (used with alpha and Standard lighting function, or explicitly alpha:premul)
	#if _ALPHAPREMULTIPLY_ON
	fogValue.xyz *= colorToApply.w;
	#endif

	// Add inscattering only once, so in forward base, but not forward add.
	#ifndef UNITY_PASS_FORWARDADD
    colorToApply.xyz += (fogValue.xyz + noise.xyz);
	#endif
} 
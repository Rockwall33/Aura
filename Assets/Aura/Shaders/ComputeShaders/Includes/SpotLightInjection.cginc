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

uint spotLightCount;
StructuredBuffer<SpotLightParameters> spotLightDataBuffer;
Texture2DArray<half> spotShadowMapsArray;
Texture2DArray<half> spotCookieMapsArray;

half SampleSpotShadowMap(SpotLightParameters lightParameters, half4 shadowPosition, half2 offset)
{
	// TODO : CHECK FOR SUPPOSED OFFSET
	half shadowMapValue = 1.0f - spotShadowMapsArray.SampleLevel(_LinearClamp, half3((shadowPosition.xy + offset) / shadowPosition.w, lightParameters.shadowMapIndex), 0);
	return step(shadowPosition.z / shadowPosition.w, shadowMapValue);
}

void ComputeSpotLightInjection(SpotLightParameters lightParameters, half3 worldPosition, half3 viewVector, inout half4 accumulationColor, half anisotropy)
{
	half3 lightVector = normalize(worldPosition - lightParameters.lightPosition);
	half cosAngle = dot(lightParameters.lightDirection.xyz, lightVector);
	half dist = distance(lightParameters.lightPosition.xyz, worldPosition);

    [branch]
	if (dist > lightParameters.lightRange || cosAngle < lightParameters.lightCosHalfAngle)
	{
		return;
	}
	else
	{
        half anisotropyCosAngle = dot(-lightVector, viewVector);
		half anisotropyFactor = GetAnisotropyFactor(anisotropyCosAngle, anisotropy);
		half attenuation = anisotropyFactor;
        
		half4 lightPos = mul(lightParameters.worldToShadowMatrix.ToMatrix(), half4(worldPosition, 1));
		half normalizedDistance = saturate(lightPos.z / lightParameters.lightRange);
        
		attenuation *= GetLightDistanceAttenuation(lightParameters.distanceFalloffParameters, normalizedDistance);
        
		half angleAttenuation = 1;
		angleAttenuation = smoothstep(lightParameters.lightCosHalfAngle, lerp(1, lightParameters.lightCosHalfAngle, lightParameters.angularFalloffParameters.x), cosAngle);
		angleAttenuation = pow(angleAttenuation, lightParameters.angularFalloffParameters.y);
		attenuation *= angleAttenuation;
        
        #if defined(ENABLE_SPOT_LIGHTS_SHADOWS)
        [branch]
		if (lightParameters.shadowMapIndex > -1)
		{
			half shadowAttenuation = SampleSpotShadowMap(lightParameters, lightPos, 0);
			shadowAttenuation = lerp(lightParameters.shadowStrength, 1.0f, shadowAttenuation);
			
			attenuation *= shadowAttenuation;
		}
        #endif
        
        #if defined(ENABLE_LIGHTS_COOKIES)
        [branch]
		if (lightParameters.cookieMapIndex > -1)
		{        
			half cookieMapValue = spotCookieMapsArray.SampleLevel(_LinearRepeat, half3(lightPos.xy / lightPos.w, lightParameters.cookieMapIndex), 0).x;        
            cookieMapValue = lerp(1, cookieMapValue, pow(smoothstep(lightParameters.cookieParameters.x, lightParameters.cookieParameters.y, normalizedDistance), lightParameters.cookieParameters.z));
        
			attenuation *= cookieMapValue;
		}
        #endif
        
		accumulationColor.xyz += lightParameters.color * attenuation;
	}
}

void ComputeSpotLightsInjection(half3 worldPosition, half3 viewVector, inout half4 accumulationColor, half anisotropy)
{
    [allow_uav_condition]
	for (uint i = 0; i < spotLightCount; ++i)
	{
		ComputeSpotLightInjection(spotLightDataBuffer[i], worldPosition, viewVector, accumulationColor, anisotropy);
	}
}
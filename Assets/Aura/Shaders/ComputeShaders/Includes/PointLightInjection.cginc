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

uint pointLightCount;
StructuredBuffer<PointLightParameters> pointLightDataBuffer;
Texture2DArray<half2> pointShadowMapsArray;
Texture2DArray<half> pointCookieMapsArray;

half SamplePointShadowMap(PointLightParameters lightParameters, half3 samplingDirection, half2 polarCoordinates)
{
	half2 shadowMapValue = pointShadowMapsArray.SampleLevel(_LinearClamp, half3(polarCoordinates, lightParameters.shadowMapIndex), 0).xy;
    half biasedReferenceDistance = length(samplingDirection) * shadowMapValue.y;
        biasedReferenceDistance *= 0.97f; // bias
	return step(biasedReferenceDistance, shadowMapValue.x);
}

void ComputePointLightInjection(PointLightParameters lightParameters, half3 worldPosition, half3 viewVector, inout half4 accumulationColor, half anisotropy)
{
	half3 lightVector = worldPosition - lightParameters.lightPosition;
    half3 normalizedLightVector = normalize(lightVector);
	half dist = distance(lightParameters.lightPosition, worldPosition);

	[branch]
	if (dist > lightParameters.lightRange)
	{
		return; 
	}
	else
	{
		half normalizedDistance = saturate(dist / lightParameters.lightRange);
        half anisotropyCosAngle = dot(-normalizedLightVector, viewVector);
		half anisotropyFactor = GetAnisotropyFactor(anisotropyCosAngle, anisotropy);
		half attenuation = anisotropyFactor;
	
		attenuation *= GetLightDistanceAttenuation(lightParameters.distanceFalloffParameters, normalizedDistance);
		
		#if defined(ENABLE_POINT_LIGHTS_SHADOWS) || defined(ENABLE_LIGHTS_COOKIES)
        half2 polarCoordinates = GetNormalizedYawPitchFromNormalizedVector(normalizedLightVector);
		#endif

		#if defined(ENABLE_POINT_LIGHTS_SHADOWS)
		[branch]
		if (lightParameters.shadowMapIndex > -1)
		{
			half shadowAttenuation = SamplePointShadowMap(lightParameters, lightVector, polarCoordinates);
			shadowAttenuation = lerp(lightParameters.shadowStrength, 1.0f, shadowAttenuation);
		
			attenuation *= shadowAttenuation;
		}
		#endif
	
		#if defined(ENABLE_LIGHTS_COOKIES)
		[branch]
		if (lightParameters.cookieMapIndex > -1)
		{        
			half cookieMapValue = pointCookieMapsArray.SampleLevel(_LinearClamp, half3(polarCoordinates, lightParameters.cookieMapIndex), 0).x;
			cookieMapValue = lerp(1, cookieMapValue, pow(smoothstep(lightParameters.cookieParameters.x, lightParameters.cookieParameters.y, normalizedDistance), lightParameters.cookieParameters.z));
        
			attenuation *= cookieMapValue;
		}
		#endif
	
		accumulationColor.xyz += lightParameters.color * attenuation;
	}
}

void ComputePointLightsInjection(half3 worldPosition, half3 viewVector, inout half4 accumulationColor, half anisotropy)
{
	[allow_uav_condition]
	for (uint i = 0; i < pointLightCount; ++i)
	{
		ComputePointLightInjection(pointLightDataBuffer[i], worldPosition, viewVector, accumulationColor, anisotropy);
	}
}
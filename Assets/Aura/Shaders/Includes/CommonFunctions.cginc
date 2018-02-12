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

///-----------------------------------------------------------------------------------------
///			Noise functions
///-----------------------------------------------------------------------------------------
#include "Noise.cginc"

///-----------------------------------------------------------------------------------------
///			GetBiasedDepth
///			Bias the depth towards the camera
///-----------------------------------------------------------------------------------------
float GetBiasedNormalizedDepth(float normalizedDepth)
{
    //return pow(normalizedDepth, normalizedDepthBiasExponent);
    return normalizedDepth;
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalPosition
///			Gets the normalized coordinates from the thread id and the bufferResolution
///-----------------------------------------------------------------------------------------
float3 GetNormalizedLocalPosition(uint3 id)
{
    return ((float3) id + 0.5f) / bufferResolution.xyz;
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalPositionWithDepthBias
///			Gets the volume normalized coordinates with a depth biased towards the camera
///-----------------------------------------------------------------------------------------
float3 GetNormalizedLocalPositionWithDepthBias(uint3 id)
{
    float3 normalizedLocalPosWithDepthBias = GetNormalizedLocalPosition(id);
    normalizedLocalPosWithDepthBias.z = GetBiasedNormalizedDepth(normalizedLocalPosWithDepthBias.z);

    return normalizedLocalPosWithDepthBias;
}

///-----------------------------------------------------------------------------------------
///			GetWorldPosition
///			Gets the world position from normalized coordinates and the corners' position of the frustum
///-----------------------------------------------------------------------------------------
float3 GetWorldPosition(float3 normalizedLocalPos, half4 cornersPosition[8])
{
	float3 AtoB = lerp(cornersPosition[0].xyz, cornersPosition[1].xyz, normalizedLocalPos.x);
	float3 DtoC = lerp(cornersPosition[3].xyz, cornersPosition[2].xyz, normalizedLocalPos.x);
	float3 nearBottomToTop = lerp(DtoC, AtoB, normalizedLocalPos.y);

	float3 EtoF = lerp(cornersPosition[4].xyz, cornersPosition[5].xyz, normalizedLocalPos.x);
	float3 HtoG = lerp(cornersPosition[7].xyz, cornersPosition[6].xyz, normalizedLocalPos.x);
	float3 farBottomToTop = lerp(HtoG, EtoF, normalizedLocalPos.y);

	float3 worldPosition = lerp(nearBottomToTop, farBottomToTop, normalizedLocalPos.z);

	return worldPosition;
}

///-----------------------------------------------------------------------------------------
///			TransformPositions
///			Gets the 3d texture coordinates to be used with combined Texture 3D
///-----------------------------------------------------------------------------------------
float3 TransformPoint(float3 p, float4x4 transform)
{
	return mul(transform, float4(p, 1)).xyz;
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedYawPitchFromNormalizedVector
///			Compute normalized Yaw Pitch angles from a normalized direction vector
///-----------------------------------------------------------------------------------------
float2 GetNormalizedYawPitchFromNormalizedVector(float3 NormalizedVector)
{
	const float InvPi = 0.31830988618379067153776752674503f;
	const float TwoInvPi = 2.0f * InvPi;
	float Yaw = (atan2(NormalizedVector.z, NormalizedVector.x) * InvPi + 1.0f) * 0.5f;
	float Pitch = (asin(NormalizedVector.y) * TwoInvPi + 1.0f) * 0.5f;

	return float2(Yaw, Pitch);
}
///-----------------------------------------------------------------------------------------
///			GetNormalizedVectorFromNormalizedYawPitch
///			Compute a normalized direction vector from normalized Yaw Pitch angles
///-----------------------------------------------------------------------------------------
float3 GetNormalizedVectorFromNormalizedYawPitch(float Yaw, float Pitch)
{
	const float Pi = 3.1415926535897932384626433832795f;
	const float HalfPi = Pi * 0.5f;
	Yaw = (Yaw * 2.0f - 1.0f) * Pi;
	Pitch = (Pitch * 2.0f - 1.0f) * HalfPi;
	return float3(cos(Yaw) * cos(Pitch), sin(Pitch), cos(Pitch) * sin(Yaw));
}
float3 GetNormalizedVectorFromNormalizedYawPitch(float2 YawPitch)
{
	return GetNormalizedVectorFromNormalizedYawPitch(YawPitch.x, YawPitch.y);
}

///-----------------------------------------------------------------------------------------
///			GetCombinedTexture3dCoordinates
///			Gets the 3d texture coordinates to be used with combined Texture 3D
///-----------------------------------------------------------------------------------------
float3 GetCombinedTexture3dCoordinates(float3 positions, float textureWidth, float textureDepth, float index, float4x4 transform, int wrapMode, int filterMode)
{
	float textureCount = textureDepth / textureWidth;
	float borderClamp = 0.5f / textureWidth;
	float offset = index / textureCount;

	float3 textureCoordinates = frac(TransformPoint(positions, transform) + 0.5f);
	textureCoordinates.z /= textureCount;
	textureCoordinates.z += offset;
	textureCoordinates.z = clamp(offset + borderClamp, offset + 1.0f - borderClamp, textureCoordinates.z);

    if(filterMode < 1)
    {
        textureCoordinates = (floor(textureCoordinates * textureWidth) + 0.5f) / textureWidth;
    }

	return textureCoordinates;
}

///-----------------------------------------------------------------------------------------
///			GetExponentialValue
///			Gets "exponentialized" value based on 0->1 gradient
///-----------------------------------------------------------------------------------------
float GetExponentialValue(float value)
{
	return pow(abs(value), e);
}
///-----------------------------------------------------------------------------------------
///			GetLogarithmicValue
///			Gets "logarithmized" value based on 0->1 gradient
///-----------------------------------------------------------------------------------------
float GetLogarithmicValue(float value)
{
	return pow(abs(value), n);
}

///-----------------------------------------------------------------------------------------
///			(Clamped)InverseLerp
///			Gets the linear gradient, returning where the value locates between the low and hi thresholds
///-----------------------------------------------------------------------------------------
float InverseLerp(float lowThreshold, float hiThreshold, float value)
{
	return (value - lowThreshold) / (hiThreshold - lowThreshold);
}
float ClampedInverseLerp(float lowThreshold, float hiThreshold, float value)
{
	return saturate(InverseLerp(lowThreshold, hiThreshold, value));
}
float3 InverseLerp(float lowThreshold, float hiThreshold, float3 value)
{
	return (value - lowThreshold) / (hiThreshold - lowThreshold);
}
float3 ClampedInverseLerp(float lowThreshold, float hiThreshold, float3 value)
{
	return saturate(InverseLerp(lowThreshold, hiThreshold, value));
}

///-----------------------------------------------------------------------------------------
///			LevelValue
///			Filters value between "levelLowThreshold" and "levelHiThreshold", contrast by "contrast" factor, then rescale the result between "outputLowValue" and "outputHiValue". Similar to the Levels adjustment tool in Photoshop.
///-----------------------------------------------------------------------------------------
float LevelValue(VolumeLevelsData levelsParameters, float value)
{
	float tmp = ClampedInverseLerp(levelsParameters.levelLowThreshold, levelsParameters.levelHiThreshold, value);
	tmp = saturate(lerp(0.5f, tmp, levelsParameters.contrast));
	tmp = lerp(levelsParameters.outputLowValue, levelsParameters.outputHiValue, tmp);

	return tmp;
}
float3 LevelValue(VolumeLevelsData levelsParameters, float3 value)
{
	float3 tmp;
	tmp.x = LevelValue(levelsParameters, value.x);
	tmp.y = LevelValue(levelsParameters, value.y);
	tmp.z = LevelValue(levelsParameters, value.z);

	return tmp;
}

//-----------------------------------------------------------------------------------------
//			GetAnisotropyFactor
//			Henyey–Greenstein phase function. Returns the anisotropic scattering factor.
//-----------------------------------------------------------------------------------------
float GetAnisotropyFactor(float cosAngle, float coefficient)
{
	float squareCoefficient = coefficient * coefficient;
	//return res = (1.0 / 4.0 * pi) * (1.0 - squareCoefficient) / pow(1.0 + squareCoefficient - 2.0 * coefficient * cosAngle, 3.0 / 2.0);
	return pi * 0.25 * (1.0 - squareCoefficient) / pow(abs(1.0 + squareCoefficient - 2.0 * coefficient * cosAngle), 0.75);
}

//-----------------------------------------------------------------------------------------
//			GetLinearDepth
//			Linearize depth/shadow maps
//			
//			Params : Values used to linearize the Z buffer (http://www.humus.name/temp/Linearize%20depth.txt)
//          x = 1-far/near
//          y = far/near
//          z = x/far
//          w = y/far
//          or in case of a reversed depth buffer (UNITY_REVERSED_Z is 1) -> Our case
//          x = -1+far/near
//          y = 1
//          z = x/far
//          w = 1/far
//-----------------------------------------------------------------------------------------
float GetLinearDepth(float depth, float4 params)
{
	return 1.0 / (params.z * depth + params.w);
}
float GetLinearDepth01(float depth, float4 params)
{
	return 1.0 / (params.z * depth + params.y);
}

/*
//-----------------------------------------------------------------------------------------
//			NormalizedDepthToLinearDepth
//			LinearDepthToNormalizedDepth
//			Bias the depth
//-----------------------------------------------------------------------------------------
float NormalizedDepthToLinearDepth(float normalizedDepth)
{
	float nearClip = 
	float depthRangeSize = cameraRanges.y - cameraRanges.x;
	return pow(normalizedDepth, normalizedDepthBiasExponent) * depthRangeSize + cameraRanges.x;
}
float NormalizedDepthToLinearDepth(float normalizedDepth, float2 volumeFrustumRanges)
{
	float depthRangeSize = volumeFrustumRanges.y - volumeFrustumRanges.x;
	return pow(normalizedDepth, normalizedDepthBiasExponent) * depthRangeSize + volumeFrustumRanges.x;
}
float LinearDepthToNormalizedDepth(float linearDepth)
{
	float depthRangeSize = cameraRanges.y - cameraRanges.x;
	return pow((linearDepth - cameraRanges.x) / depthRangeSize, 1.0f / normalizedDepthBiasExponent);
}
*/

//-----------------------------------------------------------------------------------------
//			GetLightDistanceAttenuation
//			Computes the distance attenuation factor for Point and Spot lights
//-----------------------------------------------------------------------------------------
half GetLightDistanceAttenuation(half2 distanceFalloffParameters, half normalizedDistance)
{
    float distanceAttenuation = ClampedInverseLerp(1, distanceFalloffParameters.x, normalizedDistance);
    distanceAttenuation = pow(distanceAttenuation, distanceFalloffParameters.y);

    return distanceAttenuation;
}

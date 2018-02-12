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

///
///			MatrixFloats
///
struct MatrixFloats
{
	half4 a;
	half4 b;
	half4 c;
	half4 d;

	half4x4 ToMatrix()
	{
		return half4x4(half4(a.x, b.x, c.x, d.x), half4(a.y, b.y, c.y, d.y), half4(a.z, b.z, c.z, d.z), half4(a.w, b.w, c.w, d.w));
	}
};

///
///			VolumeLevelsData
///
struct VolumeLevelsData
{
	half levelLowThreshold;
	half levelHiThreshold;
	half outputLowValue;
	half outputHiValue;
	half contrast;
};

///
///			VolumetricTextureData
///
struct VolumetricTextureData
{
	MatrixFloats transform;
	int index;
    int wrapMode;
    int filterMode;
    int clipOnAlpha;
    half clippingThreshold;
};

///
///			VolumetricNoiseData
///
struct VolumetricNoiseData
{
	int enable;
	MatrixFloats transform;
	half speed;
	half offset;
};

///
///			VolumeData
///
struct VolumeData
{
	MatrixFloats transform;
	int shape;
	/*
		Global      = 0
		Planar      = 1
		Box         = 2
		Sphere      = 3
		Cylinder    = 4
		Cone        = 5
	*/
	half falloffExponent;
	half xPositiveFade;
	half xNegativeFade;
	half yPositiveFade;
	half yNegativeFade;
	half zPositiveFade;
	half zNegativeFade;
	VolumetricTextureData textureData;
	VolumetricNoiseData noiseData;
	int injectDensity;
	half densityValue;
    VolumeLevelsData densityTextureLevelsParameters;
    VolumeLevelsData densityNoiseLevelsParameters;
    int injectAnisotropy;
    half anisotropyValue;
    VolumeLevelsData anisotropyTextureLevelsParameters;
    VolumeLevelsData anisotropyNoiseLevelsParameters;
    int injectColor;
    half3 colorValue;
    VolumeLevelsData colorTextureLevelsParameters;
    VolumeLevelsData colorNoiseLevelsParameters;
};

///
///			DirectionalShadowData
///
struct DirectionalShadowData
{
	half4 shadowSplitSqRadii;
	half4 lightSplitsNear;
	half4 lightSplitsFar;
	half4 shadowSplitSpheres[4];
	half4x4 world2Shadow[4];
	half4 lightShadowData;
};

///
///			DirectionalLightParameters
///
struct DirectionalLightParameters
{
    half3 color;
	half3 lightPosition;
	half3 lightDirection;
	MatrixFloats worldToLightMatrix;
	MatrixFloats lightToWorldMatrix;
	int shadowmapIndex;
	int cookieMapIndex;
	half2 cookieParameters;
	int enableOutOfPhaseColor;
    half3 outOfPhaseColor;
};

///
///			SpotLightParameters
///
struct SpotLightParameters
{
    half3 color;
	half3 lightPosition;
	half3 lightDirection;
	half lightRange;
	half lightCosHalfAngle;
	half2 angularFalloffParameters;
	half2 distanceFalloffParameters;
	MatrixFloats worldToShadowMatrix;
	int shadowMapIndex;
	half shadowStrength;
	int cookieMapIndex;
	half3 cookieParameters;
};

///
///			PointLightParameters
///
struct PointLightParameters
{
    half3 color;
    half3 lightPosition;
    half lightRange;
    half2 distanceFalloffParameters;
    MatrixFloats worldToShadowMatrix;
    int shadowMapIndex;
    half shadowStrength;
	int cookieMapIndex;
	half3 cookieParameters;
};
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

StructuredBuffer<VolumeData> volumeDataBuffer;
uint volumeCount;
Texture3D<half4> volumeMaskTexture;

half GetShapeGradient(VolumeData volumeData, inout half3 position)
{
    half gradient = 1;

    [branch]
	if (volumeData.shape == 1)
	{
		position = TransformPoint(position, volumeData.transform.ToMatrix());
		gradient = ClampedInverseLerp(volumeData.yPositiveFade, 0, position.y);
	}
	else if (volumeData.shape == 2)
	{
		position = TransformPoint(position, volumeData.transform.ToMatrix());
		half x = ClampedInverseLerp(-0.5f, -0.5f + volumeData.xNegativeFade, position.x) - ClampedInverseLerp(0.5f - volumeData.xPositiveFade, 0.5f, position.x);
		half y = ClampedInverseLerp(-0.5f, -0.5f + volumeData.yNegativeFade, position.y) - ClampedInverseLerp(0.5f - volumeData.yPositiveFade, 0.5f, position.y);
		half z = ClampedInverseLerp(-0.5f, -0.5f + volumeData.zNegativeFade, position.z) - ClampedInverseLerp(0.5f - volumeData.zPositiveFade, 0.5f, position.z);
		gradient = saturate(lerp(0, lerp(0, x, y), z));
	}
	else if (volumeData.shape == 3)
	{
		position = TransformPoint(position, volumeData.transform.ToMatrix());
		gradient = ClampedInverseLerp(0.5f, 0.5f - volumeData.xPositiveFade * 0.5f, length(position));
	}
	else if (volumeData.shape == 4)
	{
		position = TransformPoint(position, volumeData.transform.ToMatrix());
		half y = ClampedInverseLerp(-0.5f, -0.5f + volumeData.yNegativeFade, position.y) - ClampedInverseLerp(0.5f - volumeData.yPositiveFade, 0.5f, position.y);
		half xz = ClampedInverseLerp(0.5f, 0.5f - volumeData.xPositiveFade * 0.5f, length(position.xz));
		gradient = lerp(0, xz, y);
	}
	else if (volumeData.shape == 5)
	{
		position = TransformPoint(position, volumeData.transform.ToMatrix());
		half z = ClampedInverseLerp(1, 1.0f - volumeData.zPositiveFade * 2, position.z);
		half xy = ClampedInverseLerp(0.5f, 0.5f - volumeData.xPositiveFade * 0.5f, length(position.xy / saturate(position.z)));
		gradient = lerp(0, xy, z);
	}

	return gradient;
}

void ComputeVolumeContribution(VolumeData volumeData, half3 worldPosition, inout half density, inout half anisotropy, inout half3 color)
{
	half gradient = GetShapeGradient(volumeData, worldPosition);

    [branch]
	if (gradient > 0)
	{		
        half densityMask = 1.0f;
        half anisotropyMask = 1.0f;
        half3 colorMask = half3(1,1,1);

        #if ENABLE_VOLUMES_TEXTURE_MASK
        [branch]
        if(volumeData.textureData.index > -1)
        {
	        uint width;
	        uint height;
	        uint depth;
	        volumeMaskTexture.GetDimensions(width, height, depth);
	        half3 samplingPosition = GetCombinedTexture3dCoordinates(worldPosition, (half)width, (half)depth, (half)volumeData.textureData.index, volumeData.textureData.transform.ToMatrix(), volumeData.textureData.wrapMode, volumeData.textureData.filterMode);
	        half4 textureMask = volumeMaskTexture.SampleLevel(_LinearClamp, samplingPosition, 0);
        
            [branch]
            if(volumeData.textureData.clipOnAlpha && volumeData.textureData.clippingThreshold > textureMask.w)
            {
                return;
            }
        
            densityMask *= LevelValue(volumeData.densityTextureLevelsParameters, textureMask.w);
            anisotropyMask *= LevelValue(volumeData.anisotropyTextureLevelsParameters, textureMask.w);
            colorMask *= LevelValue(volumeData.colorTextureLevelsParameters, textureMask.xyz);
        }
        #endif
        
        #if ENABLE_VOLUMES_NOISE_MASK
        [branch]
        if(volumeData.noiseData.enable)
        {
	        half3 noisePosition = TransformPoint(worldPosition, volumeData.noiseData.transform.ToMatrix());
            half noiseMask = snoise(half4(noisePosition, (time + volumeData.noiseData.offset) * volumeData.noiseData.speed)) * 0.5f + 0.5f;

			densityMask *= LevelValue(volumeData.densityNoiseLevelsParameters, noiseMask);
			anisotropyMask *= LevelValue(volumeData.anisotropyNoiseLevelsParameters, noiseMask);
			colorMask *= LevelValue(volumeData.colorNoiseLevelsParameters, noiseMask);
        }
        #endif
        
		gradient = pow(abs(gradient), volumeData.falloffExponent);
    
        [branch]
	    if (volumeData.injectDensity)
	    {
		    density += volumeData.densityValue * gradient * densityMask;
	    }
    
        [branch]
        if (volumeData.injectAnisotropy)
        {
		    anisotropy += volumeData.anisotropyValue * gradient * anisotropyMask;
        }
    
        [branch]
	    if (volumeData.injectColor == 1)
	    {
	        color += volumeData.colorValue * gradient * colorMask;
        }
	}
}

void ComputeVolumesInjection(half3 worldPosition, half3 viewVector, inout half4 accumulationColor, inout half density, inout half anisotropy)
{
    [allow_uav_condition]
	for (uint i = 0; i < volumeCount; ++i)
	{
		ComputeVolumeContribution(volumeDataBuffer[i], worldPosition, density, anisotropy, accumulationColor.xyz);
	}

	density = max(0, density);
	anisotropy = saturate(anisotropy);
}
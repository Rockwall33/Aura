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

using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Ordered struct of volume parameters to be sent to the compute shader
    /// </summary>
    public struct VolumeData
    {
        #region Public Members
        /// <summary>
        /// Transform of the volume
        /// </summary>
        public MatrixFloats transform;
        /// <summary>
        /// Id of the shape of the volume
        /// </summary>
        public int shape;
        /// <summary>
        /// Exponent the fading gradient will be raised to
        /// </summary>
        public float falloffExponent;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local X axis
        /// </summary>
        public float xPositiveFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local X axis
        /// </summary>
        public float xNegativeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Y axis
        /// </summary>
        public float yPositiveFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Y axis
        /// </summary>
        public float yNegativeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Z axis
        /// </summary>
        public float zPositiveFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Z axis
        /// </summary>
        public float zNegativeFade;
        /// <summary>
        /// Texture parameters
        /// </summary>
        public VolumeTextureData textureData;
        /// <summary>
        /// Noise parameters
        /// </summary>
        public VolumeDynamicNoiseData noiseData;
        /// <summary>
        /// Enables density injection
        /// </summary>
        public int injectDensity;
        /// <summary>
        /// Density injection strength
        /// </summary>
        public float densityValue;
        /// <summary>
        /// Density texture mask levels parameters
        /// </summary>
        public VolumeLevelsData densityTextureLevelsParameters;
        /// <summary>
        /// Density noise mask levels parameters
        /// </summary>
        public VolumeLevelsData densityNoiseLevelsParameters;
        /// <summary>
        /// Enables anisotropy injection
        /// </summary>
        public int injectAnisotropy;
        /// <summary>
        /// Anisotropy injection strength
        /// </summary>
        public float anisotropyValue;
        /// <summary>
        /// Anisotropy texture mask levels parameters
        /// </summary>
        public VolumeLevelsData anisotropyTextureLevelsParameters;
        /// <summary>
        /// Anisotropy noise mask levels parameters
        /// </summary>
        public VolumeLevelsData anisotropyNoiseLevelsParameters;
        /// <summary>
        /// Enables color injection
        /// </summary>
        public int injectColor;
        /// <summary>
        /// Color value * injection strength
        /// </summary>
        public Vector3 colorValue;
        /// <summary>
        /// Color texture mask levels parameters
        /// </summary>
        public VolumeLevelsData colorTextureLevelsParameters;
        /// <summary>
        /// Color noise mask levels parameters
        /// </summary>
        public VolumeLevelsData colorNoiseLevelsParameters;
        #endregion

        #region Functions
        /// <summary>
        /// Returns the bytes size of the struct
        /// </summary>
        public static int Size
        {
            get
            {
                int byteSize = 0;
                byteSize += MatrixFloats.Size; // transform
                byteSize += sizeof(int); // type
                byteSize += sizeof(float); // falloffExponent
                byteSize += sizeof(float); // xPositiveFade
                byteSize += sizeof(float); // xNegativeFade
                byteSize += sizeof(float); // yPositiveFade
                byteSize += sizeof(float); // yNegativeFade
                byteSize += sizeof(float); // zPositiveFade
                byteSize += sizeof(float); // zNegativeFade
                byteSize += VolumeTextureData.Size; // textureData
                byteSize += VolumeDynamicNoiseData.Size; // noiseData
                byteSize += sizeof(int); // injectDensity
                byteSize += sizeof(float); // densityValue
                byteSize += VolumeLevelsData.Size; // densityTextureLevelsParameters
                byteSize += VolumeLevelsData.Size; // densityNoiseLevelsParameters
                byteSize += sizeof(int); // injectAnisotropy
                byteSize += sizeof(float); // anisotropyValue
                byteSize += VolumeLevelsData.Size; // anisotropyTextureLevelsParameters
                byteSize += VolumeLevelsData.Size; // anisotropyNoiseLevelsParameters
                byteSize += sizeof(int); // injectColor
                byteSize += sizeof(float) * 3; // colorValue
                byteSize += VolumeLevelsData.Size; // colorTextureLevelsParameters
                byteSize += VolumeLevelsData.Size; // colorNoiseLevelsParameters

                return byteSize;
            }
        }
        #endregion
    }
}

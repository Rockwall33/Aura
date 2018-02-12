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

namespace AuraAPI
{
    /// <summary>
    /// Ordered struct of texture parameters to be sent to the compute shader
    /// </summary>
    public struct VolumeTextureData
    {
        #region Public Members
        /// <summary>
        /// The tranform of the texture
        /// </summary>
        public MatrixFloats transform;
        /// <summary>
        /// The index of the texture in the composed volumetric texture. The "enable" parameter is included in this as it is set to -1 if enable == false
        /// </summary>
        public int index;
        /// <summary>
        /// Defines if the texture should loop or if the last pixel should be repeated
        /// </summary>
        public int wrapMode;
        /// <summary>
        /// Defines the texture sampling filter as bilinear or point
        /// </summary>
        public int filterMode;
        /// <summary>
        /// Allows to disable the computation of the volume's cell if the alpha value of the texture is under a defined threshold
        /// </summary>
        public int clipOnAlpha;
        /// <summary>
        /// Threshold used for computation clipping
        /// </summary>
        public float clippingThreshold;
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
                byteSize += sizeof(int); // index
                byteSize += sizeof(int); // wrapMode
                byteSize += sizeof(int); // filterMode
                byteSize += sizeof(int); // clipOnAlpha
                byteSize += sizeof(float); // clippingThreshold

                return byteSize;
            }
        }
        #endregion
    }
}

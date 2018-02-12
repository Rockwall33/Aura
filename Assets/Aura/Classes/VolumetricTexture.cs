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

using System;
using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Collection of parameters defining a texture to be used in a volume
    /// </summary>
    [Serializable]
    public class VolumetricTexture
    {
        #region Public Members
        /// <summary>
        /// Allows to disable the computation of the volume's cell if the alpha value of the texture is under a defined threshold
        /// </summary>
        public bool clipComputationBasedOnAlpha;
        /// <summary>
        /// Threshold used for computation clipping
        /// </summary>
        [Range(0, 1)]
        public float clippingThreshold = 0.5f;
        /// <summary>
        /// Enables the volume texture
        /// </summary>
        public bool enable;
        /// <summary>
        /// Defines the texture sampling filter as bilinear or point
        /// </summary>
        public VolumetricTextureFilterModeEnum filterMode = VolumetricTextureFilterModeEnum.SameAsSource;
        /// <summary>
        /// The source Texture3D
        /// </summary>
        [Texture3DPreview]
        public Texture3D texture;
        /// <summary>
        /// Index of the texture inside the composed volumetric texture inside the volumes manager
        /// </summary>
        public int textureIndex = -1;
        /// <summary>
        /// Allows to set base position, rotation and scale and animate them
        /// </summary>
        public TransformParameters transform;
        /// <summary>
        /// Defines if the texture should loop or if the last pixel should be repeated
        /// </summary>
        public VolumetricTextureWrapModeEnum wrapMode = VolumetricTextureWrapModeEnum.SameAsSource;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public VolumetricTexture()
        {
            transform.space = Space.Self;
            transform.position = Vector3.zero;
            transform.rotation = Vector3.zero;
            transform.scale = Vector3.one;
            transform.animatePosition = false;
            transform.positionSpeed = Vector3.zero;
            transform.animateRotation = false;
            transform.rotationSpeed = Vector3.zero;

            textureIndex = -1;
        }
        #endregion
    }
}

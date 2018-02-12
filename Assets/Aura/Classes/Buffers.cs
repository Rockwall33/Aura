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
using UnityEngine.Rendering;

namespace AuraAPI
{
    /// <summary>
    /// Collection of texture buffers which contain the computed volumetric data
    /// </summary>
    public class Buffers
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Buffers(Vector3Int resolution)
        {
            Resolution = resolution;
            CreateBuffers();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the resolution of the buffers
        /// </summary>
        public Vector3Int Resolution
        {
            get;
            private set;
        }

        /// <summary>
        /// Accessor to the volumetric data buffer (containing the lighting (RGB) and the density (A))
        /// </summary>
        public SwappableRenderTexture LightingVolumeTextures
        {
            get;
            private set;
        }

        /// <summary>
        /// Accessor to the volumetric accumulated buffer (containing the accumulated lighting)
        /// </summary>
        public RenderTexture FogVolumeTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// Accessor to the buffer containing the maximum depth
        /// </summary>
        public SwappableRenderTexture OcclusionTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates the needed texture buffers
        /// </summary>
        private void CreateBuffers()
        {
            LightingVolumeTextures = new SwappableRenderTexture(Resolution.x, Resolution.y, Resolution.z, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, TextureWrapMode.Clamp, FilterMode.Bilinear);

            FogVolumeTexture = CreateTexture(RenderTextureFormat.ARGBHalf, TextureDimension.Tex3D, TextureWrapMode.Clamp, FilterMode.Bilinear);
            Shader.SetGlobalTexture("Aura_VolumetricLightingTexture", FogVolumeTexture);

            OcclusionTexture = new SwappableRenderTexture(Resolution.x, Resolution.y, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear, TextureWrapMode.Clamp, FilterMode.Point);
        }

        /// <summary>
        /// Generic function to create a texture buffer
        /// </summary>
        /// <param name="format">The desired format</param>
        /// <param name="dimensions">The desired dimensions</param>
        /// <param name="wrapMode">The desired wrap mode</param>
        /// <param name="filterMode">The desired filter mode</param>
        /// <returns>The texture buffer</returns>
        private RenderTexture CreateTexture(RenderTextureFormat format, TextureDimension dimensions, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            RenderTexture texture = new RenderTexture(Resolution.x, Resolution.y, 0, format, RenderTextureReadWrite.Linear);
            texture.dimension = dimensions;
            if(dimensions == TextureDimension.Tex3D)
            {
                texture.volumeDepth = Resolution.z;
            }
            texture.wrapMode = wrapMode;
            texture.filterMode = filterMode;
            texture.enableRandomWrite = true;
            texture.Create();

            return texture;
        }

        /// <summary>
        /// Clears the content of the volumetric data buffer (containing the lighting (RGB) and the density (A)) to 0 (Black)
        /// </summary>
        public void ClearVolumetricFogTexture()
        {
            FogVolumeTexture.Clear(Color.black);
        }

        /// <summary>
        /// Releases the different buffers
        /// </summary>
        public void ReleaseBuffers()
        {
            LightingVolumeTextures.Release();
            FogVolumeTexture.Release();
            OcclusionTexture.Release();
        }
        #endregion
    }
}

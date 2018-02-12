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
    /// Double buffering render texture
    /// </summary>
    public class SwappableRenderTexture
    {
        #region Private Members
        /// <summary>
        /// The two swapped render textures
        /// </summary>
        private readonly RenderTexture[] _buffers = new RenderTexture[2];
        /// <summary>
        /// The ID of the read texture
        /// </summary>
        private int _readId = 0;
        /// <summary>
        /// The ID of the write texture
        /// </summary>
        private int _writeId = 1;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for a 2D swappable RenderTexture
        /// </summary>
        /// <param name="width">The width of the textures</param>
        /// <param name="height">The height of the textures</param>
        /// <param name="format">The format of the textures</param>
        /// <param name="sRgbSampling">The color space of the textures</param>
        /// <param name="wrapMode">The wrap mode of the textures</param>
        /// <param name="filterMode">The filter mode of the textures</param>
        public SwappableRenderTexture(int width, int height, RenderTextureFormat format, RenderTextureReadWrite sRgbSampling, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            CreateRenderTexture(0, width, height, -1, format, sRgbSampling, wrapMode, filterMode);
            CreateRenderTexture(1, width, height, -1, format, sRgbSampling, wrapMode, filterMode);
        }

        /// <summary>
        /// Constructor for a 3D swappable RenderTexture
        /// </summary>
        /// <param name="width">The width of the textures</param>
        /// <param name="height">The height of the textures</param>
        /// <param name="depth">The depth of the textures</param>
        /// <param name="format">The format of the textures</param>
        /// <param name="sRgbSampling">The color space of the textures</param>
        /// <param name="wrapMode">The wrap mode of the textures</param>
        /// <param name="filterMode">The filter mode of the textures</param>
        public SwappableRenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite sRgbSampling, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            CreateRenderTexture(0, width, height, depth, format, sRgbSampling, wrapMode, filterMode);
            CreateRenderTexture(1, width, height, depth, format, sRgbSampling, wrapMode, filterMode);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the read texture
        /// </summary>
        public RenderTexture ReadBuffer
        {
            get
            {
                return _buffers[_readId];
            }
        }

        /// <summary>
        /// Accessor to the write texture
        /// </summary>
        public RenderTexture WriteBuffer
        {
            get
            {
                return _buffers[_writeId];
            }
        }

        /// <summary>
        /// Creates a formated RenderTexture and assign it to an ID
        /// </summary>
        /// <param name="index">The target ID of the texture</param>
        /// <param name="width">The width of the texture</param>
        /// <param name="height">The height of the texture</param>
        /// <param name="depth">The depth of the texture</param>
        /// <param name="format">The format of the texture</param>
        /// <param name="sRgbSampling">The color space of the texture</param>
        /// <param name="wrapMode">The wrap mode of the texture</param>
        /// <param name="filterMode">The filter mode of the texture</param>
        private void CreateRenderTexture(int index, int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite sRgbSampling, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            _buffers[index] = new RenderTexture(width, height, 0, format, sRgbSampling);
            if(depth > 0)
            {
                _buffers[index].dimension = TextureDimension.Tex3D;
                _buffers[index].volumeDepth = depth;
            }
            _buffers[index].wrapMode = wrapMode;
            _buffers[index].filterMode = filterMode;
            _buffers[index].enableRandomWrite = true;
            _buffers[index].Create();
        }

        /// <summary>
        /// Swaps the textures
        /// </summary>
        public void Swap()
        {
            int tmp = _readId;
            _readId = _writeId;
            _writeId = tmp;
        }

        /// <summary>
        /// Releases the textures
        /// </summary>
        public void Release()
        {
            _buffers[0].Release();
            _buffers[1].Release();
        }
        #endregion
    }
}

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
using System.Collections.Generic;
using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Compose a Texture3D out of several Texture3Ds
    /// </summary>
    internal class VolumetricTextureArrayComposer
    {
        #region Private Members
        /// <summary>
        /// The list of candidate Textures
        /// </summary>
        private readonly List<Texture3D> _texturesList;
        /// <summary>
        /// The format of the generated Texture3D
        /// </summary>
        private readonly TextureFormat _requiredTextureFormat;
        /// <summary>
        /// The cubic size of the generated Texture3D
        /// </summary>
        private readonly int _requiredSize;
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the composed texture has been generated
        /// </summary>
        public event Action OnTextureUpdated;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requiredTextureFormat">The format of the composed Texture3D</param>
        /// <param name="requiredSize">The size in pixels of the width and the height of the composed Texture3D</param>
        public VolumetricTextureArrayComposer(TextureFormat requiredTextureFormat, int requiredSize)
        {
            _texturesList = new List<Texture3D>();
            _requiredTextureFormat = requiredTextureFormat;
            _requiredSize = requiredSize;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the generated Texture3D
        /// </summary>
        public Texture3D VolumeTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells if a Texture3D has been generated
        /// </summary>
        public bool HasVolumeTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells if changes were made and Generate() should be called
        /// </summary>
        public bool NeedsToUpdateVolumeTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// Raises the onTextureUpdated event
        /// </summary>
        private void RaiseTextureUpdatedEvent()
        {
            if(OnTextureUpdated != null)
            {
                OnTextureUpdated();
            }
        }

        /// <summary>
        /// Clears the candidate textures list
        /// </summary>
        public void ClearTextureList()
        {
            _texturesList.Clear();
            NeedsToUpdateVolumeTexture = true;
        }

        /// <summary>
        /// Adds a new candidate texture to the textures list
        /// </summary>
        /// <param name="texture">The Texture3D to be added</param>
        public void AddTexture(Texture3D texture)
        {
            if(texture != null)
            {
                if(texture.height != _requiredSize || texture.width != _requiredSize || texture.depth != _requiredSize)
                {
                    Debug.LogError("Pixel sizes of Texture3D \"" + texture + "\" does not match the required size of " + _requiredSize + "pixels for every dimensions.", texture);
                    return;
                }

                if(texture.format != _requiredTextureFormat)
                {
                    Debug.LogError("Texture format of Texture3D \"" + texture + "\" does not match the required " + _requiredTextureFormat + " format.", texture);
                    return;
                }

                if(!_texturesList.Contains(texture))
                {
                    _texturesList.Add(texture);
                    NeedsToUpdateVolumeTexture = true;
                }
            }
        }

        /// <summary>
        /// Removes a texture from the candidate textures list
        /// </summary>
        /// <param name="texture">The Texture3D to be removed</param>
        public void RemoveTexture(Texture3D texture)
        {
            if(_texturesList.Contains(texture))
            {
                _texturesList.Remove(texture);
                NeedsToUpdateVolumeTexture = true;
            }
        }

        /// <summary>
        /// Generates the Texture3D composed out of the candidate textures (already handles NeedsToUpdateVolumeTexture parameter check)
        /// </summary>
        public void GenerateComposedTexture3D()
        {
            if(NeedsToUpdateVolumeTexture)
            {
                if(_texturesList.Count > 0)
                {
                    Color[] colorArray = new Color[0];
                    VolumeTexture = new Texture3D(_requiredSize, _requiredSize, _requiredSize * _texturesList.Count, _requiredTextureFormat, false);

                    for(int i = 0; i < _texturesList.Count; ++i)
                    {
                        // TODO : DO WITH GRAPHICS.COPYTEXTURES NOW THAT TEXTURE3D COPY ACTUALLY WORKS
                        colorArray = colorArray.Append(_texturesList[i].GetPixels());
                    }

                    VolumeTexture.SetPixels(colorArray);
                    VolumeTexture.Apply();

                    HasVolumeTexture = true;
                }
                else
                {
                    VolumeTexture = null;
                    HasVolumeTexture = false;
                }

                NeedsToUpdateVolumeTexture = false;

                RaiseTextureUpdatedEvent();
            }
        }

        /// <summary>
        /// Returns the index of the queried texture in the candidate textures list. This index is the same as the corresponding Texture3D inside the composed Texture3D.
        /// </summary>
        /// <param name="texture">The queried texture.</param>
        /// <returns>The index of the texture. -1 if not found.</returns>
        public int GetTextureIndex(Texture3D texture)
        {
            return _texturesList.IndexOf(texture);
        }
        #endregion
    }
}

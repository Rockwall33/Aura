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

using System.Collections.Generic;
using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Manages the volumes, collects and packs data and volumetric textures
    /// </summary>
    public class VolumesManager
    {
        #region Public Members
        /// <summary>
        /// The size of the volumetric textures
        /// </summary>
        public static readonly int volumetricTexturesSize = 16; // TODO : EXPOSE AS DYNAMIC PARAMETER
        #endregion

        #region Private Members
        /// <summary>
        /// The list of registred volumes
        /// </summary>
        private readonly List<AuraVolume> _registredVolumes;
        /// <summary>
        /// The ObjectsCuller in charge of selecting which volumes are visible
        /// </summary>
        private readonly ObjectsCuller<AuraVolume> _culler;
        /// <summary>
        /// In charge of composing a volume texture out of the volume texture masks of the volumes
        /// </summary>
        private readonly VolumetricTextureArrayComposer _volumeTextureComposer;
        /// <summary>
        /// Array of visible volumes
        /// </summary>
        private AuraVolume[] _visibleVolumes;
        /// <summary>
        /// Array of data of the visible volumes
        /// </summary>
        private VolumeData[] _visibleVolumesDataArray;
        /// <summary>
        /// Is the manager initialised and not disposed
        /// </summary>
        private bool _isEnabled;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="volumetricTexturesSize">The desired size of the composed Texture3D (made of the volumes' texture masks)</param>
        public VolumesManager()
        {
            _registredVolumes = new List<AuraVolume>();
            if(_culler == null)
            {
                _culler = new ObjectsCuller<AuraVolume>();
            }
            _volumeTextureComposer = new VolumetricTextureArrayComposer(TextureFormat.RGBA32, volumetricTexturesSize);
            _volumeTextureComposer.OnTextureUpdated += VolumeTextureComposer_onTextureUpdated;

            Aura.OnPreRenderEvent += Aura_onPreRenderEvent;

            _isEnabled = true;
        }
        #endregion

        #region Functions
        /// <summary>
        /// The compute buffer in charge of passing the volumes' data to the compute shaders
        /// </summary>
        public ComputeBuffer Buffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Has one volume or more a texture mask?
        /// </summary>
        public bool HasVolumeTexture
        {
            get
            {
                return _volumeTextureComposer.HasVolumeTexture;
            }
        }

        /// <summary>
        /// Retrieves the composed volume texture out of the volume texture masks of the volumes
        /// </summary>
        public Texture3D VolumeTexture
        {
            get
            {
                return _volumeTextureComposer.VolumeTexture;
            }
        }

        /// <summary>
        /// Is one or more volume(s) inside the ranged frustum?
        /// </summary>
        public bool HasVisibleVolumes
        {
            get
            {
                return _culler != null && _culler.HasVisibleObjects;
            }
        }

        /// <summary>
        /// Updates the manager
        /// </summary>
        private void Aura_onPreRenderEvent(Camera cameraComponent)
        {
            if(_isEnabled)
            {
                _culler.Update();

                SetupComputeBuffer();

                CollectVolumesData();
            }
        }

        /// <summary>
        /// Releases unmanaged objects and unregisters events
        /// </summary>
        public void Dispose()
        {
            if(_isEnabled)
            {
                _culler.Dispose();
                DisposeComputeBuffer();

                Aura.OnPreRenderEvent -= Aura_onPreRenderEvent;

                _isEnabled = false;
            }
        }

        /// <summary>
        /// Allocate new compute buffer or null, according to visible objects count from culler
        /// </summary>
        private void SetupComputeBuffer()
        {
            if(Buffer == null || _culler.VisibleObjectsCount != Buffer.count)
            {
                DisposeComputeBuffer();

                if(_culler.HasVisibleObjects)
                {
                    Buffer = new ComputeBuffer(_culler.VisibleObjectsCount, VolumeData.Size);
                    _visibleVolumesDataArray = new VolumeData[_culler.VisibleObjectsCount];
                }
                else
                {
                    Buffer = null;
                }
            }
        }

        /// <summary>
        /// Collects the volumes's data and pack them in the computeBuffer
        /// </summary>
        private void CollectVolumesData()
        {
            if(_culler.HasVisibleObjects)
            {
                AuraVolume[] visibleVolumes = _culler.GetVisibleObjects();
                for(int i = 0; i < _culler.VisibleObjectsCount; ++i)
                {
                    _visibleVolumesDataArray[i] = visibleVolumes[i].GetData();
                }

                Buffer.SetData(_visibleVolumesDataArray);
            }
        }

        /// <summary>
        /// Releases the computeBuffer
        /// </summary>
        private void DisposeComputeBuffer()
        {
            if(Buffer != null)
            {
                Buffer.Release();
            }
        }

        /// <summary>
        /// Registers a new volume to the manager
        /// </summary>
        /// <param name="volume">The volume to add</param>
        public void Register(AuraVolume volume)
        {
            _culler.Register(volume);

            if(!_registredVolumes.Contains(volume))
            {
                _registredVolumes.Add(volume);

                volume.OnTextureMaskStateChanged += Volume_onTextureMaskStateChanged;
                volume.OnTextureMaskChanged += Volume_onTextureMaskChanged;
            }
        }

        /// <summary>
        /// Unregisters a volume from the manager
        /// </summary>
        /// <param name="volume">The volume to remove</param>
        public void Unregister(AuraVolume volume)
        {
            _culler.Unregister(volume);

            if(_registredVolumes.Contains(volume))
            {
                _registredVolumes.Remove(volume);

                volume.OnTextureMaskStateChanged -= Volume_onTextureMaskStateChanged;
                volume.OnTextureMaskChanged -= Volume_onTextureMaskChanged;
            }
        }

        /// <summary>
        /// Called when the volume texture composer has generated a new texture
        /// </summary>
        private void VolumeTextureComposer_onTextureUpdated()
        {
            foreach(AuraVolume volume in _registredVolumes)
            {
                if(volume.textureMask.enable && volume.textureMask.texture != null)
                {
                    volume.textureMask.textureIndex = _volumeTextureComposer.GetTextureIndex(volume.textureMask.texture);
                }
                else
                {
                    volume.textureMask.textureIndex = -1;
                }
            }
        }

        /// <summary>
        /// Called when a volume has changes on its texture mask state
        /// </summary>
        /// <param name="volume">The volume in which the texture mask state has changed</param>
        private void Volume_onTextureMaskStateChanged(AuraVolume volume)
        {
            ResetCandidateTexturesAndGenerateVolumeTexture();
        }

        /// <summary>
        /// Called when a volume has changes on its texture mask texture
        /// </summary>
        /// <param name="volume">The volume in which the texture mask texture has changed</param>
        private void Volume_onTextureMaskChanged(AuraVolume volume)
        {
            ResetCandidateTexturesAndGenerateVolumeTexture();
        }

        /// <summary>
        /// Asks the volume texture composer to generate a new composed texture
        /// </summary>
        private void GenerateVolumeTexture()
        {
            _volumeTextureComposer.GenerateComposedTexture3D();
        }

        /// <summary>
        /// Clears the candidate textures list from the volume texture composer, refills the list by iterating on volumes and calls the composer to generate a new composed volume texture
        /// </summary>
        private void ResetCandidateTexturesAndGenerateVolumeTexture()
        {
            _volumeTextureComposer.ClearTextureList();
            foreach(AuraVolume volume in _registredVolumes)
            {
                if(volume.textureMask.enable && volume.textureMask.texture != null)
                {
                    _volumeTextureComposer.AddTexture(volume.textureMask.texture);
                }
            }

            GenerateVolumeTexture();
        }
        #endregion
    }
}

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
    /// Manager that handles point AuraLights
    /// </summary>
    public class PointLightsManager
    {
        #region Public Members
        /// <summary>
        /// The shadow map size
        /// </summary>
        public static readonly Vector2Int shadowMapSize = new Vector2Int(512, 256); // TODO : EXPOSE AS DYNAMIC PARAMETER
        /// <summary>
        /// The cookie map size
        /// </summary>
        public static readonly Vector2Int cookieMapSize = new Vector2Int(512, 256); // TODO : EXPOSE AS DYNAMIC PARAMETER
        #endregion

        #region Private Members
        /// <summary>
        /// The culler that will tell which candidate light is visible from the camera
        /// </summary>
        private readonly ObjectsCuller<AuraLight> _culler;
        /// <summary>
        /// The candidate lights
        /// </summary>
        private readonly List<AuraLight> _lights = new List<AuraLight>();
        /// <summary>
        /// The composer that will collect the shadow maps and stack them in a Texture2DArray
        /// </summary>
        private readonly ShadowmapsCollector _shadowmapsCollector;
        /// <summary>
        /// The composer that will collect the cookie maps and stack them in a Texture2DArray
        /// </summary>
        private readonly Texture2DArrayComposer _cookieMapsCollector;
        /// <summary>
        /// The collected packed data of the visible lights
        /// </summary>
        private PointLightParameters[] _visiblePointLightParametersArray;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointLightsManager()
        {
            _shadowmapsCollector = new ShadowmapsCollector(PointLightsManager.shadowMapSize.x, PointLightsManager.shadowMapSize.y);
            _cookieMapsCollector = new Texture2DArrayComposer(PointLightsManager.cookieMapSize.x, PointLightsManager.cookieMapSize.y, TextureFormat.R8, true);
            _cookieMapsCollector.alwaysGenerateOnUpdate = true;
            _culler = new ObjectsCuller<AuraLight>();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Tells if there are visible/unculled lights
        /// </summary>
        public bool HasVisibleLights
        {
            get
            {
                return _culler != null && _culler.HasVisibleObjects;
            }
        }

        /// <summary>
        /// Tells if there are any shadow caster
        /// </summary>
        public bool HasShadowCasters
        {
            get
            {
                return _shadowmapsCollector.HasTexture;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked shadow maps
        /// </summary>
        public Texture2DArray ShadowMapsArray
        {
            get
            {
                return _shadowmapsCollector.Texture;
            }
        }

        /// <summary>
        /// Tells if there are any cookie caster
        /// </summary>
        public bool HasCookieCasters
        {
            get
            {
                return _cookieMapsCollector.HasTexture;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked cookie maps
        /// </summary>
        public Texture2DArray CookieMapsArray
        {
            get
            {
                return _cookieMapsCollector.Texture;
            }
        }

        /// <summary>
        /// Accessor to the compute buffer containing the packed data of the visible lights
        /// </summary>
        public ComputeBuffer DataBuffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Disposes the members
        /// </summary>
        public void Dispose()
        {
            ReleaseBuffer();
            _culler.Dispose();
        }

        /// <summary>
        /// Releases the compute buffer containing the packed data of the visible lights
        /// </summary>
        private void ReleaseBuffer()
        {
            if(DataBuffer != null)
            {
                DataBuffer.Release();
            }
        }

        /// <summary>
        /// Resizes the buffers containing the packed data of the visible lights
        /// </summary>
        private void SetupBuffers()
        {
            if(DataBuffer == null || _culler.VisibleObjectsCount != DataBuffer.count)
            {
                ReleaseBuffer();

                if(HasVisibleLights)
                {
                    DataBuffer = new ComputeBuffer(_culler.VisibleObjectsCount, PointLightParameters.Size);
                    _visiblePointLightParametersArray = new PointLightParameters[_culler.VisibleObjectsCount];
                }
                else
                {
                    DataBuffer = null;
                    _visiblePointLightParametersArray = null;
                }
            }
        }

        /// <summary>
        /// Register an AuraLights to be taken into account
        /// </summary>
        /// <param name="light">The candidate light</param>
        /// <returns>True if registration went well, false otherwise</returns>
        public bool Register(AuraLight light, bool castShadows, bool castCookie)
        {
            if(light.Type == LightType.Point && !_lights.Contains(light))
            {
                _culler.Register(light);

                _lights.Add(light);

                if(castShadows)
                {
                    _shadowmapsCollector.AddTexture(light.shadowMapRenderTexture);
                }

                if(castCookie)
                {
                    _cookieMapsCollector.AddTexture(light.cookieMapRenderTexture);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters an AuraLights
        /// </summary>
        /// <param name="light">The candidate light</param>
        /// <returns>True if unregistration went well, false otherwise</returns>
        public bool Unregister(AuraLight light) // TODO : HANDLE SHADOW/COOKIE UNREGISTRATION AUTOMATICALLY
        {
            if(_lights.Contains(light))
            {
                if(light.cookieMapRenderTexture != null)
                {
                    _cookieMapsCollector.RemoveTexture(light.cookieMapRenderTexture);
                }

                if(light.shadowMapRenderTexture != null)
                {
                    _shadowmapsCollector.RemoveTexture(light.shadowMapRenderTexture);
                }

                _culler.Unregister(light);

                _lights.Remove(light);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the manager (collects shadows/cookies/data and packs them)
        /// </summary>
        public void Update()
        {
            _culler.Update();

            SetupBuffers();

            if(_culler.HasVisibleObjects)
            {
                _shadowmapsCollector.Generate();
                _cookieMapsCollector.Generate();

                AuraLight[] visibleLights = _culler.GetVisibleObjects();
                for(int i = 0; i < _culler.VisibleObjectsCount; ++i)
                {
                    visibleLights[i].SetShadowMapIndex(_shadowmapsCollector.GetTextureIndex(visibleLights[i].shadowMapRenderTexture));
                    visibleLights[i].SetCookieMapIndex(_cookieMapsCollector.GetTextureIndex(visibleLights[i].cookieMapRenderTexture));
                    _visiblePointLightParametersArray[i] = visibleLights[i].GetPointParameters();
                }

                DataBuffer.SetData(_visiblePointLightParametersArray);
            }
        }
        #endregion
    }
}

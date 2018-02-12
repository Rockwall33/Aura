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
    /// Manages the lights, collects and packs data, shadow maps and cookie maps
    /// </summary>
    public class LightsManager
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LightsManager()
        {
            DirectionalLightsManager = new DirectionalLightsManager();
            SpotLightsManager = new SpotLightsManager();
            PointLightsManager = new PointLightsManager();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the directional AuraLights manager
        /// </summary>
        public DirectionalLightsManager DirectionalLightsManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Accessor to the spot AuraLights manager
        /// </summary>
        public SpotLightsManager SpotLightsManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Accessor to the point AuraLights manager
        /// </summary>
        public PointLightsManager PointLightsManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Disposes the different managers
        /// </summary>
        public void Dispose()
        {
            DirectionalLightsManager.Dispose();
            SpotLightsManager.Dispose();
            PointLightsManager.Dispose();
        }

        /// <summary>
        /// Updates the different managers
        /// </summary>
        public void Update()
        {
            DirectionalLightsManager.Update();
            SpotLightsManager.Update();
            PointLightsManager.Update();
        }

        /// <summary>
        /// Registers an AuraLight onto the correct manager
        /// </summary>
        /// <param name="light">The candidate light</param>
        public void Register(AuraLight light, bool castShadows, bool castCookie)
        {
            switch(light.Type)
            {
                case LightType.Directional :
                    {
                        DirectionalLightsManager.Register(light, castShadows, castCookie);
                    }
                    break;

                case LightType.Spot :
                    {
                        SpotLightsManager.Register(light, castShadows, castCookie);
                    }
                    break;

                case LightType.Point :
                    {
                        PointLightsManager.Register(light, castShadows, castCookie);
                    }
                    break;
            }
        }

        /// <summary>
        /// Unregisters an AuraLight from the correct manager
        /// </summary>
        /// <param name="light">The candidate light</param>
        public void Unregister(AuraLight light)
        {
            switch(light.Type)
            {
                case LightType.Directional :
                    {
                        DirectionalLightsManager.Unregister(light);
                    }
                    break;

                case LightType.Spot :
                    {
                        SpotLightsManager.Unregister(light);
                    }
                    break;

                case LightType.Point :
                    {
                        PointLightsManager.Unregister(light);
                    }
                    break;
            }
        }
        #endregion
    }
}

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
    /// Collection of settings defining the computation of the volumetric data
    /// </summary>
    public struct FrustumSettingsParameters
    {
        #region Public Members
        /// <summary>
        /// Enables the computation of volumes
        /// </summary>
        public bool enableVolumes;
        /// <summary>
        /// Enables the computation of volumes' texture mask
        /// </summary>
        public bool enableVolumesTextureMask;
        /// <summary>
        /// Enables the computation of volumes' noise mask
        /// </summary>
        public bool enableVolumesNoiseMask;
        /// <summary>
        /// Enables the computation of directional lights
        /// </summary>
        public bool enableDirectionalLights;
        /// <summary>
        /// Enables the computation of directional lights' shadow
        /// </summary>
        public bool enableDirectionalLightsShadows;
        /// <summary>
        /// Enables the computation of spot lights
        /// </summary>
        public bool enableSpotLights;
        /// <summary>
        /// Enables the computation of spot lights' shadow
        /// </summary>
        public bool enableSpotLightsShadows;
        /// <summary>
        /// Enables the computation of point lights
        /// </summary>
        public bool enablePointLights;
        /// <summary>
        /// Enables the computation of point lights' shadow
        /// </summary>
        public bool enablePointLightsShadows;
        /// <summary>
        /// Enables the computation of lights' cookie
        /// </summary>
        public bool enableLightsCookies;
        /// <summary>
        /// Enables depth occlusion culling
        /// </summary>
        public bool enableOcclusionCulling;
        /// <summary>
        /// The accuracy of the occlusion search
        /// </summary>
        public OcclusionCullingAccuracyEnum occlusionCullingAccuracy;
        /// <summary>
        /// Enables temporal reprojection
        /// </summary>
        public bool enableTemporalReprojection;
        /// <summary>
        /// Amount of reprojection with the previous frame
        /// </summary>
        [Range(0, 1)]
        public float temporalReprojectionFactor;
        /// <summary>
        /// The resolution of the frustum grid
        /// </summary>
        public Vector3Int resolution;
        /// <summary>
        /// The maximum distance where the volumetric data will be computed
        /// </summary>
        public float farClipPlaneDistance;
        #endregion

        #region Functions
        /// <summary>
        /// Initializes the struct to default values
        /// </summary>
        public void Init()
        {
            enableVolumes = true;
            enableVolumesTextureMask = true;
            enableVolumesNoiseMask = true;
            enableDirectionalLights = true;
            enableDirectionalLightsShadows = true;
            enableSpotLights = true;
            enableSpotLightsShadows = true;
            enablePointLights = true;
            enablePointLightsShadows = true;
            enableLightsCookies = true;
            enableOcclusionCulling = true;
            enableTemporalReprojection = true;
            temporalReprojectionFactor = 0.9f;
            resolution = new Vector3Int(160, 90, 128);
            farClipPlaneDistance = 25.0f;
        }
        #endregion
    }
}

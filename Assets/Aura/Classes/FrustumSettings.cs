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
    /// Collection of settings for the data computation
    /// </summary>
    [Serializable]
    public class FrustumSettings
    {
        #region Private members
        /// <summary>
        /// Bitmask collecting the different parameters and giving a unique int for each combinaison
        /// </summary>
        private FrustumParametersEnum _frustumParameters;
        #endregion

        #region Public members
        /// <summary>
        /// Enables the computation of volumes
        /// </summary>
        public bool enableVolumes = true;
        /// <summary>
        /// Enables the computation of volumes' texture mask
        /// </summary>
        public bool enableVolumesTextureMask = true;
        /// <summary>
        /// Enables the computation of volumes' noise mask
        /// </summary>
        public bool enableVolumesNoiseMask = true;
        /// <summary>
        /// Enables the computation of directional lights
        /// </summary>
        public bool enableDirectionalLights = true;
        /// <summary>
        /// Enables the computation of directional lights' shadow
        /// </summary>
        public bool enableDirectionalLightsShadows = true;
        /// <summary>
        /// Enables the computation of spot lights
        /// </summary>
        public bool enableSpotLights = true;
        /// <summary>
        /// Enables the computation of spot lights' shadow
        /// </summary>
        public bool enableSpotLightsShadows = true;
        /// <summary>
        /// Enables the computation of point lights
        /// </summary>
        public bool enablePointLights = true;
        /// <summary>
        /// Enables the computation of point lights' shadow
        /// </summary>
        public bool enablePointLightsShadows = true;
        /// <summary>
        /// Enables the computation of lights' cookie
        /// </summary>
        public bool enableLightsCookies = true;
        /// <summary>
        /// Enables depth occlusion culling
        /// </summary>
        public bool enableOcclusionCulling = true;
        /// <summary>
        /// Accuracy of the occlusion computation (highest means more sampling)
        /// </summary>
        public OcclusionCullingAccuracyEnum occlusionCullingAccuracy = OcclusionCullingAccuracyEnum.Medium;
        /// <summary>
        /// Enables temporal reprojection
        /// </summary>
        public bool enableTemporalReprojection = true;
        /// <summary>
        /// Amount of reprojection with the previous frame
        /// </summary>
        [Range(0, 1)]
        public float temporalReprojectionFactor = 0.9f;
        /// <summary>
        /// The resolution of the volumetric grid
        /// </summary>
        public Vector3Int resolution = new Vector3Int(160, 90, 128);
        /// <summary>
        /// The furthest distance where the data will be computed
        /// </summary>
        public float farClipPlaneDistance = 50.0f;
        /// <summary>
        /// The base density of the volume
        /// </summary>
        public float density = 0.25f;
        /// <summary>
        /// The base anisotropy of the volume
        /// </summary>
        [Range(0, 1)]
        public float anisotropy = 0.5f;
        /// <summary>
        /// The base color of the volume
        /// </summary>
        [ColorCircularPicker]
        public Color color = Color.grey * 0.5f;
        /// <summary>
        /// The base color factor of the volume
        /// </summary>
        public float colorStrength = 0.25f;
        #endregion

        #region Functions
        /// <summary>
        /// Returns a unique int relative to the combinaison of the different parameters
        /// </summary>
        /// <param name="recomputeFlags">Forces the Id recomputation. Default : false</param>
        /// <returns>A unique int relative to the combinaison of the different parameters</returns>
        public int GetId(bool recomputeFlags = false)
        {
            if(recomputeFlags)
            {
                ComputeFlags();
            }

            return (int)_frustumParameters;
        }

        /// <summary>
        /// Recomputes the unique int relative to the combinaison of the different parameters
        /// </summary>
        /// <returns>A unique int relative to the combinaison of the different parameters</returns>
        public int ComputeFlags()
        {
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableOcclusionCulling, enableOcclusionCulling);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableTemporalReprojection, enableTemporalReprojection && Aura.FrameId > 1 && !Mathf.Approximately(temporalReprojectionFactor, 0));

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableVolumes, enableVolumes && Aura.VolumesManager.HasVisibleVolumes);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableVolumesNoiseMask, enableVolumesNoiseMask && _frustumParameters.HasFlags(FrustumParametersEnum.EnableVolumes));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableVolumesTextureMask, enableVolumesTextureMask && _frustumParameters.HasFlags(FrustumParametersEnum.EnableVolumes) && Aura.VolumesManager.HasVolumeTexture);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableDirectionalLights, enableDirectionalLights && Aura.LightsManager.DirectionalLightsManager.HasCandidateLights);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableDirectionalLightsShadows, enableDirectionalLightsShadows && _frustumParameters.HasFlags(FrustumParametersEnum.EnableDirectionalLights) && Aura.LightsManager.DirectionalLightsManager.HasShadowCasters);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.DirectionalLightsShadowsOneCascade, QualitySettings.shadowCascades == 1 && _frustumParameters.HasFlags(FrustumParametersEnum.EnableDirectionalLights));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.DirectionalLightsShadowsTwoCascades, QualitySettings.shadowCascades == 2 && _frustumParameters.HasFlags(FrustumParametersEnum.EnableDirectionalLights));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.DirectionalLightsShadowsFourCascades, QualitySettings.shadowCascades == 4 && _frustumParameters.HasFlags(FrustumParametersEnum.EnableDirectionalLights));

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableSpotLights, enableSpotLights && Aura.LightsManager.SpotLightsManager.HasVisibleLights);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableSpotLightsShadows, enableSpotLightsShadows && _frustumParameters.HasFlags(FrustumParametersEnum.EnableSpotLights) && Aura.LightsManager.SpotLightsManager.HasShadowCasters);
            
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnablePointLights, enablePointLights && Aura.LightsManager.PointLightsManager.HasVisibleLights);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnablePointLightsShadows, enablePointLightsShadows && _frustumParameters.HasFlags(FrustumParametersEnum.EnablePointLights) && Aura.LightsManager.PointLightsManager.HasShadowCasters);
            
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParametersEnum.EnableLightsCookies, enableLightsCookies && (_frustumParameters.HasFlags(FrustumParametersEnum.EnablePointLights) && Aura.LightsManager.PointLightsManager.HasCookieCasters || _frustumParameters.HasFlags(FrustumParametersEnum.EnableSpotLights) && Aura.LightsManager.SpotLightsManager.HasCookieCasters || _frustumParameters.HasFlags(FrustumParametersEnum.EnableDirectionalLights) && Aura.LightsManager.DirectionalLightsManager.HasCookieCasters));

            return GetId();
        }

        /// <summary>
        /// Tells if the specified flags combinaision is found inside the parameters bitmask
        /// </summary>
        /// <param name="flags">A combinaison of flags</param>
        /// <returns>Is the flags combinaison found in the parameters bitmask</returns>
        public bool HasFlags(FrustumParametersEnum flags)
        {
            return _frustumParameters.HasFlags(flags);
        }
        #endregion
    }
}

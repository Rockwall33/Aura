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

namespace AuraAPI
{
    /// <summary>
    /// Bitmask representing the possible parameters for the volumetric data computation
    /// </summary>
    [Flags]
    public enum FrustumParametersEnum
    {
        EnableNothing                           = 0,
        EnableOcclusionCulling                  = 1 << 0,
        EnableTemporalReprojection              = 1 << 1,
        EnableVolumes                           = 1 << 2,
        EnableVolumesNoiseMask                  = 1 << 3,
        EnableVolumesTextureMask                = 1 << 4,
        EnableDirectionalLights                 = 1 << 5,
        EnableDirectionalLightsShadows          = 1 << 6,
        DirectionalLightsShadowsOneCascade      = 1 << 7,
        DirectionalLightsShadowsTwoCascades     = 1 << 8,
        DirectionalLightsShadowsFourCascades    = 1 << 9,
        EnableSpotLights                        = 1 << 10,
        EnableSpotLightsShadows                 = 1 << 11,
        EnablePointLights                       = 1 << 12,
        EnablePointLightsShadows                = 1 << 13,
        EnableLightsCookies                     = 1 << 14
    }
}

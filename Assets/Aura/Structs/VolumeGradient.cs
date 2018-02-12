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
    /// 
    /// </summary>
    [Serializable]
    public struct VolumeGradient
    {
        /// <summary>
        /// Exponent of the fading gradient
        /// </summary>
        public float falloffExponent;

        #region Cube fading parameters
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local X axis
        /// </summary>
        public float xPositiveCubeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local X axis
        /// </summary>
        public float xNegativeCubeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Y axis
        /// </summary>
        public float yPositiveCubeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Y axis
        /// </summary>
        public float yNegativeCubeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Z axis
        /// </summary>
        public float zPositiveCubeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Z axis
        /// </summary>
        public float zNegativeCubeFade;
        #endregion

        #region Cone fading parameters
        /// <summary>
        /// Normalized size of the fading on the borders, depending on the angle
        /// </summary>
        public float angularConeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Z axis
        /// </summary>
        public float distanceConeFade;
        #endregion

        #region Cylinder fading parameters
        /// <summary>
        /// Normalized size of the fading on the borders, depending on the distance with the barycenter
        /// </summary>
        public float widthCylinderFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Y axis
        /// </summary>
        public float yNegativeCylinderFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Y axis
        /// </summary>
        public float yPositiveCylinderFade;
        #endregion

        #region Plane fading parameters
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Y axis
        /// </summary>
        public float heightPlaneFade;
        #endregion

        #region Sphere fading parameters
        /// <summary>
        ///Normalized size of the fading on the borders, depending on the distance with the center
        /// </summary>
        public float distanceSphereFade;
        #endregion
    }
}

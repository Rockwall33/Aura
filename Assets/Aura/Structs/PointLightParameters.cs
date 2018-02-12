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
    /// Struct containing parameters of a point AuraLight
    /// </summary>
    public struct PointLightParameters
    {
        #region Public Members
        public Vector3 color;
        public Vector3 lightPosition;
        public float lightRange;
        public Vector2 distanceFalloffParameters;
        public MatrixFloats worldToShadowMatrix;
        public int shadowMapIndex;
        public float shadowStrength;
        public int cookieMapIndex;
        public Vector3 cookieParameters;
        #endregion

        #region Functions
        /// <summary>
        /// Returns the byte size of the struct
        /// </summary>
        public static int Size
        {
            get
            {
                int byteSize = 0;
                byteSize += sizeof(float) * 3; //color
                byteSize += sizeof(float) * 3; //lightPosition
                byteSize += sizeof(float); //lightRange
                byteSize += sizeof(float) * 2; //distanceFalloffParameters
                byteSize += MatrixFloats.Size; //worldToLightMatrix
                byteSize += sizeof(int); //shadowMapIndex
                byteSize += sizeof(float); //shadowStrength
                byteSize += sizeof(int); //cookieMapIndex
                byteSize += sizeof(float) * 3; //cookieParameters

                return byteSize;
            }
        }
        #endregion
    }
}

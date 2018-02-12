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
    /// Static class containing extension for Vector3 type
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Formats a Vector3 into a array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] AsFloatArray(this Vector3 vector)
        {
            float[] floatArray = new float[3];
            floatArray[0] = vector.x;
            floatArray[1] = vector.y;
            floatArray[2] = vector.z;

            return floatArray;
        }

        /// <summary>
        /// Formats an array of Vector3 into a array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] AsFloatArray(this Vector3[] vector)
        {
            float[] floatArray = new float[vector.Length * 3];

            for(int i = 0; i < vector.Length; ++i)
            {
                floatArray[i * 3] = vector[i].x;
                floatArray[i * 3 + 1] = vector[i].y;
                floatArray[i * 3 + 2] = vector[i].z;
            }

            return floatArray;
        }
    }
}

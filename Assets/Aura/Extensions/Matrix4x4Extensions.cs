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
    /// Collection of extension functions for Matrix4x4 objects
    /// </summary>
    public static class Matrix4X4Extensions
    {
        /// <summary>
        /// Converts the matrix to the MatrixFloats format
        /// </summary>
        /// <returns>The converted MatrixFloats</returns>
        public static MatrixFloats ToAuraMatrixFloats(this Matrix4x4 matrix)
        {
            return MatrixFloats.ToMatrixFloats(matrix);
        }

        /// <summary>
        /// Converts the matrix to an array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] ToFloatArray(this Matrix4x4 matrix)
        {
            float[] matrixFloats =
            {
                matrix[0, 0],
                matrix[1, 0],
                matrix[2, 0],
                matrix[3, 0],
                matrix[0, 1],
                matrix[1, 1],
                matrix[2, 1],
                matrix[3, 1],
                matrix[0, 2],
                matrix[1, 2],
                matrix[2, 2],
                matrix[3, 2],
                matrix[0, 3],
                matrix[1, 3],
                matrix[2, 3],
                matrix[3, 3]
            };

            return matrixFloats;
        }
    }
}

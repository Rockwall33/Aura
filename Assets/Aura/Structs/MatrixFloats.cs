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
    /// Ordered struct of Vector4 representing a matrix to be sent to the compute shader
    /// </summary>
    public struct MatrixFloats
    {
        #region Public Members
        /// <summary>
        /// Matric column A
        /// </summary>
        public Vector4 a;
        /// <summary>
        /// Matric column B
        /// </summary>
        public Vector4 b;
        /// <summary>
        /// Matric column C
        /// </summary>
        public Vector4 c;
        /// <summary>
        /// Matric column D
        /// </summary>
        public Vector4 d;
        #endregion

        #region Functions
        /// <summary>
        /// Returns the bytes size of the struct
        /// </summary>
        public static int Size
        {
            get
            {
                return sizeof(float) * 16;
            }
        }

        /// <summary>
        /// Converts a Matrix4x4 to MatrixFloats format
        /// </summary>
        /// <param name="matrix">The matrix to be converted</param>
        /// <returns>The matrix converted into the MatrixFloats format</returns>
        public static MatrixFloats ToMatrixFloats(Matrix4x4 matrix)
        {
            MatrixFloats matrixFloats = new MatrixFloats
                                        {
                                            a = matrix.GetColumn(0),
                                            b = matrix.GetColumn(1),
                                            c = matrix.GetColumn(2),
                                            d = matrix.GetColumn(3)
                                        };

            return matrixFloats;
        }
        #endregion
    }
}

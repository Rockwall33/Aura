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

namespace AuraAPI
{
    /// <summary>
    /// Ordered struct of Levels operation parameters to be sent to the compute shader
    /// </summary>
    public struct VolumeLevelsData
    {
        #region Public Members
        /// <summary>
        /// Offsets the bottom values (similar to Levels in Photoshop)
        /// </summary>
        public float levelLowThreshold;
        /// <summary>
        /// Offsets the top values (similar to Levels in Photoshop)
        /// </summary>
        public float levelHiThreshold;
        /// <summary>
        /// Output value of the bottom threshold (similar to Levels in Photoshop, except that it is unclamped here)
        /// </summary>
        public float outputLowValue;
        /// <summary>
        /// Output value of the top threshold (similar to Levels in Photoshop, except that it is unclamped here)
        /// </summary>
        public float outputHiValue;
        /// <summary>
        /// Contrast intensity
        /// </summary>
        public float contrast;
        #endregion

        #region Functions
        /// <summary>
        /// Returns the bytes size of the struct
        /// </summary>
        public static int Size
        {
            get
            {
                int byteSize = 0;
                byteSize += sizeof(float); // levelLowThreshold
                byteSize += sizeof(float); // levelHiThreshold
                byteSize += sizeof(float); // outputLowValue
                byteSize += sizeof(float); // outputHiValue
                byteSize += sizeof(float); // contrast

                return byteSize;
            }
        }
        #endregion
    }
}

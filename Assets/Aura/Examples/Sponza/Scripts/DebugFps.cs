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
    /// Computes the average FPS over a specified duration
    /// </summary>
    public class DebugFps : MonoBehaviour
    {
        #region Public Members
        /// <summary>
        /// The duration while the average FPS will be computed
        /// </summary>
        public float interval = 1;
        #endregion

        #region Private Members
        /// <summary>
        /// The accumulated FPS since the last timeout
        /// </summary>
        private float _accumulationValue;
        /// <summary>
        /// The amount of frames since the last timeout
        /// </summary>
        private int _framesCount;
        /// <summary>
        /// The time of the the last timeout
        /// </summary>
        private float _timestamp;
        /// <summary>
        /// The raw FPS (1/deltaTime)
        /// </summary>
        private float _rawFps;
        /// <summary>
        /// The last computed mean FPS
        /// </summary>
        private float _meanFps;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)
        private void Update()
        {
            if(Time.time - _timestamp > interval)
            {
                _meanFps = _accumulationValue / _framesCount;
                _timestamp = Time.time;
                _framesCount = 0;
                _accumulationValue = 0;
            }

            ++_framesCount;
            _rawFps = 1.0f / Time.deltaTime;
            _accumulationValue += _rawFps;
        }

        private void OnGUI()
        {
            GUI.color = Color.white;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Mean FPS over " + interval + " second(s) = " + _meanFps + "\nRaw FPS = " + _rawFps);
        }
        #endregion
    }
}

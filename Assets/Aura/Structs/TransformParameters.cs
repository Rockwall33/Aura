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
    /// Used for transforming reference position for textures/noise inside volumes
    /// </summary>
    [Serializable]
    public struct TransformParameters
    {
        #region Public Members
        /// <summary>
        /// Referential to use for transformations and animations
        /// </summary>
        public Space space;
        /// <summary>
        /// Position of the volume in the selected referencial
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// Rotation of the volume in the selected referencial
        /// </summary>
        public Vector3 rotation;
        /// <summary>
        /// Scale of the volume in the selected referencial
        /// </summary>
        public Vector3 scale;
        /// <summary>
        /// Animate position of the volume in the selected referencial?
        /// </summary>
        public bool animatePosition;
        /// <summary>
        /// Speed of the position offset (in meters per second)
        /// </summary>
        public Vector3 positionSpeed;
        /// <summary>
        /// Animate rotation of the volume in the selected referencial?
        /// </summary>
        public bool animateRotation;
        /// <summary>
        /// Speed of the rotation offset (in degrees per second)
        /// </summary>
        public Vector3 rotationSpeed;
        #endregion

        #region Private Members
        /// <summary>
        /// Timestamp for matrix update
        /// </summary>
        private float _timeStamp;
        #endregion

        #region Functions
        /// <summary>
        /// The resulting transformation matrix
        /// </summary>
        public Matrix4x4 Matrix
        {
            get
            {
                float deltaTime = Aura.Time - _timeStamp;
                _timeStamp = Aura.Time;

                if(animatePosition)
                {
                    position += positionSpeed * deltaTime;
                }
                if(animateRotation)
                {
                    rotation += rotationSpeed * deltaTime;
                    rotation.x = rotation.x % 360.0f;
                    rotation.y = rotation.y % 360.0f;
                    rotation.z = rotation.z % 360.0f;
                }

                return Matrix4x4.TRS(position, Quaternion.Euler(rotation), scale).inverse;
            }
        }
        #endregion
    }
}

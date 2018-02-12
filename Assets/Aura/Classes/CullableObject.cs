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
    /// Class to inherit from when using an ObjectCuller
    /// </summary>
    public abstract class CullableObject : MonoBehaviour
    {
        #region Private Members
        /// <summary>
        /// The bounding sphere used to cull with the camera
        /// </summary>
        private BoundingSphere _boundingSphereMember;
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the bounding sphere used to cull with the camera
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get
            {
                return _boundingSphereMember;
            }
        }

        /// <summary>
        /// Updates the bounding sphere used to cull with the camera
        /// </summary>
        /// <param name="position">The new postition</param>
        /// <param name="radius">The new radius</param>
        public void UpdateBoundingSphere(Vector3 position, float radius)
        {
            _boundingSphereMember.position = position;
            _boundingSphereMember.radius = radius;
        }

        /// <summary>
        /// Updates the bounding sphere used to cull with the camera
        /// </summary>
        /// <param name="boundingSphere">The reference bounding sphere</param>
        public void UpdateBoundingSphere(BoundingSphere boundingSphere)
        {
            _boundingSphereMember = boundingSphere;
        }
        #endregion
    }
}

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
    /// Static class containing extension for Camera type
    /// </summary>
    public static class CameraExtensions
    {
        /// <summary>
        /// Computes the world position of the frustum corners with custom near/far clip distances
        /// </summary>
        /// <param name="nearClipPlaneDistance">The near clip distance</param>
        /// <param name="farClipPlaneDistance">The far clip distance</param>
        /// <returns>
        /// An array with the world position of the corners in the following order : nearTopLeft, nearTopRight, nearBottomRight, nearBottomLeft, farTopLeft, farTopRight, farBottomRight, farBottomLeft
        /// </returns>
        public static Vector4[] GetFrustumCornersWorldPosition(this Camera camera, float nearClipPlaneDistance, float farClipPlaneDistance)
        {
            return new Vector4[]
                   {
                       camera.ViewportToWorldPoint(new Vector3(0, 1, nearClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(1, 1, nearClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(1, 0, nearClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(0, 0, nearClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(0, 1, farClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(1, 1, farClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(1, 0, farClipPlaneDistance)),
                       camera.ViewportToWorldPoint(new Vector3(0, 0, farClipPlaneDistance))
                   };
        }
    }
}

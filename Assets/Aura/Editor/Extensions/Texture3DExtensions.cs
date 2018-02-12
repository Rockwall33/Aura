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
    /// Extensions for Texture3D object so we can just invoke functions on them
    /// </summary>
    public static class Texture3DExtensions
    {
        #region Private Members
        /// <summary>
        /// The material used to render the Texture3D
        /// </summary>
        private static Material _previewTexture3DMaterial;
        /// <summary>
        /// The texture that will be used for the background
        /// </summary>
        private static Texture2D _backgroundTexture;
        /// <summary>
        /// The GUIStyle that will be used for the background
        /// </summary>
        private static GUIStyle _backgroundGuiStyle;
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the material used to render the Texture3D
        /// </summary>
        private static Material PreviewTexture3DMaterial
        {
            get
            {
                if(Texture3DExtensions._previewTexture3DMaterial == null)
                {
                    Texture3DExtensions._previewTexture3DMaterial = new Material(Shader.Find("Hidden/Aura/DrawTexture3DPreview"));
                }

                return Texture3DExtensions._previewTexture3DMaterial;
            }
        }

        /// <summary>
        /// Accessor to the static background texture
        /// </summary>
        public static Texture2D BackgroundTexture
        {
            get
            {
                if(Texture3DExtensions._backgroundTexture == null)
                {
                    Texture3DExtensions._backgroundTexture = new Texture2D(1, 1);
                    Texture3DExtensions._backgroundTexture.SetPixel(0, 0, Color.gray * 0.5f);
                    Texture3DExtensions._backgroundTexture.Apply();
                }

                return Texture3DExtensions._backgroundTexture;
            }
        }

        /// <summary>
        /// Accessor to the static background GUIStyle
        /// </summary>
        public static GUIStyle BackgroundGuiStyle
        {
            get
            {
                Texture3DExtensions._backgroundGuiStyle = new GUIStyle();
                Texture3DExtensions._backgroundGuiStyle.active.background = Texture3DExtensions.BackgroundTexture;
                Texture3DExtensions._backgroundGuiStyle.focused.background = Texture3DExtensions.BackgroundTexture;
                Texture3DExtensions._backgroundGuiStyle.hover.background = Texture3DExtensions.BackgroundTexture;
                Texture3DExtensions._backgroundGuiStyle.normal.background = Texture3DExtensions.BackgroundTexture;

                return Texture3DExtensions._backgroundGuiStyle;
            }
        }

        /// <summary>
        /// Sets the parameters to the PreviewRenderUtility and calls the rendering
        /// </summary>
        /// <param name="texture3D">The Texture3D to preview</param>
        /// <param name="angle">The camera angle</param>
        /// <param name="distance">The distance of the camera to the preview cube</param>
        /// <param name="samplingIterations">The amount of slices used to raymarch in the Texture3D</param>
        /// <param name="density">A linear factor to multiply the Texture3D with</param>
        private static void RenderInPreviewRenderUtility(Texture3D texture3D, Vector2 angle, float distance, int samplingIterations, float density)
        {
            Texture3DExtensions.PreviewTexture3DMaterial.SetInt("_SamplingQuality", samplingIterations);
            Texture3DExtensions.PreviewTexture3DMaterial.SetTexture("_MainTex", texture3D);
            Texture3DExtensions.PreviewTexture3DMaterial.SetFloat("_Density", density);

            PreviewRenderUtilityHelpers.Instance.DrawMesh(MeshHelpers.Cube, Matrix4x4.identity, Texture3DExtensions.PreviewTexture3DMaterial, 0);

            PreviewRenderUtilityHelpers.Instance.camera.transform.position = Vector2.zero;
            PreviewRenderUtilityHelpers.Instance.camera.transform.rotation = Quaternion.Euler(new Vector3(-angle.y, -angle.x, 0));
            PreviewRenderUtilityHelpers.Instance.camera.transform.position = PreviewRenderUtilityHelpers.Instance.camera.transform.forward * -distance;
            PreviewRenderUtilityHelpers.Instance.camera.Render();
        }

        /// <summary>
        /// Renders a preview of the Texture3D
        /// </summary>
        /// <param name="texture3D">The Texture3D to preview</param>
        /// <param name="rect">The area where the preview is located</param>
        /// <param name="angle">The camera angle</param>
        /// <param name="distance">The distance of the camera to the preview cube</param>
        /// <param name="samplingIterations">The amount of slices used to raymarch in the Texture3D</param>
        /// <param name="density">A linear factor to multiply the Texture3D with</param>
        /// <returns>A Texture with the preview</returns>
        public static Texture RenderTexture3DPreview(this Texture3D texture3D, Rect rect, Vector2 angle, float distance, int samplingIterations, float density)
        {
            PreviewRenderUtilityHelpers.Instance.BeginPreview(rect, Texture3DExtensions.BackgroundGuiStyle);

            Texture3DExtensions.RenderInPreviewRenderUtility(texture3D, angle, distance, samplingIterations, density);

            return PreviewRenderUtilityHelpers.Instance.EndPreview();
        }

        /// <summary>
        /// Renders a thumbnail of the Texture3D
        /// </summary>
        /// <param name="texture3D">The Texture3D to preview</param>
        /// <param name="rect">The area where the preview is located</param>
        /// <param name="angle">The camera angle</param>
        /// <param name="distance">The distance of the camera to the preview cube</param>
        /// <param name="samplingIterations">The amount of slices used to raymarch in the Texture3D</param>
        /// <param name="density">A linear factor to multiply the Texture3D with</param>
        /// <returns>A Texture2D with the thumbnail</returns>
        public static Texture2D RenderTexture3DStaticPreview(this Texture3D texture3D, Rect rect, Vector2 angle, float distance, int samplingIterations, float density)
        {
            PreviewRenderUtilityHelpers.Instance.BeginStaticPreview(rect);

            Texture3DExtensions.RenderInPreviewRenderUtility(texture3D, angle, distance, samplingIterations, density);

            return PreviewRenderUtilityHelpers.Instance.EndStaticPreview();
        }
        #endregion
    }
}

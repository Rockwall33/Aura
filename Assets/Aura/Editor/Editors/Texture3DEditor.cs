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

using UnityEditor;
using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Custom Inspector for Texture3D class
    /// </summary>
    [CustomEditor(typeof(Texture3D))]
    public class Texture3DEditor : Editor
    {
        #region Public Members
        /// <summary>
        /// Aura Logo
        /// </summary>
        public Texture2D logoTexture;
        #endregion

        #region Private Members
        /// <summary>
        /// The angle of the camera preview
        /// </summary>
        private Vector2 _cameraAngle = new Vector2(127.5f, -22.5f); // This default value will be used when rendering the asset thumbnail (see RenderStaticPreview)
        /// <summary>
        /// The raymarch interations
        /// </summary>
        private int _samplingIterations = 64;
        /// <summary>
        /// The factor of the Texture3D
        /// </summary>
        private float _density = 1;
        //// TODO : INVESTIGATE TO ACCESS THOSE VARIABLES AS THE DEFAULT INSPECTOR IS UGLY
        //private SerializedProperty wrapModeProperty;
        //private SerializedProperty filterModeProperty;
        //private SerializedProperty anisotropyLevelProperty;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        /// <summary>
        /// Draws the content of the Inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            GuiHelpers.DrawHeader(logoTexture);

            //serializedObject.Update();

            //// HAD TO DISABLE THE DEFAULT INSPECTOR AS IT MADE PREVIEW LAG
            //DrawDefaultInspector();

            //serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Tells if the Object has a custom preview
        /// </summary>
        public override bool HasPreviewGUI()
        {
            return true;
        }

        /// <summary>
        /// Draws the toolbar area on top of the preview window
        /// </summary>
        public override void OnPreviewSettings()
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Reset Camera", EditorStyles.miniButton))
            {
                ResetPreviewCameraAngle();
            }
            EditorGUILayout.LabelField("Quality", GUILayout.MaxWidth(50));
            _samplingIterations = EditorGUILayout.IntPopup(_samplingIterations, new string[]
                                                                                {
                                                                                    "16",
                                                                                    "32",
                                                                                    "64",
                                                                                    "128",
                                                                                    "256",
                                                                                    "512"
                                                                                }, new int[]
                                                                                   {
                                                                                       16,
                                                                                       32,
                                                                                       64,
                                                                                       128,
                                                                                       256,
                                                                                       512
                                                                                   }, GUILayout.MaxWidth(50));
            EditorGUILayout.LabelField("Density", GUILayout.MaxWidth(50));
            _density = EditorGUILayout.Slider(_density, 0, 5, GUILayout.MaxWidth(200));
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the preview area
        /// </summary>
        /// <param name="rect">The area of the preview window</param>
        /// <param name="backgroundStyle">The default GUIStyle used for preview windows</param>
        public override void OnPreviewGUI(Rect rect, GUIStyle backgroundStyle)
        {
            _cameraAngle = PreviewRenderUtilityHelpers.DragToAngles(_cameraAngle, rect);

            if(Event.current.type == EventType.Repaint)
            {
                GUI.DrawTexture(rect, ((Texture3D)serializedObject.targetObject).RenderTexture3DPreview(rect, _cameraAngle, 6.5f /*TODO : Find distance with fov and boundingsphere, when non uniform size will be supported*/, _samplingIterations, _density), ScaleMode.StretchToFill, true);
            }
        }

        /// <summary>
        /// Draws the custom preview thumbnail for the asset in the Project window
        /// </summary>
        /// <param name="assetPath">Path of the asset</param>
        /// <param name="subAssets">Array of children assets</param>
        /// <param name="width">Width of the rendered thumbnail</param>
        /// <param name="height">Height of the rendered thumbnail</param>
        /// <returns></returns>
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            return ((Texture3D)serializedObject.targetObject).RenderTexture3DStaticPreview(new Rect(0, 0, width, height), _cameraAngle, 6.5f /*TODO : Find distance with fov and boundingsphere, when non uniform size will be supported*/, _samplingIterations, _density);
        }

        /// <summary>
        /// Allows to give a custom title to the preview window
        /// </summary>
        /// <returns></returns>
        public override GUIContent GetPreviewTitle()
        {
            return new GUIContent(serializedObject.targetObject.name + " preview");
        }
        #endregion

        #region Functions
        /// <summary>
        /// Sets back the camera angle
        /// </summary>
        public void ResetPreviewCameraAngle()
        {
            _cameraAngle = new Vector2(127.5f, -22.5f);
        }
        #endregion
    }
}

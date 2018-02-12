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
using UnityEditor;

namespace AuraAPI
{
    /// <summary>
    /// Collection of function/variables related to the PreviewRenderUtility class
    /// </summary>
    public static class PreviewRenderUtilityHelpers
    {
        #region Private Members
        /// <summary>
        /// A static copy of the PreviewRenderUtility class
        /// </summary>
        private static PreviewRenderUtility _instance;
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the PreviewRenderUtility instance
        /// </summary>
        public static PreviewRenderUtility Instance
        {
            get
            {
                if(PreviewRenderUtilityHelpers._instance == null)
                {
                    PreviewRenderUtilityHelpers._instance = new PreviewRenderUtility();
                }

                return PreviewRenderUtilityHelpers._instance;
            }
        }

        /// <summary>
        /// Transforms the drag delta of the mouse on the UI into Euler angles
        /// </summary>
        /// <param name="angles">Input angles</param>
        /// <param name="position">The area where the mouse will be watched</param>
        /// <returns>The modified angles</returns>
        public static Vector2 DragToAngles(Vector2 angles, Rect position)
        {
            int controlId = GUIUtility.GetControlID("DragToAngles".GetHashCode(), FocusType.Passive);
            Event current = Event.current;
            switch(current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown :
                    if(position.Contains(current.mousePosition))
                    {
                        GUIUtility.hotControl = controlId;
                        current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp :
                    if(GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                    }
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    break;
                case EventType.MouseDrag :
                    if(GUIUtility.hotControl == controlId)
                    {
                        angles -= current.delta / Mathf.Min(position.width, position.height) * 180;
                        current.Use();
                        GUI.changed = true;
                    }
                    break;
            }

            return angles;
        }
        #endregion
    }
}

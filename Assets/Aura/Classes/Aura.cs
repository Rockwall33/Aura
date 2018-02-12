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
using UnityEngine.Profiling;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AuraAPI
{
    /// <summary>
    ///     Main component to assign on a GameObject with a Camera component
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Aura/Aura Main Component", 0)]
    [ExecuteInEditMode]
    public class Aura : MonoBehaviour
    {
        #region Public members
        /// <summary>
        /// The frustum inside which the data will be computed
        /// </summary>
        [SerializeField]
        public Frustum frustum;
        /// <summary>
        /// Enabled the volumetric lighting to be applied as a Post Process 
        /// </summary>
        public bool applyAsPostProcess = true;
        /// <summary>
        /// DO NOT CHANGE : Compute Shader in charge of computing the maximum depth for occlusion culling
        /// </summary>
        public ComputeShader computeMaximumDepthComputeShader;
        /// <summary>
        /// DO NOT CHANGE : Shader in charge of filtering and formatting the maximum depth
        /// </summary>
        public Shader processOcclusionMapShader;
        /// <summary>
        /// DO NOT CHANGE : Compute Shader in charge of computing the data contribution inside the frustum
        /// </summary>
        public ComputeShader computeDataComputeShader;
        /// <summary>
        /// DO NOT CHANGE : Compute Shader in charge of accumulating the data
        /// </summary>
        public ComputeShader computeAccumulationComputeShader;
        /// <summary>
        /// DO NOT CHANGE : Shader used for applying the volumetric lighting as Post Process
        /// </summary>
        public Shader postProcessShader;
        /// <summary>
        /// DO NOT CHANGE : Texture containing blue noise for dithering volumetric lighting
        /// </summary>
        public Texture2DArray blueNoiseTexturesArray;
        #endregion

        #region Private members
        /// <summary>
        /// Has the component been assigned/reset
        /// </summary>
        [SerializeField]
        private bool _hasBeenAssigned;
        /// <summary>
        /// The material used for applying the volumetric lights as Post Process
        /// </summary>
        private Material _postProcessMaterial;
#if UNITY_EDITOR /// <summary>
/// Custom time used during edition
/// </summary>
        private static double editorTime;
        /// <summary>
        /// Custom delta time used during edition
        /// </summary>
        private static float editorDeltaTime;
#endif
        #endregion

        #region Events
        /// <summary>
        /// Event triggered during the OnPreCull stage of the Camera
        /// </summary>
        public static event Action OnPreCullEvent;
        /// <summary>
        /// Event triggered during the OnPreRender stage of the Camera
        /// </summary>
        public static event Action<Camera> OnPreRenderEvent;
        /// <summary>
        /// Event triggered during the OnRenderImage stage of the Camera
        /// </summary>
        public static event Action OnRenderImageEvent;
        #endregion

        #region Monobehavious functions
        private void Reset()
        {
            if(!Aura.CheckCompatibility())
            {
                enabled = false;
                return;
            }

            SetupFrustum();

            if(!_hasBeenAssigned)
            {
                Initialize();
                _hasBeenAssigned = true;
            }
        }

        private void OnEnable()
        {
            if(!Aura.CheckCompatibility())
            {
                enabled = false;
                return;
            }

            if(_hasBeenAssigned && !Aura.IsInitialized)
            {
                Initialize();
            }
        }

        private void OnDisable()
        {
            Shader.DisableKeyword("USE_AURA");

            DisposeFrustrum();
            Aura.VolumesManager.Dispose();
            Aura.LightsManager.Dispose();
            Aura.Instance = null;

            Aura.IsInitialized = false;
        }

        private void OnPreCull()
        {
            if(Aura.OnPreCullEvent != null)
            {
                Aura.OnPreCullEvent();
            }
        }

        private void OnPreRender()
        {
            if(Aura.OnPreRenderEvent != null)
            {
                Aura.OnPreRenderEvent(Aura.CameraComponent);
            }
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
#if UNITY_EDITOR
            UpdateEditorTimeData();
#endif

            if(Aura.OnRenderImageEvent != null)
            {
                Aura.OnRenderImageEvent();
            }

            Aura.LightsManager.Update();

            Shader.SetGlobalInt("_frameID", Aura.FrameId);

            UpdateFrustrum();

            if(applyAsPostProcess)
            {
                Profiler.BeginSample("Aura : Post process");
                Graphics.Blit(src, dest, _postProcessMaterial);
                Profiler.EndSample();
            }
            else
            {
                Graphics.CopyTexture(src, dest);
            }

            ++Aura.FrameId;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the current instance of the Aura component
        /// </summary>
        public static Aura Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current instance of the Aura component
        /// </summary>
        public static bool HasInstance
        {
            get
            {
                return Aura.Instance != null;
            }
        }

        /// <summary>
        /// Gets the Camera component
        /// </summary>
        public static Camera CameraComponent
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the Volumes Manager
        /// </summary>
        public static VolumesManager VolumesManager
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the Lights Manager
        /// </summary>
        public static LightsManager LightsManager
        {
            get;
            private set;
        }
        /// <summary>
        /// Tells if the Aura component is initialized
        /// </summary>
        public static bool IsInitialized
        {
            get;
            private set;
        }
        /// <summary>
        /// The frame count since the begining
        /// </summary>
        public static int FrameId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the elapsed Time 
        /// </summary>
        public static float Time
        {
            get
            {
#if !UNITY_EDITOR
                return UnityEngine.Time.time;
#else
                return (float)editorTime;
#endif
            }
        }

        /// <summary>
        /// Gets the current Delta Time 
        /// </summary>
        public static float DeltaTtime
        {
            get
            {
#if !UNITY_EDITOR
                return UnityEngine.Time.deltaTime;
#else
                return editorDeltaTime;
#endif
            }
        }

#if UNITY_EDITOR 
        /// <summary>
        /// Updates the custom Time and DeltaTime and repaints the GameView if focused
        /// </summary>
        private void UpdateEditorTimeData()
        {
            double currentTime = EditorApplication.timeSinceStartup;
            editorDeltaTime = (float)(currentTime - Time);
            editorTime = currentTime;
            //editorTime += 1.0f/60.0f; //For recorderd fixed framerate

            EditorWindow window = EditorWindow.focusedWindow;
            bool isGameViewFocused = window != null && window.GetType().ToString() == "UnityEditor.GameView";
            if (!Application.isPlaying && isGameViewFocused)
            {
                window.Repaint();
            }
        }
#endif
        /// <summary>
        /// Setups variables
        /// </summary>
        private void SetupFrustum()
        {
            frustum = new Frustum(computeMaximumDepthComputeShader, processOcclusionMapShader, computeDataComputeShader, computeAccumulationComputeShader);
        }

        /// <summary>
        /// Initialize the Aura component and its variables
        /// </summary>
        private void Initialize()
        {
            Aura.Instance = this;
            Aura.CameraComponent = GetComponent<Camera>();
            Aura.CameraComponent.depthTextureMode = DepthTextureMode.Depth;
            Aura.VolumesManager = new VolumesManager();
            Aura.LightsManager = new LightsManager();

            _postProcessMaterial = new Material(postProcessShader);
            Shader.SetGlobalTexture("_blueNoiseTexturesArray", blueNoiseTexturesArray);

            frustum.SetResolution(frustum.settings.resolution);

            Shader.EnableKeyword("USE_AURA");

            Aura.IsInitialized = true;
        }

        /// <summary>
        /// Updates the Frustum
        /// </summary>
        private void UpdateFrustrum()
        {
            frustum.ComputeData();
        }

        /// <summary>
        /// Properly dispose the Frustum and its managed variables
        /// </summary>
        private void DisposeFrustrum()
        {
            frustum.Dispose();
        }

        /// <summary>
        /// Checks if the environment is able to run Aura
        /// </summary>
        /// <returns>True if the environment is able to run Aura, false otherwise</returns>
        public static bool CheckCompatibility()
        {
#if UNITY_2017_2_OR_NEWER
            bool isCompatible =
                SystemInfo.graphicsShaderLevel >= 50 &&                                                     // For compute shader support
                SystemInfo.supports2DArrayTextures &&                                                       // Uses Texture2DArrays for packing textures
                SystemInfo.supports3DTextures &&                                                            // For volumetric texture masks and point shadows
                SystemInfo.supports3DRenderTextures &&                                                      // For point shadows
                SystemInfo.supportsComputeShaders &&                                                        // Checks compute shader support
                SystemInfo.supportsRawShadowDepthSampling;                                                  // Shadow maps are stored in raw float format

            return isCompatible;
#else
            Debug.LogError("The current version of Unity is not compatible with Aura");
            return false;
#endif
        }
        #endregion
    }

#if UNITY_EDITOR /// <summary>
/// Custom Gizmo drawer for Aura component
/// </summary>
    public class AuraGizmoDrawer
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NonSelected | GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
        static void DrawGizmoForAura(Aura component, GizmoType gizmoType)
        {
            bool isFaded = (int)gizmoType == (int)GizmoType.NonSelected || (int)gizmoType == (int)GizmoType.NotInSelectionHierarchy || (int)gizmoType == (int)GizmoType.NonSelected + (int)GizmoType.NotInSelectionHierarchy;
            float opacity = isFaded ? 0.25f : 1.0f;

            Matrix4x4 tmp = Gizmos.matrix;
            Gizmos.matrix = component.GetComponent<Camera>().transform.localToWorldMatrix;
            Gizmos.color = new Color( 0, 1, 1, opacity);
            Gizmos.DrawFrustum(Vector3.zero, component.GetComponent<Camera>().fieldOfView, component.frustum.settings.farClipPlaneDistance, component.GetComponent<Camera>().nearClipPlane, component.GetComponent<Camera>().aspect);
            Gizmos.matrix = tmp;
        }
    }
#endif
}

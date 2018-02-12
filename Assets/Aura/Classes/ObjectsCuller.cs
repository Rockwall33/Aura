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

using System.Collections.Generic;
using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Culls candidate objects according to the ranged camera frustum and puts an array of visible objects at disposal
    /// </summary>
    /// <typeparam name="T">The type of objects that the culler will have to handle. Must inherit from CullableObject</typeparam>
    internal class ObjectsCuller<T> where T : CullableObject
    {
        #region Private Members
        /// <summary>
        /// List of canditate objects to cull
        /// </summary>
        private readonly List<T> _registredObjectsList;
        /// <summary>
        /// List of visible objects after culling
        /// </summary>
        private T[] _visibleObjectsArray;
        /// <summary>
        /// The Aura component on the camera
        /// </summary>
        private readonly Aura _auraComponent;
        /// <summary>
        /// Array of one float distance for the culler.
        /// </summary>
        private readonly float[] _cullingDistance;
        /// <summary>
        /// The culling group that will handle the culling computation
        /// </summary>
        private CullingGroup _cullingGroup;
        /// <summary>
        /// Array of the bounding spheres to send to the culling group
        /// </summary>
        private BoundingSphere[] _boundingSpheres;
        /// <summary>
        /// Array with the indices of the visible bounding spheres. Indices of bounding spheres and registred objects are directly linked
        /// </summary>
        private int[] _visibleObjectsIndices;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectsCuller()
        {
            _registredObjectsList = new List<T>();
            _auraComponent = Aura.Instance;
            _cullingDistance = new float[1];
            _cullingGroup = new CullingGroup();
            _cullingGroup.targetCamera = Aura.CameraComponent;
            _cullingGroup.SetDistanceReferencePoint(_auraComponent.transform);

            Aura.OnPreCullEvent += Aura_onPreCullEvent;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Tells if the culler has candidate objects
        /// </summary>
        public bool HasRegistredObjects
        {
            get
            {
                return _registredObjectsList.Count > 0;
            }
        }

        /// <summary>
        /// Tells if there are visible objects
        /// </summary>
        public bool HasVisibleObjects
        {
            get
            {
                return _visibleObjectsArray != null && _visibleObjectsArray.Length > 0;
            }
        }

        /// <summary>
        /// Tells the amount of visible objects
        /// </summary>
        public int VisibleObjectsCount
        {
            get
            {
                return _visibleObjectsArray != null ? _visibleObjectsArray.Length : 0;
            }
        }

        /// <summary>
        /// Tells if culling group is initialized
        /// </summary>
        private bool IsCullingGroupValid
        {
            get
            {
                return _cullingGroup != null;
            }
        }

        /// <summary>
        /// Disposes the managed members and unregisters events
        /// </summary>
        public void Dispose()
        {
            _cullingGroup.Dispose();
            _cullingGroup = null;

            Aura.OnPreCullEvent -= Aura_onPreCullEvent;
        }

        /// <summary>
        /// Sets the culling group parameters
        /// </summary>
        private void SetupCullingGroup()
        {
            if(HasRegistredObjects)
            {
                _cullingGroup.enabled = true;
                _boundingSpheres = new BoundingSphere[_registredObjectsList.Count];
                _cullingGroup.SetBoundingSpheres(_boundingSpheres);
                _visibleObjectsIndices = new int[_boundingSpheres.Length];
                _cullingGroup.SetBoundingSphereCount(_registredObjectsList.Count);
            }
            else
            {
                _cullingGroup.enabled = false;
            }
        }

        /// <summary>
        /// Registers a new candidate object
        /// </summary>
        /// <param name="candidate">The candidate object to be culled</param>
        public void Register(T candidate)
        {
            if(!_registredObjectsList.Contains(candidate) && IsCullingGroupValid)
            {
                _registredObjectsList.Add(candidate);
                SetupCullingGroup();
            }
        }

        /// <summary>
        /// Unregisters the candiadate object
        /// </summary>
        /// <param name="volume"></param>
        public void Unregister(T volume)
        {
            if(_registredObjectsList.Contains(volume) && IsCullingGroupValid)
            {
                _registredObjectsList.Remove(volume);
                SetupCullingGroup();
            }
        }

        /// <summary>
        /// Updates the culler.
        /// </summary>
        public void Update()
        {
            if(HasRegistredObjects && _cullingGroup != null)
            {
                for(int i = 0; i < _registredObjectsList.Count; ++i)
                {
                    _boundingSpheres[i] = _registredObjectsList[i].BoundingSphere;
                }

                int visibleObjectsCount = _cullingGroup.QueryIndices(0, _visibleObjectsIndices, 0);
                _visibleObjectsArray = new T[visibleObjectsCount];
                for(int i = 0; i < visibleObjectsCount; ++i)
                {
                    _visibleObjectsArray[i] = _registredObjectsList[_visibleObjectsIndices[i]];
                }
            }
            else
            {
                _visibleObjectsArray = null;
            }
        }

        /// <summary>
        /// Retreives the array of visible objects
        /// </summary>
        /// <returns></returns>
        public T[] GetVisibleObjects()
        {
            return _visibleObjectsArray;
        }

        /// <summary>
        /// Called when the camera is on pre cull
        /// </summary>
        private void Aura_onPreCullEvent()
        {
            if(_cullingGroup == null)
            {
                Aura.OnPreCullEvent -= Aura_onPreCullEvent;
                return;
            }

            _cullingDistance[0] = _auraComponent.frustum.settings.farClipPlaneDistance;
            _cullingGroup.SetBoundingDistances(_cullingDistance);
        }
        #endregion
    }
}

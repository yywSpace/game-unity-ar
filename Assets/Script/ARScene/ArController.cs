//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using GoogleARCore;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;

namespace Script.ARScene
{
    /// <summary>
    /// 管理AR的生命周期，初始化AR模型
    /// </summary>
    public class ArController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera firstPersonCamera;

        public GameObject dialogueBox;

        public GameObject map;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool _mIsQuitting = false;

        private bool _hasLoadModelInMiniMap;
        private List<ArModelInfo> _showedArModels;
        private AbstractMap _abstractMap;
        List<Task> _taskList;

        // todo 读取任务，初始化模型
        public void Start()
        {
            _showedArModels = new List<ArModelInfo>();
            // 初始化模型经纬度andyObject1
            _abstractMap = map.GetComponent<AbstractMap>();
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();
            Vector3 position = _abstractMap.GeoToWorldPosition(TaskLab.Get().GetTaskList()[0].TaskLocation);
            // 等待abstractMap加载完成
            if (position != Vector3.zero && !_hasLoadModelInMiniMap)
            {
                var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                TaskLab.Get().SetCurrentLatlng(locationProvider.CurrentLocation.LatitudeLongitude);
                _taskList = TaskLab.Get().GetTaskListIn(200);
                InitModelByLatLngs(_taskList, true);
                _hasLoadModelInMiniMap = true;
            }
            // todo 一定范围内模型
            // 每个几帧重新更新矛点位置
            //if (++count % 1000 == 0)
            //{
            //    DestoryShowedARModels(ShowedARModels);
            //    InitModelByLatLngs(TaskList, false);
            //}
        }

        public List<ArModelInfo> GetShowedArModels()
        {
            return _showedArModels;
        }


        void DestoryShowedArModels(List<ArModelInfo> showedArModels)
        {
            foreach (var modelInfo in showedArModels)
            {
                Destroy(modelInfo.ArGameObject);
                Destroy(modelInfo.Anchor);
                showedArModels.Remove(modelInfo);
            }
        }

        /// <summary>
        /// Inits the model by latlngs.
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="deviceOfUserHead">If set to <c>true</c> use device orientation otherwise use user heading.</param>
        void InitModelByLatLngs(List<Task> tasks, bool deviceOfUserHead)
        {
            foreach (var task in tasks)
            {
                Vector3 position = _abstractMap.GeoToWorldPosition(task.TaskLocation);
                print(position);
                Object model = ArUtils.LoadModel("Model/"+task.TaskModelName);
                print(model);
                // 地图上显示的物体
                var mapObject = Instantiate(model, position, task.TaskModelRotation) as GameObject;
                mapObject.layer = 0;
                // ar界面显示的物体，两者为模型相同位置不同
                var arObject = CreateArModel(model, position, Quaternion.Euler(new Vector3()));
                ArModelEventController amec = arObject.ArGameObject.AddComponent<ArModelEventController>();
                amec.SetCamera(firstPersonCamera);
                amec.onModelClick = () =>
                {
                    print("click model");
                    dialogueBox.SetActive(!dialogueBox.activeSelf);
                    if (dialogueBox.activeSelf)
                    {
                        
                        dialogueBox.GetComponent<DialogueBoxController>().SetTaskDesc(task.TaskDesc);
                    }
                };
                //MessageBubblesShow mbs = arObject.ARGameObject.AddComponent<MessageBubblesShow>();
                //ModelIndicator mi = arObject.ArGameObject.AddComponent<ModelIndicator>();
                //mi.SetCameraObject(FirstPersonCamera);
                //mbs.SetTaskDesc(task.TaskDesc);
                //mbs.SetCamera(FirstPersonCamera); 
                AdjustArModelByDeviceOrientation(arObject.ArGameObject, deviceOfUserHead);
                _showedArModels.Add(arObject);
            }
        }


        /// <summary>
        /// Creates the AR Model.
        /// </summary>
        /// <returns>The AR Model.</returns>
        /// <param name="model">Model.</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public static ArModelInfo CreateArModel(Object model, Vector3 position, Quaternion rotation)
        {
            ArModelInfo modelInfo = new ArModelInfo();
            Pose pose = new Pose(position, rotation);
           // Anchor anchor = Session.CreateAnchor(pose);
            var arObject = Instantiate(model, position, rotation) as GameObject;
            print("Model:" + model);
            print("ARObject:" + arObject);
            print("position:" + position);
            print("Quaternion:" + rotation);
            //print("Anchor:" + anchor);
            if (arObject != null)
            {
                //arObject.transform.parent = anchor.transform;
                // 设置层级为Model令出FirstPersonCamera之外的所有相机不可视
                arObject.layer = 8;
                modelInfo.Pose = pose;
                modelInfo.ArGameObject = arObject;
            }

            //modelInfo.Anchor = anchor;
            return modelInfo;
        }
        /// <summary>
        /// Adjust the AR Model by device orientation.
        /// </summary>
        /// <param name="go">被调整的物体</param>
        /// <param name="deviceOfUserHead">If set to <c>true</c> use device orientation otherwise use user heading.</param>
        private void AdjustArModelByDeviceOrientation(GameObject go, bool deviceOfUserHead)
        {
            var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            float orientation;
            if (deviceOfUserHead == true)
                orientation = locationProvider.CurrentLocation.DeviceOrientation;
            else
                orientation = locationProvider.CurrentLocation.UserHeading;

            float differentOfMapAndUserDegree;
            if (orientation >= 180)
                differentOfMapAndUserDegree = 360 - orientation + _abstractMap.transform.rotation.eulerAngles.y;
            else
                differentOfMapAndUserDegree = 0 - orientation + _abstractMap.transform.rotation.eulerAngles.y;
            go.transform.RotateAround(Vector3.zero, Vector3.up, differentOfMapAndUserDegree);
            print(differentOfMapAndUserDegree);
            _ShowAndroidToastMessage(-orientation + "");
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (_mIsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                _mIsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                _mIsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
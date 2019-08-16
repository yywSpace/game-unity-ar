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
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 管理AR的生命周期，初始化AR模型
/// </summary>
public class ARController : MonoBehaviour
{
    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background).
    /// </summary>
    public Camera FirstPersonCamera;

    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error,
    /// otherwise false.
    /// </summary>
    private bool m_IsQuitting = false;

    private bool hasLoadModelInMiniMap;
    private List<ARModelInfo> ShowedARModels;
    public GameObject map;
    private AbstractMap abstractMap;
    List<Task> TaskList;

    // todo 读取任务，初始化模型
    public void Start()
    {
        ShowedARModels = new List<ARModelInfo>();
        // 初始化模型经纬度andyObject1
        abstractMap = map.GetComponent<AbstractMap>();
    }

    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    public void Update()
    {
        _UpdateApplicationLifecycle();
        Vector3 position = abstractMap.GeoToWorldPosition(TaskLab.get().GetTaskList()[0].TaskLocation);
        // 等待abstractMap加载完成
        if (position != Vector3.zero && !hasLoadModelInMiniMap)
        {
            var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            TaskLab.get().SetCurrentLatlng(locationProvider.CurrentLocation.LatitudeLongitude);
            TaskList = TaskLab.get().GetTaskListIn(200);
            InitModelByLatLngs(TaskList, true);
            hasLoadModelInMiniMap = true;
            print("werqerqr");
        }
        // todo 一定范围内模型
         // 每个几帧重新更新矛点位置
        //if (++count % 1000 == 0)
        //{
        //    DestoryShowedARModels(ShowedARModels);
        //    InitModelByLatLngs(TaskList, false);
        //}
    }

    public List<ARModelInfo> GetShowedARModels()
    {
        return ShowedARModels;
    }


    void DestoryShowedARModels(List<ARModelInfo> showedARModels)
    {
        foreach (var modelInfo in showedARModels)
        {
            Destroy(modelInfo.ARGameObject);
            Destroy(modelInfo.Anchor);
            Destroy(modelInfo.RelatedCoordinate);
            showedARModels.Remove(modelInfo);
        }
    }
    /// <summary>
    /// Inits the model by latlngs.
    /// </summary>
    /// <param name="deviceOfUserHead">If set to <c>true</c> use device orientation otherwise use user heading.</param>
    void InitModelByLatLngs(List<Task> tasks, bool deviceOfUserHead)
    {
        foreach (var task in tasks)
        {
            Vector3 position = abstractMap.GeoToWorldPosition(task.TaskLocation);
            print(position);
            Object model = ARUtils.LoadModel("Model/"+task.TaskModelName);
            print(model);
            // 地图上显示的物体
            var mapObject = Instantiate(model, position, task.TaskModelRotation) as GameObject;
            mapObject.layer = 0;
            // ar界面显示的物体，两者为模型相同位置不同
            var arObject = CreateARModel(model, position, Quaternion.Euler(new Vector3()));
            MessageBubblesShow mbs = arObject.ARGameObject.AddComponent<MessageBubblesShow>();
            ModelIndicator mi = arObject.ARGameObject.AddComponent<ModelIndicator>();
            mi.SetCameraObject(FirstPersonCamera);
            mbs.SetTaskDesc(task.TaskDesc);
            mbs.SetCamera(FirstPersonCamera); 
            AdjestARModelByDeviceOrientation(arObject.ARGameObject, deviceOfUserHead);
            ShowedARModels.Add(arObject);
        }
    }


    /// <summary>
    /// Creates the AR Model.
    /// </summary>
    /// <returns>The AR Model.</returns>
    /// <param name="model">Model.</param>
    /// <param name="position">Position.</param>
    /// <param name="rotation">Rotation.</param>
    public static ARModelInfo CreateARModel(Object model, Vector3 position, Quaternion rotation)
    {
        ARModelInfo modelInfo = new ARModelInfo();
        Pose pose = new Pose(position, rotation);
       //Anchor anchor = Session.CreateAnchor(pose);
        var arObject = Instantiate(model, position, rotation) as GameObject;
        print("Model:" + model);
        print("ARObject:" + arObject);
        print("position:" + position);
        print("Quaternion:" + rotation);
        //print("Anchor:" + anchor);
        //arObject.transform.parent = anchor.transform;
        // 设置层级为Model令出FirstPersonCamera之外的所有相机不可视
        arObject.layer = 8;
        modelInfo.Pose = pose;
        modelInfo.ARGameObject = arObject;
       // modelInfo.Anchor = anchor;
        return modelInfo;
    }
    /// <summary>
    /// Adjests the ARM odel by device orientation.
    /// </summary>
    /// <param name="go">被调整的物体</param>
    /// <param name="deviceOfUserHead">If set to <c>true</c> use device orientation otherwise use user heading.</param>
    private void AdjestARModelByDeviceOrientation(GameObject go, bool deviceOfUserHead)
    {
        var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
        float orientation;
        if (deviceOfUserHead == true)
            orientation = locationProvider.CurrentLocation.DeviceOrientation;
        else
            orientation = locationProvider.CurrentLocation.UserHeading;

        float differentOfMapAndUserDegree;
        if (orientation >= 180)
            differentOfMapAndUserDegree = 360 - orientation + abstractMap.transform.rotation.eulerAngles.y;
        else
            differentOfMapAndUserDegree = 0 - orientation + abstractMap.transform.rotation.eulerAngles.y;
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

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
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
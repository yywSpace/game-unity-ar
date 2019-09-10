using System.Collections.Generic;
using Script.ARScene;
using UnityEngine;
using UnityEngine.UI;

namespace Script.PutModelScene
{
    public class BagController : MonoBehaviour
    {
        private static int modelId;
        /// <summary>
        /// 委托，用于向外界传递当前放置的模型及总模型列表
        /// </summary>
        public delegate void OnMyItemClick(GameObject gameObject, List<ArModelInfo> putArObjects);
        public OnMyItemClick onMyItemClick;
        private GameObject _firstPersonCamera;
        /// <summary>
        /// 当前放置模型的总信息
        /// </summary>
        private List<ArModelInfo> _putArObjects;
        void Awake()
        {
            _putArObjects = new List<ArModelInfo>();
            _firstPersonCamera = GameObject.Find("First Person Camera");
            // 方格数×方格大小 + （方格数+1）* 间距 = 背景大小
            InitBag();
        }

        void InitBag()
        {
            print("InitBag");
            CreateCell("Image/Capsule");
            CreateCell("Image/Cube");
            CreateCell("Image/Cylinder");
            CreateCell("Image/Sphere");
        }

        void CreateCell(int r, int c)
        {
            GameObject go = new GameObject(r.ToString() + ":" + c.ToString());
            go.AddComponent<Image>();
            go.transform.SetParent(transform, false);
        }
        /// <summary>
        /// 根据Resouse目录下的图片初始化背包中物品
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        void CreateCell(string imagePath)
        {
            GameObject go = new GameObject(imagePath);
            go.AddComponent<Image>();
            BagItem bagItem = go.AddComponent<BagItem>();
            bagItem.SetImage(imagePath);
            bagItem.SetMyPointerClick(() =>
            {
                Object model = Resources.Load("Model/" + imagePath.Split('/')[1]);
                print("bagItem click:" + gameObject);
                print("Model/" + imagePath.Split('/')[1]);
                print("Model:" + model);
                var modelInfo = ARController.CreateArModel(model, _firstPersonCamera.transform.position + new Vector3(0,-1,2), Quaternion.Euler(new Vector3()));

                // 脚本初始化都为false 当用户选择某个模型后添加对此模型的控制
                RotateAndUpDown raud =  modelInfo.ArGameObject.AddComponent<RotateAndUpDown>();
                raud.SetCamera(_firstPersonCamera.GetComponent<Camera>());
                raud.enabled = false;
                TransfromAroundAndDistance tad = modelInfo.ArGameObject.AddComponent<TransfromAroundAndDistance>();
                tad.SetCamera(_firstPersonCamera.GetComponent<Camera>());
                tad.enabled = false;
                DoubleClickChangeStatus dccs = modelInfo.ArGameObject.AddComponent<DoubleClickChangeStatus>();
                dccs.SetCamera(_firstPersonCamera.GetComponent<Camera>());
                dccs.enabled = false;

                // 设置模型附加的脚本信息
                modelInfo.RotateAndUpDown = raud;
                modelInfo.TransfromAroundAndDistance = tad;
                modelInfo.DoubleClickChangeStatus = dccs;

                modelInfo.ArGameObject.transform.RotateAround(_firstPersonCamera.transform.position, Vector3.up, _firstPersonCamera.transform.rotation.eulerAngles.y);
                // 更改名字作为每个物体不同的标识
                modelInfo.ArGameObject.name = $"{modelInfo.ArGameObject.name.Split('(')[0]}({modelId++})";
                //Object coordinate = ARUtils.LoadModel("Model/Coordinate");
                //var coordinateObject = Instantiate(coordinate, new Vector3(), Quaternion.Euler(new Vector3())) as GameObject;
                //// 开始时不启用，点击时启用
                //coordinateObject.SetActive(false);
                //// 更改名字作为每个物体不同的标识
                //coordinateObject.name = string.Format("{0}:{1}", coordinateObject.name.Split('(')[0], modelInfo.ARGameObject.name);
                //TransformWithCoordinateAxis tfwca = coordinateObject.GetComponent<TransformWithCoordinateAxis>();
                //tfwca.enabled = false;
                //TransfromModel tfm = coordinateObject.GetComponent<TransfromModel>();
                //tfm.SetCamera(FirstPersonCamera.GetComponent<Camera>());
                //tfm.SetFollowObject(modelInfo.ARGameObject); tfwca.SetCamera(FirstPersonCamera.GetComponent<Camera>());
                //tfwca.SetFollowObject(modelInfo.ARGameObject);
                //tfwca.SetCamera(FirstPersonCamera.GetComponent<Camera>());
                //tfwca.SetFollowObject(modelInfo.ARGameObject);
                //modelInfo.RelatedCoordinate = coordinateObject;
                _putArObjects.Add(modelInfo);
                onMyItemClick?.Invoke(modelInfo.ArGameObject, _putArObjects);
            });
            go.transform.SetParent(transform, false);
        }
        public void SetOnMyItemClick(OnMyItemClick clickEvend)
        {
            onMyItemClick = clickEvend;
        }

    }
}

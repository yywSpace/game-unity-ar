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
            CreateCell("Image/Cyan_Fish");
            CreateCell("Image/Bamboo");
            CreateCell("Image/Earth");
            CreateCell("Image/Aircraft");
            CreateCell("Image/Bird_man");
            CreateCell("Image/八重樱");
//            CreateCell("Image/Capsule");
//            CreateCell("Image/Cube");
//            CreateCell("Image/Cylinder");
//            CreateCell("Image/Sphere");
        }

        void CreateCell(int r, int c)
        {
            GameObject go = new GameObject(r + ":" + c);
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
                if (_putArObjects.Count >= 1)
                {
                    print("只可放置一个模型");
                    return;
                }
                Object model = Resources.Load("Model/" + imagePath.Split('/')[1]);
                print("bagItem click:" + gameObject);
                print("Model/" + imagePath.Split('/')[1]);
                print("Model:" + model);
                var modelInfo = ArController.CreateArModel(model, new Vector3(0,0,2), Quaternion.Euler(new Vector3()));
                Vector3 size =  ArUtils.GetObjectSizeByCollider(modelInfo.ArGameObject);
                modelInfo.ArGameObject.transform.position = new Vector3(0,-size.y/2,2*size.z);
                // 脚本初始化都为false 当用户选择某个模型后添加对此模型的控制
                RotateAndUpDown raud =  modelInfo.ArGameObject.AddComponent<RotateAndUpDown>();
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
             
                _putArObjects.Add(modelInfo);
                onMyItemClick?.Invoke(modelInfo.ArGameObject, _putArObjects);
            });
            go.transform.SetParent(transform, false);
        }
        public void SetOnMyItemClick(OnMyItemClick clickEvent)
        {
            onMyItemClick = clickEvent;
        }
     
    }
}

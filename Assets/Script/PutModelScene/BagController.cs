using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    public static int ModelId;
    /// <summary>
    /// 委托，用于向外界传递当前放置的模型及总模型列表
    /// </summary>
    public delegate void OnMyItemClick(GameObject gameObject, List<ARModelInfo> PutARObjects);
    public OnMyItemClick onMyItemClick;
    private GameObject FirstPersonCamera;
    /// <summary>
    /// 当前放置模型的总信息
    /// </summary>
    private List<ARModelInfo> PutARObjects;
    void Awake()
    {
        PutARObjects = new List<ARModelInfo>();
        FirstPersonCamera = GameObject.Find("First Person Camera");
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
            var modelInfo = ARController.CreateARModel(model, FirstPersonCamera.transform.position + Vector3.forward * 2, Quaternion.Euler(new Vector3())); 
            Object coordinate = ARUtils.LoadModel("Model/Coordinate");
            modelInfo.ARGameObject.AddComponent<RotateWithCoordinateAxis>().SetCamera(FirstPersonCamera.GetComponent<Camera>());
            modelInfo.ARGameObject.transform.RotateAround(FirstPersonCamera.transform.position, Vector3.up, FirstPersonCamera.transform.rotation.eulerAngles.y);
            // 更改名字作为每个物体不同的标识
            modelInfo.ARGameObject.name = string.Format("{0}({1})", modelInfo.ARGameObject.name.Split('(')[0], ModelId++);
            var coordinateObject = Instantiate(coordinate, new Vector3(), Quaternion.Euler(new Vector3())) as GameObject;
            // 开始时不启用，点击时启用
            coordinateObject.SetActive(false);
            // 更改名字作为每个物体不同的标识
            coordinateObject.name = string.Format("{0}:{1}", coordinateObject.name.Split('(')[0], modelInfo.ARGameObject.name);
            TransformWithCoordinateAxis tfwca = coordinateObject.GetComponent<TransformWithCoordinateAxis>();
            tfwca.enabled = false;
            TransfromModel tfm = coordinateObject.GetComponent<TransfromModel>();
            tfm.SetCamera(FirstPersonCamera.GetComponent<Camera>());
            tfm.SetFollowObject(modelInfo.ARGameObject); tfwca.SetCamera(FirstPersonCamera.GetComponent<Camera>());
            tfwca.SetFollowObject(modelInfo.ARGameObject);
            tfwca.SetCamera(FirstPersonCamera.GetComponent<Camera>());
            tfwca.SetFollowObject(modelInfo.ARGameObject);
            modelInfo.RelatedCoordinate = coordinateObject;
            PutARObjects.Add(modelInfo);
            onMyItemClick?.Invoke(modelInfo.ARGameObject, PutARObjects);
        });
        go.transform.SetParent(transform, false);
    }
    public void SetOnMyItemClick(OnMyItemClick clickEvend)
    {
        onMyItemClick = clickEvend;
    }

}

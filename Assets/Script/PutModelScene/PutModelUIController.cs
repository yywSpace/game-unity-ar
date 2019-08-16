using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 控制放置模型界面的UI元素
/// </summary>
public class PutModelUIController : MonoBehaviour
{
    // 当前放置的物体
    private GameObject CurrentPutARObjec;
    // 所有放到场景中的ar物体
    private List<ARModelInfo> PutARObjects;
    // 当前被选中的AR模型
    private GameObject SelectedARObject;

    public Button ShowItemsButton;
    public Button SetModelButton;
    public Button DeleteModelButton;
    public GameObject bag;
    public GameObject map;
    private AbstractMap abstractMap;
    public Camera FirstPersonCamera;
    private bool hasShowItems;
    private List<Task> TaskList;
    // Start is called before the first frame update
    void Start()
    {
        TaskList = TaskLab.get().GetTaskList();
        abstractMap = map.GetComponent<AbstractMap>();
        bag.GetComponent<BagController>().SetOnMyItemClick((GameObject currentAddGameObject, List<ARModelInfo> putARObjects) =>
        {
            PutARObjects = putARObjects;
            print(PutARObjects);
            CurrentPutARObjec = currentAddGameObject;
            bag.SetActive(false);
            hasShowItems = false;
        });

        SetModelButton.onClick.AddListener(() =>
        {
            if (SelectedARObject != null)
            {
                Task task = new Task();
                task.TaskDesc = "1 2 3 4";
                task.TaskLocation = abstractMap.WorldToGeoPosition(SelectedARObject.transform.position);
                task.TaskModelRotation = SelectedARObject.transform.rotation;
                task.TaskModelName = SelectedARObject.transform.name.Split('(')[0];
                TaskList.Add(task);
                GameObject.Find("Text").GetComponent<Text>().text += "\nTaskList.Count:" + TaskList.Count + "\nPutedGameObject position:" + SelectedARObject.transform.position + "\n" + "PutedGameObject Latlng:" + abstractMap.WorldToGeoPosition(SelectedARObject.transform.position);                print("PutedGameObject position:" + SelectedARObject.transform.position);
                print("PutedGameObject Latlng:" + abstractMap.WorldToGeoPosition(SelectedARObject.transform.position));
            }
        });

        ShowItemsButton.onClick.AddListener(() =>
        {
            if (hasShowItems)
                bag.SetActive(false);
            else
                bag.SetActive(true);
            hasShowItems = !hasShowItems;
        });

        // 找到当前选择的模型，删除它的所有信息
        DeleteModelButton.onClick.AddListener(() =>
        {
            ARModelInfo modelInfo = PutARObjects.Find((info) =>
            {
                return info.ARGameObject.name == SelectedARObject.name;
            });
            print(modelInfo.ARGameObject.name);

            if (modelInfo != null)
            {
                Destroy(modelInfo.ARGameObject);
                Destroy(modelInfo.Anchor);
                Destroy(modelInfo.RelatedCoordinate);
                PutARObjects.Remove(modelInfo);
            }
        });
    }
    /// <summary>
    /// 通过go获取当前所放置所有模型中与此模型相关的所有信息
    /// </summary>
    /// <returns>选择AR模型的所有信息.</returns>
    /// <param name="go">当前选中的模型</param>
    ARModelInfo GetSelectARMdel(GameObject go)
    {
        ARModelInfo modelInfo = PutARObjects.Find((info) =>
        {
            return info.ARGameObject.name == go.name;
        });
        return modelInfo;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        Ray ray = FirstPersonCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            // todo 选中时高亮

            if (Input.GetMouseButtonDown(0))
            {
                ARModelInfo modelInfo = GetSelectARMdel(hitInfo.collider.gameObject);
                if (modelInfo == null)
                    return;

                if (SelectedARObject == null)
                {
                    SelectedARObject = modelInfo.ARGameObject;
                    modelInfo.RelatedCoordinate.SetActive(true);
                    return;
                }

                // 选中相同物体
                if (SelectedARObject == hitInfo.collider.gameObject)
                {
                    print("相同物体");
                    return;
                }

                GetSelectARMdel(SelectedARObject).RelatedCoordinate.SetActive(false);
                SelectedARObject = modelInfo.ARGameObject;
                modelInfo.RelatedCoordinate.SetActive(true);
            }
        }
    }
}

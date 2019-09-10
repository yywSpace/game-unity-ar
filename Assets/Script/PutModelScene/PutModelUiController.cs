using System.Collections.Generic;
using Mapbox.Unity.Map;
using Script.ARScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.PutModelScene
{
    /// <summary>
    /// 控制放置模型界面的UI元素
    /// </summary>
    public class PutModelUiController : MonoBehaviour
    {
        // 当前放置的物体
        private GameObject CurrentPutARObjec;
        // 所有放到场景中的ar物体
        private List<ArModelInfo> _putArObjects;
        // 当前被选中的AR模型
        private GameObject _selectedArObject;

        public Button ShowItemsButton;
        public Button SetModelButton;
        public Button DeleteModelButton;
        public GameObject bag;
        public GameObject map;
        public Image controlStatus;

        private AbstractMap abstractMap;
        public Camera FirstPersonCamera;
        private bool hasShowItems;
        private List<Task> TaskList;
        // Start is called before the first frame update
        void Start()
        {
            controlStatus.GetComponent<LongPressEventTrigger>().onLongPress.AddListener(
                () => { print("长按"); }
            );
            TaskList = TaskLab.get().GetTaskList();
            abstractMap = map.GetComponent<AbstractMap>();
            bag.GetComponent<BagController>().SetOnMyItemClick((GameObject currentAddGameObject, List<ArModelInfo> putArObjects) =>
            {
                _putArObjects = putArObjects;
                print(_putArObjects);
                CurrentPutARObjec = currentAddGameObject;
                bag.SetActive(false);
                hasShowItems = false;
            });

            SetModelButton.onClick.AddListener(() =>
            {
                if (_selectedArObject == null) return;
                var position = _selectedArObject.transform.position;
                var task = new Task
                {
                    TaskDesc = "1 2 3 4",
                    TaskLocation = abstractMap.WorldToGeoPosition(position),
                    TaskModelRotation = _selectedArObject.transform.rotation,
                    TaskModelName = _selectedArObject.transform.name.Split('(')[0]
                };
                TaskList.Add(task);
                GameObject.Find("Text").GetComponent<Text>().text += "\nTaskList.Count:" + TaskList.Count + "\nPutedGameObject position:" + position + "\n" + "PutedGameObject Latlng:" + abstractMap.WorldToGeoPosition(position);                print("PutedGameObject position:" + position);
                print("PutedGameObject Latlng:" + abstractMap.WorldToGeoPosition(position));
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
                ArModelInfo modelInfo = _putArObjects.Find((info) => info.ArGameObject.name == _selectedArObject.name);


                if (modelInfo == null) return;
                print(modelInfo.ArGameObject.name);
                Destroy(modelInfo.ArGameObject);
                Destroy(modelInfo.Anchor);
                _putArObjects.Remove(modelInfo);
            });
        }
        /// <summary>
        /// 通过go获取当前所放置所有模型中与此模型相关的所有信息
        /// </summary>
        /// <returns>选择AR模型的所有信息.</returns>
        /// <param name="go">当前选中的模型</param>
        ArModelInfo GetSelectArModel(GameObject go)
        {
            ArModelInfo modelInfo = _putArObjects.Find((info) =>
            {
                return info.ArGameObject.name == go.name;
            });
            return modelInfo;
        }
        void Update()
        {
            // 当选择的物体为空，或者已经被删除后置为灰色
            if (_selectedArObject == null)
            {
                controlStatus.color = Color.gray;
            }
            else if (GetSelectArModel(_selectedArObject).DoubleClickChangeStatus.GetStatus() == 1)
            {
                // TransformWithCoordinateAxis 被激活
                controlStatus.color = Color.red;
            }
            else if (GetSelectArModel(_selectedArObject).DoubleClickChangeStatus.GetStatus() == -1)
            {
                // RotateAndUpDown被激活
                controlStatus.color = Color.green;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

            Ray ray = FirstPersonCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                // todo 选中时高亮

                if (Input.GetMouseButtonDown(0))
                {
                    ArModelInfo modelInfo = GetSelectArModel(hitInfo.collider.gameObject);
                    if (modelInfo == null)
                        return;

                    if (_selectedArObject == null)
                    {
                        _selectedArObject = modelInfo.ArGameObject;
                        modelInfo.TransfromAroundAndDistance.enabled = true;
                        modelInfo.RotateAndUpDown.enabled = false;
                        modelInfo.DoubleClickChangeStatus.enabled = true;
                        return;
                    }

                    // 选中相同物体
                    if (_selectedArObject == hitInfo.collider.gameObject)
                    {
                        print("相同物体");
                        return;
                    }

                    // disable 前一选中模型
                    GetSelectArModel(_selectedArObject).TransfromAroundAndDistance.enabled = false;
                    GetSelectArModel(_selectedArObject).RotateAndUpDown.enabled = false;
                    GetSelectArModel(_selectedArObject).DoubleClickChangeStatus.enabled = false;

                    _selectedArObject = modelInfo.ArGameObject;
                    // 激活当前选择模型
                    modelInfo.TransfromAroundAndDistance.enabled = true;
                    modelInfo.RotateAndUpDown.enabled = false;
                    modelInfo.DoubleClickChangeStatus.enabled = true;
                }
            }
        }
    }
}

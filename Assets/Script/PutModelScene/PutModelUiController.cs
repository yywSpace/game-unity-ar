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
        // 所有放到场景中的ar物体
        private List<ArModelInfo> _putArObjects;
        // 当前被选中的AR模型
        private GameObject _selectedArObject;

        public Button showItemsButton;
        public Button setModelButton;
        public Button deleteModelButton;
        public GameObject bag;
        public GameObject map;
        public GameObject controlStatus;
        public GameObject controlDetail;
        
        private Image _controlStatusImage;
        private Text[] _controlStatusDetailTexts;
        
        private AbstractMap _abstractMap;
        public Camera firstPersonCamera;
        private bool _hasShowItems;
        private List<Task> _taskList;
        // Start is called before the first frame update
        void Start()
        {
            controlDetail.SetActive(false);
            _controlStatusImage = controlStatus.GetComponent<Image>();
            _controlStatusDetailTexts = controlDetail.GetComponentsInChildren<Text>();
            controlStatus.GetComponent<LongPressEventTrigger>().onLongPress.AddListener(
                () =>
                {
                    print("长按"); 
                    controlDetail.SetActive(!controlDetail.activeSelf);
                }
                
            );
            _taskList = TaskLab.Get().GetTaskList();
            _abstractMap = map.GetComponent<AbstractMap>();
            bag.GetComponent<BagController>().SetOnMyItemClick((GameObject currentAddGameObject, List<ArModelInfo> putArObjects) =>
            {
                _putArObjects = putArObjects;
                print(_putArObjects);
               // _selectedArObject = currentAddGameObject;
                bag.SetActive(false);
                _hasShowItems = false;
            });

            setModelButton.onClick.AddListener(() =>
            {
                if (_selectedArObject == null) return;
                var position = _selectedArObject.transform.position;
                var task = new Task
                {
                    TaskDesc = _selectedArObject.name + " 2 3 4",
                    TaskLocation = _abstractMap.WorldToGeoPosition(position),
                    TaskModelRotation = _selectedArObject.transform.rotation,
                    TaskModelName = _selectedArObject.transform.name.Split('(')[0]
                };
                _taskList.Add(task);
                GameObject.Find("Text").GetComponent<Text>().text += "\nTaskList.Count:" + _taskList.Count + "\nPutedGameObject position:" + position + "\n" + "PutedGameObject Latlng:" + _abstractMap.WorldToGeoPosition(position);                print("PutedGameObject position:" + position);
                print("PutedGameObject Latlng:" + _abstractMap.WorldToGeoPosition(position));
            });

            showItemsButton.onClick.AddListener(() =>
            {
                if (_hasShowItems)
                    bag.SetActive(false);
                else
                    bag.SetActive(true);
                _hasShowItems = !_hasShowItems;
            });

            // 找到当前选择的模型，删除它的所有信息
            deleteModelButton.onClick.AddListener(() =>
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
            // 如果控制状态详细信息面板已经显示，在点击其他地方后让其消失
            if (Input.GetMouseButtonDown(0) && controlDetail.activeSelf)
            {
                controlDetail.SetActive(false);
            }
            
            // 当选择的物体为空，或者已经被删除后置为灰色
            if (_selectedArObject == null)
            {
                _controlStatusImage.color = Color.gray;
                _controlStatusDetailTexts[0].text = "未选中";
                // status detail
                _controlStatusDetailTexts[1].text = "点击模型：选中当前模型\n双击模型：改变控制状态";
            }
            else if (GetSelectArModel(_selectedArObject).DoubleClickChangeStatus.GetStatus() == 1)
            {
                // TransfromAroundAndDistance 被激活
                _controlStatusImage.color = Color.red;
                // 1->0, 2->1, 3->2
                // model name
                _controlStatusDetailTexts[0].text = "已选中";
                // status detail
                _controlStatusDetailTexts[1].text = "上下滑动：控制模型远近\n左右滑动：模型围绕移动";
            }
            else if (GetSelectArModel(_selectedArObject).DoubleClickChangeStatus.GetStatus() == -1)
            {
                // RotateAndUpDown被激活
                _controlStatusImage.color = Color.green;
                // model name
                _controlStatusDetailTexts[0].text = "已选中";
                // status detail
                _controlStatusDetailTexts[1].text = "上下滑动：控制模型高低\n左右滑动：控制模型旋转";
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

            Ray ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);
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
                        Renderer _renderer1 = _selectedArObject.GetComponent<MeshRenderer>();
                        _renderer1.material.shader = Shader.Find("CM/RimLight");
                        _renderer1.material.SetColor("_RimColor",Color.green);
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
                    _selectedArObject.GetComponent<MeshRenderer>().material.SetColor("_RimColor",Color.white);
                    
                    _selectedArObject = modelInfo.ArGameObject;
                    // 激活当前选择模型
                    modelInfo.TransfromAroundAndDistance.enabled = true;
                    modelInfo.RotateAndUpDown.enabled = false;
                    modelInfo.DoubleClickChangeStatus.enabled = true;
                    Renderer _renderer = _selectedArObject.GetComponent<MeshRenderer>();
                    _renderer.material.shader = Shader.Find("CM/RimLight");
                    _renderer.material.SetColor("_RimColor",Color.green);
                    print("render over");
                }
            }
        }
    }
}

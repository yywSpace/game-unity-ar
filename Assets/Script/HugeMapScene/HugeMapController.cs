using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.HugeMapScene
{
    public class HugeMapController : MonoBehaviour
    {
        private bool _hasLoadModel;
        private List<Task> _taskList;
        private AbstractMap _abstractMap;
        private Object _model;
        public GameObject map;
        public Text text;
        void Start()
        {
            _abstractMap = map.GetComponent<AbstractMap>();
        }
        // Update is called once per frame
        void Update()
        {
            // 返回主页面
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainPage");
            }
        
            Vector3 position = _abstractMap.GeoToWorldPosition(TaskLab.Get().GetTaskList()[0].TaskLocation);
            if (position != Vector3.zero && !_hasLoadModel)
            {
                var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                TaskLab.Get().SetCurrentLatlng(locationProvider.CurrentLocation.LatitudeLongitude);
                _taskList = TaskLab.Get().GetTaskListIn(200);
                //TaskList = TaskLab.get().GetTaskList();
                text.text = "abstractMap: " + _abstractMap + "\n";
                foreach (var task in _taskList)
                {
                    _model = ArUtils.LoadModel("Model/" + task.TaskModelName);
                    position = _abstractMap.GeoToWorldPosition(task.TaskLocation);
                    var message = text.text;
                    message += "name:" + task.TaskModelName + "\n";
                    message += "location:" + task.TaskLocation + "\n";
                    message += "position:" + position + "\n";
                    text.text = message;
                    Instantiate(_model, position, task.TaskModelRotation);
                }
                _hasLoadModel = true;
            }
        }
    }
}

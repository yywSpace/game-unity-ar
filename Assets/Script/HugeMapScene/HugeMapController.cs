using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HugeMapController : MonoBehaviour
{
    private bool hasLoadModel;
    private List<Task> TaskList;
    private AbstractMap abstractMap;
    public GameObject map;
    public Text text;
    Object model;
    void Start()
    {
        abstractMap = map.GetComponent<AbstractMap>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 position = abstractMap.GeoToWorldPosition(TaskLab.get().GetTaskList()[0].TaskLocation);
        if (position != Vector3.zero && !hasLoadModel)
        {
            var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            TaskLab.get().SetCurrentLatlng(locationProvider.CurrentLocation.LatitudeLongitude);
            TaskList = TaskLab.get().GetTaskListIn(200);
            //TaskList = TaskLab.get().GetTaskList();
            print(position);
            text.text = "abstractMap: " + abstractMap + "\n";
            foreach (var task in TaskList)
            {
                model = ARUtils.LoadModel("Model/" + task.TaskModelName);
                position = abstractMap.GeoToWorldPosition(task.TaskLocation);
                text.text += "name:" + task.TaskModelName + "\n";
                text.text += "location:" + task.TaskLocation + "\n";
                text.text += "position:" + position + "\n";
                //print(position);
                Instantiate(model, position, task.TaskModelRotation);
            }
            hasLoadModel = true;
        }
     
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
            print("LoadScene ContorlModel");
        }
    }
}

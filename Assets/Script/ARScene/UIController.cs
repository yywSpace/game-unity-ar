using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 控制ARScene的界面元素
/// </summary>
public class UIController : MonoBehaviour
{
    private AbstractMap abstractMap;
    public Button PutModelButton;
    public Button ArrayToggerButton;

    public Text MessageText;
    public GameObject map;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        abstractMap = map.GetComponent<AbstractMap>();
        PutModelButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ContorlModel");
            print("LoadScene");
        });

        ArrayToggerButton.onClick.AddListener(() =>
        {
            GameObject arController = GameObject.Find("ARController");
            List<ARModelInfo> modelList = arController.GetComponent<ARController>().GetShowedARModels();
            foreach (var model in modelList)
            {
                ModelIndicator mi = model.ARGameObject.GetComponent<ModelIndicator>();
                mi.enabled = !mi.enabled;
                print(mi.enabled);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
        // 根据地图将经纬度转化为当前世界坐标
        Vector3 worldPosition = abstractMap.GeoToWorldPosition(locationProvider.CurrentLocation.LatitudeLongitude);
        // 根据地图将当前世界坐标转化为经纬度
        // abstractMap.WorldToGeoPosition();

        MessageText.text = string.Format(
            "Position:{0}\n" +
            "Rotation:{1}\nL" +
            "ocationProvider:{2}\n" +
            "Heading:{3}\n" +
            "Latlng:{4}\n" +
            "LatlngToWorldPosition:{5}",
            playerTransform.position, playerTransform.rotation.eulerAngles, locationProvider.GetType(),
            locationProvider.CurrentLocation.UserHeading + ":" + locationProvider.CurrentLocation.DeviceOrientation, locationProvider.CurrentLocation.LatitudeLongitude.x + "," + locationProvider.CurrentLocation.LatitudeLongitude.y,
            worldPosition - abstractMap.transform.position);
    }
}

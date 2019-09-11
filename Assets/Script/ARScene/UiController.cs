using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.ARScene
{
    /// <summary>
    /// 控制ARScene的界面元素
    /// </summary>
    public class UiController : MonoBehaviour
    {
        private AbstractMap _abstractMap;
        public Button putModelButton;
        public Button arrayToggleButton;

        public Text messageText;
        public GameObject map;
        public Transform playerTransform;
        // Start is called before the first frame update
        void Start()
        {
            _abstractMap = map.GetComponent<AbstractMap>();
            putModelButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("ControlModel");
                print("LoadScene");
            });

            arrayToggleButton.onClick.AddListener(() =>
            {
                GameObject arController = GameObject.Find("ARController");
                List<ArModelInfo> modelList = arController.GetComponent<ArController>().GetShowedArModels();
                foreach (var model in modelList)
                {
                    ModelIndicator mi = model.ArGameObject.GetComponent<ModelIndicator>();
                    mi.enabled = !mi.enabled;
                }
            });
        }

        // Update is called once per frame
        void Update()
        {
            var locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            // 根据地图将经纬度转化为当前世界坐标
            Vector3 worldPosition = _abstractMap.GeoToWorldPosition(locationProvider.CurrentLocation.LatitudeLongitude);
            // 根据地图将当前世界坐标转化为经纬度
            // abstractMap.WorldToGeoPosition();

            messageText.text = $"Position:{playerTransform.position}\n" +
                               $"Rotation:{playerTransform.rotation.eulerAngles}\nL" +
                               $"locationProvider:{locationProvider.GetType()}\n" +
                               $"Heading:{locationProvider.CurrentLocation.UserHeading + ":" + locationProvider.CurrentLocation.DeviceOrientation}\n" +
                               $"Latlng:{locationProvider.CurrentLocation.LatitudeLongitude.x + "," + locationProvider.CurrentLocation.LatitudeLongitude.y}\n" +
                               $"LatlngToWorldPosition:{worldPosition - _abstractMap.transform.position}";
        }
    }
}

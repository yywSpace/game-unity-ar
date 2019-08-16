using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
     
    }
    // Update is called once per frame
    void Update()
    {
        // 始终位于object上方一定距离
        transform.position = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + 100, targetObject.transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Utilities;
using UnityEngine;

/// <summary>
/// 当切换场景后所保留的物体
/// </summary>
public class DontDestroyObjectList : MonoBehaviour
{
    public GameObject[] DontDestroyList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in DontDestroyList)
        {
            if (GameObject.Find("LocationBasedGame") != null)
            {
                DontDestroyOnLoad(item);
            }
        }
    }
}

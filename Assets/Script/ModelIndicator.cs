using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelIndicator : MonoBehaviour
{

    private GameObject Array;
    public Camera CameraObject;
    private Text arrayText;
    void OnEnable()
    {
        if (Array != null)
        {
            Array.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Object obj = ARUtils.LoadModel("Model/Array");
        Array = Instantiate(obj, GameObject.Find("Canvas").transform) as GameObject;
        Array.transform.localScale = Vector3.one * .5f;
        arrayText = Array.GetComponentInChildren<Text>();
    }
    void OnDisable()
    {
        Array.SetActive(false);
    }
    public void OnBecameVisible()
    {
        Array.SetActive(false);
    }


    public void OnBecameInvisible()
    {
        Array.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Array.activeSelf)
            return;

        arrayText.text = (transform.position - CameraObject.transform.position).magnitude + "";
        // 从当前物体指向其他物体
        Vector3 indicatorPoint = (transform.position - CameraObject.transform.position).normalized + CameraObject.transform.position;
        print(CameraObject);
        Vector3 screenPoint = CameraObject.WorldToScreenPoint(indicatorPoint);
        float angle = Vector3.Angle(Vector3.forward, indicatorPoint);
        Vector3 cross = Vector3.Cross(Vector3.forward, indicatorPoint);
        Debug.DrawLine(CameraObject.transform.position, CameraObject.transform.position - transform.position);
        // 判断物体在相机的前方还是后方
        float dotAngle = Vector3.Dot(CameraObject.transform.forward, CameraObject.transform.position - transform.position);
        print(dotAngle);
        //print("cross: " + cross);
        Array.transform.rotation = Quaternion.Euler(new Vector3());
        arrayText.transform.rotation = Quaternion.Euler(new Vector3());
        //print("angle:" + angle);
        if (cross.y < 0)
        {
            Array.transform.Rotate(new Vector3(0, 0, angle));
            arrayText.transform.Rotate(new Vector3(0, 0, -angle));
        }
        else
        {
            Array.transform.Rotate(new Vector3(0, 0, -angle));
            arrayText.transform.Rotate(new Vector3(0, 0, -angle));
        }

        Array.transform.position = screenPoint;
        Vector3 arrayViewPoint = CameraObject.ScreenToViewportPoint(Array.transform.position);
        arrayViewPoint.y = arrayViewPoint.z;
        //arrayViewPoint.y = 0.5f;

        if (arrayViewPoint.x < 0)
            arrayViewPoint.x = 0;
        if (arrayViewPoint.y < 0)
            arrayViewPoint.y = 0;
        if (arrayViewPoint.x > 1)
            arrayViewPoint.x = 1;
        if (arrayViewPoint.y > 1)
            arrayViewPoint.y = 1;
        // 物体在相机后方
        if (dotAngle > 0)
            arrayViewPoint.x = 1 - arrayViewPoint.x;

        Array.transform.position = CameraObject.ViewportToScreenPoint(arrayViewPoint);
    }
    public void SetCameraObject(Camera camera)
    {
        CameraObject = camera;
    }

}

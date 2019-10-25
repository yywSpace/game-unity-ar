using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class TransfromModel : MonoBehaviour
{
    private string CurrentAxis;
    private bool isClick;
    private Vector3 followObjectOffset;
    public GameObject FollowObject;
    public Camera cam;
    private float curentCameraDegree;
    private int moveSpeed = 200;// 越大越慢
    Text msgText;

    // Start is called before the first frame update
    void Start()
    {
        msgText = GameObject.Find("Text").GetComponent<Text>();
        Vector3 objectSize = ArUtils.GetObjectSizeByMeshFilter(FollowObject);
        followObjectOffset = new Vector3(0, objectSize.y / 2 + .1f, 0);
        transform.position = FollowObject.transform.position + followObjectOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo) && Input.GetMouseButtonDown(0))
        {
            if (hitInfo.transform.name == "x" || hitInfo.transform.name == "y" || hitInfo.transform.name == "z")
            {
                CurrentAxis = hitInfo.transform.name;
            }
        }
        SwipeInfo swipeInfo = SwipeInfo.GetSwipeInfo(ref isClick);
        if (swipeInfo == null)
        {
            return;
        }
        TransfromWithCameraDegree(swipeInfo, CurrentAxis);
        FollowObject.transform.position = transform.position - followObjectOffset;

    }

    public void SetCamera(Camera camera)
    {
        cam = camera;
        curentCameraDegree = cam.transform.eulerAngles.y;
    }

    public void SetFollowObject(GameObject gameObject)
    {
        FollowObject = gameObject;
    }

    void TransfromWithCameraDegree(SwipeInfo swipeInfo, string currentAxis)
    {
        float zRotation = cam.transform.rotation.eulerAngles.y;
        float rotation = zRotation - curentCameraDegree;
        if (rotation < 0)
            rotation += 360;
        print(rotation);
        //  z轴指向相机前方
        if ((rotation <= 45 && rotation >= 0) || rotation >= 315)
        {
            switch (currentAxis)
            {
                case "x":
                    if (swipeInfo.SwipAxisDirection == Vector3.right)
                    {
                        transform.position = new Vector3(transform.position.x + swipeInfo.RightDistance / moveSpeed, transform.position.y, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.left)
                    {
                        transform.position = new Vector3(transform.position.x + swipeInfo.LeftDistance / moveSpeed, transform.position.y, transform.position.z);
                    }

                    break;
                case "y":
                    if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.UpDistance / moveSpeed, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.DownDistance / moveSpeed, transform.position.z);
                    }
                    break;
                case "z":
                    if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.UpDistance / moveSpeed);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.DownDistance / moveSpeed);
                    }
                    break;
            }
            msgText.text = rotation + " :z轴指向相机前方:" + CurrentAxis;
        }
        // z轴指向相机左侧，
        else if (rotation > 45 && rotation <= 135)
        {
            switch (currentAxis)
            {
                case "x":
                    if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x + swipeInfo.DownDistance / moveSpeed, transform.position.y, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x + swipeInfo.UpDistance / moveSpeed, transform.position.y, transform.position.z);
                    }

                    break;
                case "y":
                    if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.UpDistance / moveSpeed, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.DownDistance / moveSpeed, transform.position.z);
                    }
                    break;
                case "z":
                    if (swipeInfo.SwipAxisDirection == Vector3.right)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - swipeInfo.RightDistance / moveSpeed);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.left)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - swipeInfo.LeftDistance / moveSpeed);
                    }
                    break;
            }
            msgText.text = rotation + " :z轴指向相机左侧:" + CurrentAxis;
        }
        // z轴指向相机后方
        else if (rotation > 135 && rotation <= 225)
        {
            switch (currentAxis)
            {
                case "x":
                    if (swipeInfo.SwipAxisDirection == Vector3.left)
                    {
                        transform.position = new Vector3(transform.position.x + swipeInfo.RightDistance / moveSpeed, transform.position.y, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.right)
                    {
                        transform.position = new Vector3(transform.position.x + swipeInfo.LeftDistance / moveSpeed, transform.position.y, transform.position.z);
                    }

                    break;
                case "y":
                    if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.UpDistance / moveSpeed, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.DownDistance / moveSpeed, transform.position.z);
                    }
                    break;
                case "z":
                    if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.DownDistance / moveSpeed);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.UpDistance / moveSpeed);
                    }
                    break;
            }
            msgText.text = rotation + " :z轴指向相机后方:" + CurrentAxis;
        }
        // z轴指向相机右侧
        else if (rotation > 225 && rotation < 315)
        {
            switch (currentAxis)
            {
                case "x":
                    if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x - swipeInfo.DownDistance / moveSpeed, transform.position.y, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x - swipeInfo.UpDistance / moveSpeed, transform.position.y, transform.position.z);
                    }

                    break;
                case "y":
                    if (swipeInfo.SwipAxisDirection == Vector3.up)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.UpDistance / moveSpeed, transform.position.z);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.down)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + swipeInfo.DownDistance / moveSpeed, transform.position.z);
                    }
                    break;
                case "z":
                    if (swipeInfo.SwipAxisDirection == Vector3.right)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.RightDistance / moveSpeed);
                    }
                    else if (swipeInfo.SwipAxisDirection == Vector3.left)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.LeftDistance / moveSpeed);
                    }
                    break;
            }
            msgText.text = rotation + " :z轴指向相机右侧:" + CurrentAxis;
        }
    }
}

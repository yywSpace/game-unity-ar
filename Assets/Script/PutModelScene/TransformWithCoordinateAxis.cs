using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://blog.csdn.net/weixin_41071087/article/details/79237849#
//https://www.cnblogs.com/gaoxu-1994/p/6600512.html

/// <summary>
/// 根据所给坐标轴模型移动物体
/// </summary>
public class TransformWithCoordinateAxis : MonoBehaviour
{
    public Camera cam;
    public GameObject FollowObject;
    private Vector3 followObjectOffset;
    private string axis;
    private Vector3 offset;
    private bool isDrage;
    private bool isClickZ;
    private string clickObjName;

    private Vector3 startPos;
    private Vector3 endPos;
    private int zTransformSpeed = 60;
    void Start()
    {
        Vector3 objectSize = ARUtils.GetObjectSize(FollowObject);
        followObjectOffset = new Vector3(0, objectSize.y / 2 + .1f, 0);
        transform.position = FollowObject.transform.position + followObjectOffset;
    }
    void Update()
    {
        if (cam == null)
            return;
        if (isDrage == false)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.parent == null)
                    return;
                clickObjName = hitInfo.transform.parent.name;
                axis = hitInfo.transform.name;
                offset = transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            }
        }
        if (Input.GetMouseButton(0))
        {
            isDrage = true;
            print(clickObjName + ":" + gameObject.name + "=" + clickObjName != transform.name);
            // 通过名字区分是否为当前物体
            if (clickObjName != transform.name)
                return;
            TransformWith(axis);
        }
        else
        {
            axis = "";
            isDrage = false;
        }

        FollowObject.transform.position = transform.position - followObjectOffset;
    }

    public void SetCamera(Camera camera)
    {
        cam = camera;
    }

    public void SetFollowObject(GameObject gameObject)
    {
        FollowObject = gameObject;
    }

    void TransformWith(string axis)
    {
        Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        switch (axis)
        {
            case "x":
                transform.position = new Vector3(worldPosition.x, transform.position.y, transform.position.z) + new Vector3(offset.x, 0, 0);
                break;
            case "y":
                transform.position = new Vector3(transform.position.x, worldPosition.y, transform.position.z) + new Vector3(0, offset.y, 0);
                break;
            case "z":
                ZTransformInScreen();
                break;
        }
        //print(axis);
    }

    // 在屏幕坐标下进行z方向的移动
    void ZTransformInScreen()
    {
        SwipeInfo swipeInfo = SwipeInfo.GetSwipeInfo(ref isClickZ);
        if (swipeInfo == null)
            return;
        // 特殊处理
        // 模型xyz均与世界坐标指向相同
        // 1. 获取当前相机z轴与世界坐标z轴夹角(rotation.y)，从而确定模型z轴相对于自身指向
        // 2. 获取手指滑动的方向及距离
        // 3. 根据1,2获取的数据进行平移
        Vector3 angles = cam.transform.eulerAngles;
        //print("angles:" + angles);
        float zRotation = angles.y % 360;
        // 模型不动，相机顺时针旋转
        // z轴指向相机前方
        if ((zRotation <= 45 && zRotation >= 0) || zRotation >= 315
         || (zRotation <= 0 && zRotation >= -45) || zRotation <= -315)
        {
            //print("z轴指向相机前方");
            if (swipeInfo.UpDistance > 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.UpDistance / zTransformSpeed);
            else if (swipeInfo.DownDistance < 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.DownDistance / zTransformSpeed);
        }
        // z轴指向相机左侧，
        else if ((zRotation > 45 && zRotation <= 135) || (zRotation > -315 && zRotation <= -225))
        {
            // print("z轴指向相机左侧");
            if (swipeInfo.RightDistance > 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.RightDistance / zTransformSpeed);
            else if (swipeInfo.LeftDistance < 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + swipeInfo.LeftDistance / zTransformSpeed);

        }
        // z轴指向相机后方
        else if ((zRotation > 135 && zRotation <= 225) || (zRotation > -225 && zRotation <= -135))
        {
            //print("z轴指向相机后方");
            if (swipeInfo.UpDistance > 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - swipeInfo.UpDistance / zTransformSpeed);
            }
            else if (swipeInfo.DownDistance < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - swipeInfo.DownDistance / zTransformSpeed);
            }
        }
        // z轴指向相机右侧
        else if ((zRotation > 225 && zRotation < 315) || (zRotation > -135 && zRotation <= -45))
        {
            //print("z轴指向相机右侧");
            if (swipeInfo.RightDistance > 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - swipeInfo.RightDistance / zTransformSpeed);
            else if (swipeInfo.LeftDistance < 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - swipeInfo.LeftDistance / zTransformSpeed);
        }
    }

}

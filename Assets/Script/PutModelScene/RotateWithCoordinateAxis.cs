using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 左右滑动旋转物体(当前，旋转轴多的话太乱，不好控制)
/// </summary>
public class RotateWithCoordinateAxis : MonoBehaviour
{
    private bool isClick = false;
    public Camera cam;

    public void SetCamera(Camera camera)
    {
        cam = camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            print("Rotated object: " + transform.name);
            if (hitInfo.transform.name == transform.name)
            {
                SwipeInfo swipeInfo = SwipeInfo.GetSwipeInfo(ref isClick);
                if (swipeInfo == null)
                    return;
                if (swipeInfo.SwipAxisDirection == Vector3.right)
                {
                    transform.Rotate(transform.up, -swipeInfo.Direction.magnitude / 5, Space.World);
                    //print("向右滑动");
                }
                else if (swipeInfo.SwipAxisDirection == Vector3.left)
                {
                    transform.Rotate(transform.up, swipeInfo.Direction.magnitude / 5, Space.World);
                    //print("向左滑动");
                }
            }
        }
        // 当鼠标离开模型后，设置点击模型为false
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
        }
    }
}

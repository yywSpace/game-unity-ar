using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠标滑动信息
/// </summary>
public class SwipeInfo
{
    public static Vector3 startPos;
    public static Vector3 endPos;

    public Vector3 StartPos { get; set; }
    public Vector3 EndPos { get; set; }
    public Vector3 Direction { get; set; }
    /// <summary>
    /// 1：上， -1：下， 2：右， -2:左
    /// </summary>
    /// <value>The swip axis direction.</value>
    public Vector3 SwipAxisDirection { get; set; }

    public float Magnitude { get; set; }

    public float UpDistance { get; set; }
    public float DownDistance { get; set; }
    public float RightDistance { get; set; }
    public float LeftDistance { get; set; }

    public static SwipeInfo GetSwipeInfo(ref bool isClick)
    {
        SwipeInfo swipeInfo = new SwipeInfo();
        if (Input.GetMouseButtonDown(0))
        {
            isClick = true;
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            isClick = false;

        if (!isClick)
            return null;
        endPos = Input.mousePosition;
        Vector3 v = endPos - startPos;
        if (v.y > 0) // 上
            swipeInfo.UpDistance = v.y;
        else
            swipeInfo.DownDistance = v.y;

        if (v.x > 0) // 右
            swipeInfo.RightDistance = v.x;
        else
            swipeInfo.LeftDistance = v.x;

        if (Mathf.Abs(v.y) > Mathf.Abs(v.x)) // 上下 
        {
            if (v.y > 0) // 上
                swipeInfo.SwipAxisDirection = Vector3.up;
            else // 下
                swipeInfo.SwipAxisDirection = Vector3.down;
        }
        else // 左右
        {
            if (v.x > 0) // 右
                swipeInfo.SwipAxisDirection = Vector3.right;
            else // 左
                swipeInfo.SwipAxisDirection = Vector3.left;
        }

        swipeInfo.StartPos = startPos;
        swipeInfo.EndPos = endPos;
        swipeInfo.Magnitude = v.magnitude;
        swipeInfo.Direction = v;
        startPos = endPos;
        return swipeInfo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制模型绕人旋转，及距离远近
/// </summary>
public class TransfromAroundAndDistance : MonoBehaviour
{
     public Camera cam;
    float currentAngle;

    public void SetCamera(Camera camera)
    {
        cam = camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
            return;

        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                print("touch");
                // 手指滑动时，要触发的代码 
                float s01 = Input.GetAxis("Mouse X");    //手指水平移动的距离
                float s02 = Input.GetAxis("Mouse Y");    //手指垂直移动的距离
                if (Mathf.Abs(s01) < 0.1 && Mathf.Abs(s02) < 0.1)
                    return;
          
                if (Mathf.Abs(s01) > Mathf.Abs(s02))
                {
                    // 水平
                    transform.RotateAround(cam.transform.position, Vector3.up , -s01);
                    currentAngle += -s01;

                }
                else
                {
                    Vector3 newV = V3RotateAround(new Vector3(0, 0, -s02 / 50), Vector3.up, currentAngle);
                    transform.Translate(newV, Space.World);
                    //竖直
                }
            }

        }
    }

    /// <summary>
    /// 计算一个Vector3绕旋转中心旋转指定角度后所得到的向量。
    /// </summary>
    /// <param name="source">旋转前的源Vector3</param>
    /// <param name="axis">旋转轴</param>
    /// <param name="angle">旋转角度</param>
    /// <returns>旋转后得到的新Vector3</returns>
    public Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);// 旋转系数
        return q * source;// 返回目标点
    }
 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Double cick change status.
/// 双击改变模型的操作状态
/// </summary>
public class DoubleCickChangeStatus : MonoBehaviour
{
    public Camera cam;
    float scale = .2f;
    int status = 1;
    private double lastKickTime; // 上一次鼠标抬起的时间（用来处理双击）
    void Awark()
    {
        lastKickTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            // 如果点击物体
            if (Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
            {
                // 双击
                if (Time.realtimeSinceStartup - lastKickTime < scale)//检测上次点击的时间和当前时间差 在一定范围内认为是双击
                {
                    status = -status;
                    gameObject.GetComponent<RotateAndUpDown>().enabled = !gameObject.GetComponent<RotateAndUpDown>().enabled;
                    gameObject.GetComponent<TransfromAroundAndDistance>().enabled = !gameObject.GetComponent<TransfromAroundAndDistance>().enabled;
                }
                lastKickTime = Time.realtimeSinceStartup;//重新设置上次点击的时间
            }
        }
    }

    public void SetCamera(Camera camera)
    {
        cam = camera;
    }

    /// <summary>
    /// Gets model status.
    /// 1：TransfromAroundAndDistance被激活, -1：RotateAndUpDown被激活
    /// </summary>
    /// <returns>The status.</returns>
    public int GetStatus()
    {
        return status;
    }
}

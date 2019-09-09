using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {

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
                if (Mathf.Abs(s01) > Mathf.Abs(s02))
                {
                    // 水平
                    transform.RotateAround(cam.transform.position, Vector3.up, s01 / 40);
                }
                else
                {
                     transform.Translate(new Vector3(0, s02/40, 0), Space.World);
                    //竖直
                }
            }

        }
    }
}

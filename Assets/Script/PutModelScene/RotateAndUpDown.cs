using UnityEngine;

namespace Script.PutModelScene
{
    /// <summary>
    /// 左右滑动旋转物体, 上下滑动控制模型上下距离
    /// </summary>
    public class RotateAndUpDown : MonoBehaviour
    {
        private bool _isClick = false;
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
                        transform.Rotate(transform.up, -s01, Space.World); // 水平
                    else
                        transform.Translate(new Vector3(0, -s02 / 50, 0), Space.World); //竖直

                }
                SwipeInfo swipeInfo = SwipeInfo.GetSwipeInfo(ref _isClick);
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
    }
}

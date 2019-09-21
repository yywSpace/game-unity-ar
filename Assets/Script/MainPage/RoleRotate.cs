using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Script.MainPage
{
    public class RoleRotate : MonoBehaviour
    {
        /// <summary>
        /// 手指滑动时的委托
        /// 前一参数表示x方向滑动距离
        /// 后一参数标识y方向滑动距离
        /// </summary>
        public OnSlideEvent onSlide = new OnSlideEvent();

        public Camera cam;

        private void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
                if (gameObject.transform.name != hitInfo.transform.name)
                    return;

            if (Input.touchCount == 1)
            {
                if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    // 手指滑动时，要触发的代码 
                    float s01 = Input.GetAxis("Mouse X"); //手指水平移动的距离
                    float s02 = Input.GetAxis("Mouse Y"); //手指垂直移动的距离
                    if (Mathf.Abs(s01) < 0.1 && Mathf.Abs(s02) < 0.1)
                        return;
                    onSlide.Invoke(s01, s02);
                }
            }
        }
        
    }
}
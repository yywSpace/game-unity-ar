using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Script.PutModelScene
{
    public class DoubleClickEventTrigger : MonoBehaviour
    {
        public UnityEvent onDoubleClick = new UnityEvent();
        private Camera _camera;
        float scale = .2f;
        private double _lastKickTime; // 上一次鼠标抬起的时间（用来处理双击）

        void Start()
        {
            _lastKickTime = Time.realtimeSinceStartup;
        }

        // Update is called once per frame
        void Update()
        {
            if (_camera == null)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                // 如果点击物体
                if (Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
                {
                    // 双击
                    //检测上次点击的时间和当前时间差 在一定范围内认为是双击
                    if (Time.realtimeSinceStartup - _lastKickTime < scale)
                    {
                        onDoubleClick?.Invoke();
                    }

                    //重新设置上次点击的时间
                    _lastKickTime = Time.realtimeSinceStartup;
                }
            }
        }

        public void SetCamera(Camera cam)
        {
            _camera = cam;
        }
    }
}
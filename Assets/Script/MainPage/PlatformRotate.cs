using UnityEngine;
using UnityEngine.Events;

namespace Script.MainPage
{

    public class PlatformRotate : MonoBehaviour
    {
        private Vector3 _startPos;
        private Vector3 _endPos;
        public float slideDistance = 5;
        private bool _isClick;
        private bool _hasLoad;
        public OnSlideEvent onSlide = new OnSlideEvent();

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("mouse down");
                _startPos = Input.mousePosition;
                _isClick = true;
                _hasLoad = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                print("mouse up");
                _endPos = Input.mousePosition;
                _isClick = false;
            }
            
            // 如果还未松手，则返回
            if (_isClick || _hasLoad)
                return;
            
            Vector3 v = _endPos - _startPos;

            // 上下滑动，返回
            if (Mathf.Abs(v.y) > Mathf.Abs(v.x))
                return;
            // 滑动距离不够，返回
            if (Mathf.Abs(v.x) < slideDistance)
                return;
            onSlide?.Invoke(v.x,v.y);
            _hasLoad = true;
        }
    }
}
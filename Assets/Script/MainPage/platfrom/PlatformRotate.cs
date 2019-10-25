using UnityEngine;
using UnityEngine.Events;

namespace Script.MainPage
{

    public class PlatformRotate : MonoBehaviour
    {
        private Vector3 _startPos;
        private Vector3 _endPos;
        public float slideDistance = 5;
        public Camera cam;
        private bool _isClick;
        private bool _hasSlide;
        public OnSlideEvent onSlide = new OnSlideEvent();

        // Update is called once per frame
        void Update()
        {
            // 按下鼠标时，如果焦点不在模型上则返回
            // 如果初始时在模型上，则之后划出模型仍然当作滑动成功
            if (Input.GetMouseButtonDown(0))
            {
                //如果非滑动模型，则返回
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (ArUtils.IsPointerOverUiObject())
                    return;
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    var hitParent = hitInfo.transform.parent;
                    string hitName = hitParent == null
                        ? hitInfo.transform.name
                        : hitParent.name;
                
                    var goParent = transform.parent;
                    string goName = goParent == null
                        ? gameObject.transform.name
                        : goParent.name;
                
                    if (hitName != goName)
                        return;
                }
                else
                {
                    return;
                }
                print("mouse down");
                _startPos = Input.mousePosition;
                _isClick = true;
                _hasSlide = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                print("mouse up");
                _endPos = Input.mousePosition;
                _isClick = false;
            }
            
            // 如果还未松手且已经滑动完成，则返回
            if (_isClick || _hasSlide)
                return;
            
            Vector3 v = _endPos - _startPos;

            // 上下滑动，返回
            if (Mathf.Abs(v.y) > Mathf.Abs(v.x))
                return;
            // 滑动距离不够，返回
            if (Mathf.Abs(v.x) < slideDistance)
                return;
            onSlide?.Invoke(v.x,v.y);
            _hasSlide = true;
        }
    }
}
using UnityEngine;

namespace Script.PutModelScene
{
    /// <summary>
    /// Double click change status.
    /// 双击改变模型的操作状态
    /// </summary>
    public class DoubleClickChangeStatus : MonoBehaviour
    {
        public Camera cam;
        int _status = 1;
        private RotateAndUpDown _rotateAndUpDown;
        private TransfromAroundAndDistance _tfAroundAndDistance;
        private void Start()
        {
            _rotateAndUpDown = gameObject.GetComponent<RotateAndUpDown>();
            _tfAroundAndDistance = gameObject.GetComponent<TransfromAroundAndDistance>();
            DoubleClickEventTrigger doubleClick = gameObject.AddComponent<DoubleClickEventTrigger>();
            doubleClick.SetCamera(cam);
            doubleClick.onDoubleClick.AddListener(() =>
            {
                _status = -_status;
                _rotateAndUpDown.enabled = !_rotateAndUpDown.enabled;
                _tfAroundAndDistance.enabled = !_tfAroundAndDistance.enabled;
            });
           
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
            return _status;
        }
    }
}

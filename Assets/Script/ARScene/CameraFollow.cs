using UnityEngine;

namespace Script.ARScene
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject targetObject;
        
        // Update is called once per frame
        void Update()
        {
            // 始终位于object上方一定距离
            var position = targetObject.transform.position;
            transform.position = new Vector3(position.x, position.y + 100, position.z);
        }
    }
}

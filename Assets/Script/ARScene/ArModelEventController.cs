using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.ARScene
{
    public class ArModelEventController : MonoBehaviour
    {
        public Camera cam;
        public Action onModelClick;

        // Update is called once per frame
        void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                // 如果点击物体
                if (!ArUtils.IsPointerOverUiObject() && Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
                {
                    onModelClick?.Invoke();
                }
            }
        }
 
        
        public void SetCamera(Camera camera)
        {
            cam = camera;
        }
    }
}

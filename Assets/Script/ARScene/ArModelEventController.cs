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
                if (!IsPointerOverUiObject() && Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
                {
                    onModelClick?.Invoke();
                }
            }
        }
        private bool IsPointerOverUiObject()
        {
            //判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        
        public void SetCamera(Camera camera)
        {
            cam = camera;
        }
    }
}

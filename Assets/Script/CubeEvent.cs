using System;
using System.Collections.Generic;
using Script.PutModelScene;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script
{
    public class CubeEvent : MonoBehaviour
    {
   

        public Camera cam;
        public GameObject dialogueBox;

        float Scale = .2f;
        private double _lastKickTime; // 上一次鼠标抬起的时间（用来处理双击）
       
        private void Start()
        {
            _lastKickTime = Time.realtimeSinceStartup;
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 如果点击物体
                if (!IsPointerOverUIObject() && Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
                {
                    dialogueBox.SetActive(!dialogueBox.activeSelf);

                    // 双击
                    if (Time.realtimeSinceStartup - _lastKickTime < Scale)//检测上次点击的时间和当前时间差 在一定范围内认为是双击
                    {
                        print("双击");
                        gameObject.GetComponent<RotateAndUpDown>().enabled = !gameObject.GetComponent<RotateAndUpDown>().enabled;


                    }
                    _lastKickTime = Time.realtimeSinceStartup;//重新设置上次点击的时间
                }
            }
        }
        private bool IsPointerOverUIObject()
        {//判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

    }
}

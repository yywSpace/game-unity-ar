using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeEvent : MonoBehaviour
{
    public Camera cam;
    public GameObject DialogueBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            // 如果点击物体
            if (EventSystem.current.IsPointerOverGameObject() == false && Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
            {
                DialogueBox.SetActive(!DialogueBox.activeSelf);
            }
        }
    }
}

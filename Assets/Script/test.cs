using System.Collections;
using System.Collections.Generic;
using Script.ARScene;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject mm;
    // Start is called before the first frame update
    void Start()
    {
        ArModelEventController amec =  gameObject.GetComponent<ArModelEventController>();
        amec.onModelClick = () =>
        {
            print("click model");
            mm.SetActive(!mm.activeSelf);
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

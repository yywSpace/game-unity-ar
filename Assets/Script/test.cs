using System.Collections;
using System.Collections.Generic;
using Script.ARScene;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Button button;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {

        _animator = GetComponent<Animator>();
     
        button.onClick.AddListener(() => { _animator.SetBool("show", !_animator.GetBool("show")); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

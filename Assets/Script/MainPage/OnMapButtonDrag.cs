using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.MainPage
{
    public class OnMapButtonDrag : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
    {
        private Animator _anim;
        private float _animProgress;

        // Start is called before the first frame update
        void Start()
        {
            _anim = GetComponent<Animator>();
            _animProgress = 0;
        }
        
        // 开始拖拽时开启动画
        public void OnBeginDrag(PointerEventData eventData)
        {
            _anim.SetBool("slide",true);
        }

        // 拖拽过程中控制动画
        public void OnDrag(PointerEventData eventData)
        {
            _animProgress += eventData.delta.x / 150;
            _anim.SetFloat("progress",_animProgress);
        }
        
        // 拖拽完成时停止动画
        public void OnEndDrag(PointerEventData eventData)
        {
            _animProgress = 0;
            _anim.SetFloat("progress", _animProgress);
           // _anim.SetBool("slide",false);
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.PutModelScene
{
    public class BagItem : MonoBehaviour, IPointerClickHandler
    {
        //定义一个委托    
        public delegate void OnMyPointerClick();
        public OnMyPointerClick onMyPointerClick;
        private Image _image;

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetImage(string imagePath)
        {
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            _image.sprite = sprite;
        }

        public void SetMyPointerClick(OnMyPointerClick clickEvent)
        {
            onMyPointerClick = clickEvent;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onMyPointerClick?.Invoke();
            print(_image.name);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItem : MonoBehaviour, IPointerClickHandler
{
    //定义一个委托    
    public delegate void OnMyPointerClick();
    public OnMyPointerClick onMyPointerClick;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetImage(string imagePath)
    {
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        image.sprite = sprite;
    }

    public void SetMyPointerClick(OnMyPointerClick clickEvent)
    {
        onMyPointerClick = clickEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onMyPointerClick?.Invoke();
        print(image.name);
    }
}


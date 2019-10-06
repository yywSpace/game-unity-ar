using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Script.Event
{
    public class PointerClickEventTrigger: MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onPointerClick = new UnityEvent();
        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke();
        }
    }
}
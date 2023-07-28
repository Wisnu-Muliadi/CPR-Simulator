using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UserInterface
{
    public class PointerDownEventHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] UnityEvent pointerDownEvent;
        public void OnPointerClick(PointerEventData eventData)
        {
            pointerDownEvent.Invoke();
        }
    }
}
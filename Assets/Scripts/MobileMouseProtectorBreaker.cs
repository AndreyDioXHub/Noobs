using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MobileMouseProtectorBreaker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private OnScreenMouse _onScreen;

    void Start()
    {
        
    }

    void Update()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _onScreen.Break();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _onScreen.Break();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}

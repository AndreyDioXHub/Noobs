using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

[AddComponentMenu("Input/On-Screen Button")]
public class OnScreenButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{/*
    [SerializeField]
    private RectTransform _rect;
    [SerializeField]
    private TextMeshProUGUI _text0;*/

    public void OnPointerUp(PointerEventData data)
    {
        SendValueToControl(0.0f);/*
        _rect.anchoredPosition = data.position;
        _text0.text = $"{data.position}";*/
    }

    public void OnPointerDown(PointerEventData data)
    {
        SendValueToControl(1.0f);/*
        _rect.anchoredPosition = data.position;
        _text0.text = $"{data.position}";*/

    }
    public void Update()
    {
        if (Input.touchCount == 0)
        {
            SendValueToControl(0.0f);
        }
    }

    [InputControl(layout = "Button")]
    [SerializeField]
    private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}

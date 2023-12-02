using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField]
    private Touch[] _touches;
    [SerializeField]
    private Vector2 _downPosition;
    [SerializeField]
    private float _minDistance, _breackDistance = 300;

    [SerializeField]
    private List<Image> _images = new List<Image>();

    public void OnPointerUp(PointerEventData data)
    {
        SendValueToControl(0.0f);
        _downPosition = data.position;

        /*
        _rect.anchoredPosition = data.position;
        _text0.text = $"{data.position}";*/
    }

    public void OnPointerDown(PointerEventData data)
    {
        SendValueToControl(1.0f);
        _downPosition = Vector2.zero;

        /*
        _rect.anchoredPosition = data.position;
        _text0.text = $"{data.position}";*/

    }

    public void Update()
    {
        _touches = Input.touches;

        if (_touches.Length > 0)
        {
        }

        if (_touches.Length == 0)
        {
            SendValueToControl(0.0f);
            _minDistance = _breackDistance * 2;
        }
        else
        {
            _minDistance = Vector2.Distance(_touches[0].position, _downPosition);

            for (int i = 0; i < _touches.Length; i++)
            {
                var d = Vector2.Distance(_touches[i].position, _downPosition);
                _minDistance = d < _minDistance ? d : _minDistance;
                //needBreak = needBreak || d > _breackDistance;
            }

            if (_minDistance > _breackDistance)
            {
                SendValueToControl(0.0f);
            }
        }

        if(SettingScreen.IsActive || AdsScreen.IsActive)
        {
            foreach(var i in _images)
            {
                i.color = new Color(0,0,0,0);
            }
        }
        else
        {
            foreach (var i in _images)
            {
                i.color = new Color(1, 1, 1, 1);
            }
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

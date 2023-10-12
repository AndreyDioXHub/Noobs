using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

public class OnScreenSectorStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    [FormerlySerializedAs("movementRange")]
    [SerializeField]
    private float m_MovementRange = 50;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    private Vector3 m_StartPos;
    private Vector2 m_PointerDownPos;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    [SerializeField]
    private Touch[] _touches;
    [SerializeField]
    private Vector2 _downPosition;
    [SerializeField]
    private float _minDistance, _breackDistance = 300;
    [SerializeField]
    private float _dot;
    [SerializeField]
    private Vector2 _pos;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        _downPosition = eventData.position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
        //Debug.Log("OnPointerDown");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - m_PointerDownPos;

        delta = Vector2.ClampMagnitude(delta, movementRange);
        ((RectTransform)transform).anchoredPosition = m_StartPos + (Vector3)delta;

        var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);

        if (((RectTransform)transform).anchoredPosition.x > 0)
        {
            _dot = Vector2.Angle(Vector2.up, ((RectTransform)transform).anchoredPosition.normalized);
        }
        else
        {
            _dot = -Vector2.Angle(Vector2.up, ((RectTransform)transform).anchoredPosition.normalized);
        }

        if (_dot > 0 && _dot < 22.5f)
        {
            _pos = new Vector2(0, 1);
        }

        if (_dot > 22.5f && _dot < 67.5f)
        {
            _pos = new Vector2(1, 1);
        }

        if (_dot > 67.5f && _dot < 112.5f)
        {
            _pos = new Vector2(1, 0);
        }

        if (_dot > 112.5f && _dot < 157.5f)
        {
            _pos = new Vector2(1, -1);
        }

        if (_dot > 157.5f && _dot < 180f)
        {
            _pos = new Vector2(0, -1);
        }

        if (_dot >-180  && _dot < -157.5f)
        {
            _pos = new Vector2(0, -1);
        }

        if (_dot > -157.5f  && _dot < -112.5f)
        {
            _pos = new Vector2(-1, -1);
        }

        if (_dot > -112.5f && _dot < -67.5f)
        {
            _pos = new Vector2(-1, 0);
        }

        if (_dot > -67.5f && _dot < -22.5f)
        {
            _pos = new Vector2(-1, 1);
        }

        if (_dot > -22.5f  && _dot <= 0)
        {
            _pos = new Vector2(0, 1);
        }

        SendValueToControl(_pos);
        //Debug.Log("OnDrag");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        ((RectTransform)transform).anchoredPosition = m_StartPos;
        _downPosition = Vector2.zero;
        //Debug.Log("OnPointerUp");
    }

    void Start()
    {
        m_StartPos = ((RectTransform)transform).anchoredPosition;

    }

    public void Update()
    {
        _touches = Input.touches;

        if (_touches.Length > 0)
        {
        }

        if (_touches.Length == 0)
        {
            SendValueToControl(Vector2.zero);
            ((RectTransform)transform).anchoredPosition = m_StartPos;
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
                SendValueToControl(Vector2.zero);
                ((RectTransform)transform).anchoredPosition = m_StartPos;
            }
        }

    }
}

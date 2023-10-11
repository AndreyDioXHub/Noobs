using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using TMPro;

/// <summary>
/// A stick control displayed on screen and moved around by touch or other pointer
/// input.
/// </summary>
[AddComponentMenu("Input/On-Screen Stick")]
public class OnScreenStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{/*
    [SerializeField]
    private RectTransform _rect;*/
    /*[SerializeField]
    private TextMeshProUGUI _text0;*/
    [SerializeField]
    private Touch[] _touches;
    [SerializeField]
    private Vector2 _downPosition;
    [SerializeField]
    private float _minDistance, _breackDistance = 300;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        _downPosition = eventData.position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
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
        SendValueToControl(newPos);/*
        _rect.anchoredPosition = eventData.position;
        _text0.text = $"{eventData.position}";*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((RectTransform)transform).anchoredPosition = m_StartPos;
        SendValueToControl(Vector2.zero);
        _downPosition = Vector2.zero;
        /*
        _rect.anchoredPosition = eventData.position;
        _text0.text = $"{eventData.position}";*/
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
            _minDistance = _breackDistance*2;
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

    private void Start()
    {
        m_StartPos = ((RectTransform)transform).anchoredPosition;
    }

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
}

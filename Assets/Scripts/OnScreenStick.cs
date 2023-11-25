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
    private Touch _touch;
    [SerializeField]
    private Touch[] _touches;
    [SerializeField]
    private Vector2 _downPosition;
    [SerializeField]
    private float _breackDistance = 300;

    public float MovementRange
    {
        get => _movementRange;
        set => _movementRange = value;
    }

    [FormerlySerializedAs("movementRange")]
    [SerializeField]
    private float _movementRange = 50;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string _controlPath;

    private Vector3 _startPos;
    private Vector2 _pointerDownPos;

    protected override string controlPathInternal
    {
        get => _controlPath;
        set => _controlPath = value;
    }

    private void Start()
    {
        _touch = new Touch { fingerId = -1 };
        _startPos = ((RectTransform)transform).anchoredPosition;
    }

    public void Update()
    {
        _touches = Input.touches;
        bool needBreak = false;

        for (int i = 0; i < _touches.Length; i++)
        {
            var d = Vector2.Distance(_touches[i].position, _downPosition);
            needBreak = needBreak || d > _breackDistance;
        }

        if (needBreak || _touches.Length == 0)
        {
            SendValueToControl(Vector2.zero);
            ((RectTransform)transform).anchoredPosition = _startPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _downPosition = eventData.position;
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _pointerDownPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - _pointerDownPos;

        delta = Vector2.ClampMagnitude(delta, MovementRange);
        ((RectTransform)transform).anchoredPosition = _startPos + (Vector3)delta;
        Debug.Log(delta);
        var newPos = new Vector2(delta.x / MovementRange, delta.y / MovementRange);
        SendValueToControl(newPos);
        /*
        _rect.anchoredPosition = eventData.position;
        _text0.text = $"{eventData.position}";*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((RectTransform)transform).anchoredPosition = _startPos;
        SendValueToControl(Vector2.zero);
        _downPosition = Vector2.zero;
        /*
        _rect.anchoredPosition = eventData.position;
        _text0.text = $"{eventData.position}";*/
    }


}

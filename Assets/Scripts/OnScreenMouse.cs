using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;

public class OnScreenMouse : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
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

    [SerializeField]
    private Vector3 _positionStart;
    [SerializeField]
    private Vector2 _positionDrag;
    [SerializeField]
    private Vector2 _positionDragPrev;
    [SerializeField]
    private Vector2 delta;
    [SerializeField]
    private bool _isDrag;
    [SerializeField]
    private Canvas _canvas;

    private Finger _movementFinger;

    [SerializeField]
    private Vector2 _anchoredPosition;
    [SerializeField]
    private Vector2 _interactDistance;

    [SerializeField]
    private Vector2 _delta;

    protected override string controlPathInternal
    {
        get => _controlPath;
        set => _controlPath = value;
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        //_positionStart = 
    }

    private void Update()
    {
        SendValueToControl(_delta);
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == _movementFinger)
        {
            if(movedFinger.touchHistory.Count > 2)
            {
                _positionDrag = movedFinger.screenPosition / _canvas.scaleFactor;
                _positionDragPrev = movedFinger.touchHistory[1].screenPosition / _canvas.scaleFactor;
                _delta = _positionDragPrev - _positionDrag;
            }

            //((RectTransform)transform).anchoredPosition = screenPosition ;
            /*
            var delta = screenPosition - _startPos;

            delta = Vector2.ClampMagnitude(delta, MovementRange);

            ((RectTransform)transform).anchoredPosition = _startPos + delta;

            var newPos = new Vector2(delta.x / MovementRange, delta.y / MovementRange);
            SendValueToControl(newPos);
            Debug.Log(newPos);*/
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == _movementFinger)
        {
            _movementFinger = null;
            _isDrag = false;
            _delta = Vector2.zero;
            //SendValueToControl(_delta);
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        Vector2 screenPosition = touchedFinger.screenPosition / _canvas.scaleFactor;

        if (screenPosition.x < _anchoredPosition.x - _interactDistance.x || screenPosition.x > _anchoredPosition.x + _interactDistance.x)
        {
            if (screenPosition.y < _anchoredPosition.y - _interactDistance.y || screenPosition.y > _anchoredPosition.y + _interactDistance.y)
            {
                if (_movementFinger == null)
                {
                    _movementFinger = touchedFinger;

                    _positionDrag = screenPosition;
                    _positionDragPrev = screenPosition;
                    _isDrag = true;
                }
            }
        }
    }

    public void Break()
    {
        _isDrag = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}

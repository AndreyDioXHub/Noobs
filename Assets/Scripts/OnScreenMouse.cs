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

public class OnScreenMouse : OnScreenControl
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
    protected override void OnEnable()
    {
        base.OnEnable();
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
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

        if (_movementFinger == null)
        {
            _positionDrag = Vector2.zero;
            _positionDragPrev = Vector2.zero;
            Debug.Log($"1) {_delta}");
        }
        else
        {
            _positionDrag = Input.mousePosition;
            Debug.Log($"2) {_delta}");

            if (_positionDrag == Vector2.zero || _positionDragPrev == Vector2.zero || _positionDrag == _positionDragPrev)
            {
                _delta = Vector2.zero;
                Debug.Log($"3) {_delta}");
            }
            else
            {
                _delta = _positionDrag - _positionDragPrev;
                Debug.Log($"4) {_delta}");
            }
        }

        Debug.Log($"5) {_delta}");
        SendValueToControl(_delta);

    }

    private void LateUpdate()
    {
        if (_movementFinger == null)
        {
            _positionDrag = Vector2.zero;
            _positionDragPrev = Vector2.zero;
        }
        else
        {
            _positionDragPrev = Input.mousePosition;
        }

    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == _movementFinger)
        {

        }
        else
        {

        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == _movementFinger)
        {
            _movementFinger = null;
            _isDrag = false;
            _delta = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        Vector2 screenPosition = touchedFinger.screenPosition / _canvas.scaleFactor;

        Rect r = new Rect(_anchoredPosition, _interactDistance);
        _delta = Vector2.zero;

        if (!r.Contains(screenPosition))
        {
            if (_movementFinger == null)
            {
                _movementFinger = touchedFinger;

               /* _positionDrag = screenPosition;
                _positionDragPrev = screenPosition;*/
                _isDrag = true;
            }
        }
    }

    public void Break()
    {
        _isDrag = false;
    }

}

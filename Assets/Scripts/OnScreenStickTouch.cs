using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

/// <summary>
/// A stick control displayed on screen and moved around by touch or other pointer
/// input.
/// </summary>
public class OnScreenStickTouch : OnScreenControl
{
    /*
    [SerializeField]
    private RectTransform _rect;*/
    /*[SerializeField]
    private TextMeshProUGUI _text0;*/
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Vector2 _anchoredPosition;
    [SerializeField]
    private Vector2 _interactDistance;
    [SerializeField]
    private Vector2 _delta;


   [SerializeField]
    private int _corner; //0-left, 1-right

    [SerializeField]
    private Vector2 _downPosition;

    private Finger _movementFinger;

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

    private Vector2 _startPos;
    private Vector2 _pointerDownPos;

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
    
    private void Start()
    {
        _startPos = ((RectTransform)transform).anchoredPosition;
    }

    private void FixedUpdate()
    {
        SendValueToControl(_delta);

    }

    public void LateUpdate()
    {
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == _movementFinger)
        {
            Vector2 screenPosition = (movedFinger.screenPosition / _canvas.scaleFactor) - ((RectTransform)transform).sizeDelta / 2;
            //((RectTransform)transform).anchoredPosition = screenPosition ;

            var delta = screenPosition - _startPos;

            delta = Vector2.ClampMagnitude(delta, MovementRange);

            ((RectTransform)transform).anchoredPosition = _startPos + delta;

            _delta = new Vector2(delta.x / MovementRange, delta.y / MovementRange);
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == _movementFinger)
        {
            _movementFinger = null;
            ((RectTransform)transform).anchoredPosition = _startPos;

            _delta = Vector2.zero;
            _downPosition = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        Vector2 screenPosition = touchedFinger.screenPosition / _canvas.scaleFactor;

        if (screenPosition.x > _anchoredPosition.x - _interactDistance.x && screenPosition.x < _anchoredPosition.x + _interactDistance.x)
        {
            if (screenPosition.y > _anchoredPosition.y - _interactDistance.y && screenPosition.y < _anchoredPosition.y + _interactDistance.y)
            {
                if (_movementFinger == null)
                {
                    _movementFinger = touchedFinger;
                    _pointerDownPos = screenPosition;
                }
            }
        }
    }
}

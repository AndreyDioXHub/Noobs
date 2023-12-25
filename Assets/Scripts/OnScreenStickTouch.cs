using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;

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
    private List<Image> _images = new List<Image>();

    [SerializeField]
    private Image _bg;
    [SerializeField]
    private Image _joydtick;

    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Vector2 _anchoredPosition;
    [SerializeField]
    private Vector2 _touchPosition;

    [SerializeField]
    private Vector2 _interactDistance;
    [SerializeField]
    private Vector2 _pos;


    [SerializeField]
    private int _corner; //0-left, 1-right
    [SerializeField]
    private float d; //0-left, 1-right

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

    [SerializeField]
    private Vector2 _startPos;
    [SerializeField]
    private Vector2 _screenPosition;

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

    private void Update()
    {
        SendValueToControl(_pos);

        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsManager.AdsPlaying)
        {
            foreach (var i in _images)
            {
                i.color = new Color(0, 0, 0, 0);
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


    private void FixedUpdate()
    {

    }

    public void LateUpdate()
    {
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == _movementFinger)
        {
            _screenPosition = (movedFinger.screenPosition / _canvas.scaleFactor);// - ((RectTransform)transform).sizeDelta / 2;
            //((RectTransform)transform).anchoredPosition = screenPosition ;

            var delta = _screenPosition - _startPos;// - _joydtick.rectTransform.sizeDelta/2;

            //delta = Vector2.ClampMagnitude(delta, MovementRange);
            delta.x = delta.x > MovementRange ? MovementRange : delta.x;
            delta.x = delta.x < -MovementRange ? -MovementRange : delta.x;
            delta.y = delta.y > MovementRange ? MovementRange : delta.y;
            delta.y = delta.y < -MovementRange ? -MovementRange : delta.y;

            ((RectTransform)transform).anchoredPosition = _screenPosition - ((RectTransform)transform).sizeDelta / 2; // _startPos + delta;
            _pos = delta/ MovementRange;

            //_pos = new Vector2(delta.x / MovementRange, delta.y / MovementRange);

            Vector2 anchoredPosition = ((RectTransform)transform).anchoredPosition - _anchoredPosition;

        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == _movementFinger)
        {
            _movementFinger = null;
            ((RectTransform)transform).anchoredPosition = _startPos - _joydtick.rectTransform.sizeDelta / 2;

            _pos = Vector2.zero;
            _downPosition = Vector2.zero;

            _bg.enabled = false;
            _joydtick.enabled = false;
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
                    _startPos = screenPosition;

                    _bg.enabled = true;
                    _joydtick.enabled = true;

                    _bg.rectTransform.anchoredPosition = _startPos - _bg.rectTransform.sizeDelta/2;
                    _joydtick.rectTransform.anchoredPosition = _startPos - _joydtick.rectTransform.sizeDelta/2;

                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class RobloxController : MonoBehaviour
{
    public UnityEvent OnEscDown = new UnityEvent();
    public UnityEvent OnEnterDown = new UnityEvent();
    public UnityEvent OnJumpPressed = new UnityEvent();
    public UnityEvent<bool> OnDie = new UnityEvent<bool>();
    

    public static RobloxController Instance;

    [SerializeField] 
    private CharacterController _controller;
    [SerializeField]
    private Transform _cam;
    [SerializeField]
    private GroundCheck _groundCheck;
    [SerializeField]
    private float _gravity = -9.81f;

    [SerializeField]
    private float _speed = 6f;
    [SerializeField]
    private float _jumpHeight = 3f;
    [SerializeField]
    private float _airStrafe = 0.3f;

    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private bool _isMoving;
    [SerializeField]
    private bool _isJump;

    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    private Vector3 _gravityVector = new Vector3(0, -1f, 0);
    private Vector3 _gravityVectorCur = new Vector3(0, -1f, 0);
    private Vector3 _moveVector;
    private Vector3 _moveVectorCur;
    private Vector3 _direction;

    private Vector2 _axisMove;
    private float _inAirTime = 0.1f;
    private float _inAirTimeCur;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _cam = Camera.main.transform; 

        _gravityVector = new Vector3(0, -1f, 0);
        _gravityVectorCur = new Vector3(0, -1f, 0);
}

    void Update()
    {
        _controller.Move(_moveVectorCur);
        _controller.Move(_gravityVectorCur);

        _moveVectorCur = Vector3.zero;
        _gravityVectorCur = Vector3.zero;
    }

    public virtual void FixedUpdate()
    {

        if (transform.position.y < 0)
        {
            return;
        }

        //Debug.Log($"{ SettingScreen.IsActive} {AdsScreen.IsActive} {AdsButtonView.IsActive} {CheckPointManager.Instance.IsWin} {ChatTexts.IsActive}");

        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin 
            || ChatTexts.IsActive || AdsManager.AdsPlaying)// || BlockCountManager.Instance.BlocksCount == 0)
        {

        }
        else
        {

            _isGrounded = _groundCheck.IsGrounded;
            Vector3 direction = new Vector3(_axisMove.x, 0f, _axisMove.y);//.normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Vector3.zero;

                _direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                _moveVector = _direction * direction.magnitude * _speed * Time.fixedDeltaTime;

                _moveVectorCur += _moveVector;

                _isMoving = true;
            }

            if (direction.magnitude <= 0.0f)
            {
                _isMoving = false;
            }

        }

        _gravityVector.y += _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;

        if (_isJump)
        {
            _inAirTimeCur += Time.fixedDeltaTime;

            if (_isGrounded && _inAirTimeCur > _inAirTime)
            {
                _isJump = false;
                _inAirTimeCur = 0;
                _gravityVector = Vector3.zero;
            }
        }
        else
        {
            _gravityVector.y = _gravity * Time.fixedDeltaTime;
        }

        _gravityVectorCur += _gravityVector;
    }

    public virtual void Push(Vector3 direction, float height)
    {
        _isJump = true;
        _gravityVector = direction * Mathf.Sqrt(height * -2f * _gravity) * Time.fixedDeltaTime;
    }

    public void ReturnToCheckPoint(Vector3 position)
    {
        OnDie?.Invoke(false);
        _gravityVector = Vector3.zero;
        _controller.enabled = false;
        transform.position = position;
        _controller.enabled = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Read.
        _axisMove = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.IsWIN 
            || ChatTexts.IsActive || AdsManager.AdsPlaying)// || BlockCountManager.Instance.BlocksCount == 0)
        {
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Started:
                OnJump();
                break;
            case InputActionPhase.Canceled:
                //_movement.Jump(false);
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Waiting:
                break;
        }
    }

    public void OnJump()
    {
        if (_groundCheck.IsGrounded)
        {
            _isJump = true;
            _gravityVector.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity) * Time.fixedDeltaTime;
            OnJumpPressed?.Invoke();
        }
    }
    public void OnMenu(InputAction.CallbackContext context)
    {
        if(AdsManager.AdsPlaying)
        {
            return;
        }

        //Read.
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OnEscDown?.Invoke();
                break;
            case InputActionPhase.Canceled:
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Waiting:
                break;
        }
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        //Read.
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OnEnterDown?.Invoke();
                break;
            case InputActionPhase.Canceled:
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Waiting:
                break;
        }
    }
}

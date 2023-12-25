using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    private float _jumpspeed = 5;
    [SerializeField]
    private float _turnSmoothTime = 0.1f;
    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private bool _isMoving;
    [SerializeField]
    private bool _isJump;
    [SerializeField]
    private float _jumpHeight = 3f;
    [SerializeField]
    private float _turnSmoothVelocity;
    [SerializeField]
    private Vector2 _axisMove;
    [SerializeField]
    private Vector3 _gravityVector;


    private float _inAirTime = 0.1f;
    private float _inAirTimeCur;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _cam = Camera.main.transform;
    }

    void Update()
    {
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

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDir * direction.magnitude * _speed * Time.fixedDeltaTime);
                _isMoving = true;
            }

            if (direction.magnitude <= 0.0f)
            {
                _isMoving = false;
            }
            /*
            if (_isJump && _isGrounded)
            {
                _isJump = false;

                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            _velocity.y += _gravity * Time.deltaTime;*/

            //_controller.Move(_velocity * Time.fixedDeltaTime);

        }

        _gravityVector.y += _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;

        if (_isJump)
        {
            _inAirTimeCur += Time.fixedDeltaTime;

            if (_isGrounded && _inAirTimeCur > _inAirTime)
            {
                _isJump = false;
                _inAirTimeCur = 0;
            }
        }
        else
        {
            _gravityVector = Vector3.zero;
            _gravityVector.y = _gravity * Time.fixedDeltaTime;
        }
        if (_groundCheck.IsGrounded)
        {
        }
        else
        {
            //_blender.AddOffcet(_offcetMove);
        }

        _controller.Move(_gravityVector);
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
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin 
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

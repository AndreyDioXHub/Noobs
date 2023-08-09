using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    protected PositionOffcetBlender _blender;
    [SerializeField]
    protected PlayerSpeed _playerSpeed;
    [SerializeField]
    protected GroundCheck _groundCheck;

    [SerializeField]
    protected float _speedWalk = 10;
    [SerializeField]
    protected float _speedRun = 15;
    [SerializeField]
    protected float _speedCrouch = 5;

    [SerializeField]
    protected float _jumpheight = 2f;
    [SerializeField]
    protected float _gravity = -9.8f;
    [SerializeField]
    protected bool _isTransition;


    protected Vector3 _offcetMove;
    protected Vector3 _gravityVector;

    protected bool _isRun;
    protected bool _isCrouch;
    protected bool _isJump;

    private float _inAirTime = 0.1f;
    private float _inAirTimeCur;


    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        /*_blender = GetComponent<PositionOffcetBlender>();
        _state = GetComponent<CharacterStateController>();
        _playerSpeed = GetComponent<PlayerSpeed>();*/

        Init();
    }
    
    public virtual void Init()
    {
        if (_isCrouch)
        {
            _playerSpeed.SetSpeed(_speedCrouch);
        }
        else
        {
            _playerSpeed.SetSpeed(_speedWalk);
        }
    }
    public virtual void SetChangeState(bool isTransition)
    {
        _isTransition = isTransition;
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        _gravityVector.y += _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;

        if (_groundCheck.IsGrounded)
        {
            if (_isJump)
            {
                _inAirTimeCur += Time.fixedDeltaTime;

                if (_groundCheck.IsGrounded && _inAirTimeCur > _inAirTime)
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
        }
        else
        {
            //_blender.AddOffcet(_offcetMove);
        }


        _blender.AddOffcet(_gravityVector);
    }

    public virtual void Move(float horizontal, float vertical)
    {
        if (_groundCheck.IsGrounded)
        {
            _offcetMove = (transform.right * horizontal + transform.forward * vertical) * _playerSpeed.Speed * Time.fixedDeltaTime;           
        }
        else
        {
            _offcetMove = (transform.right * horizontal + transform.forward * vertical) * _playerSpeed.Speed * Time.fixedDeltaTime;
        }

        _blender.AddOffcet(_offcetMove);
    }

    public virtual void Crouch(bool isCrouch)
    {
        _isCrouch = isCrouch;
        _offcetMove = Vector3.zero;
        if (_isRun)
        {
            Run(false);
        }

        if (isCrouch)
        {
            _playerSpeed.SetSpeed(_speedCrouch);
        }
        else
        {
            _playerSpeed.SetSpeed(_speedWalk);
        }
    }

    public virtual void Run(bool isRun)
    {
        _isRun = isRun;

        if (_isCrouch)
        {
            Crouch(false);
        }

        if (isRun)
        {
            _playerSpeed.SetSpeed(_speedRun);
        }
        else
        {
            _playerSpeed.SetSpeed(_speedWalk);
        }
    }

    public virtual void Jump(bool isJump)
    {
        if (isJump)
        {
            _isJump = isJump;
            //_isOnTheAir = true;
        }

        if (_groundCheck.IsGrounded)
        {
            //Debug.Log(_isJump);
            _gravityVector.y = Mathf.Sqrt(_jumpheight * -2f * _gravity) * Time.fixedDeltaTime;
        }
    }

    public virtual void Push(Vector3 direction,float height)
    {
        /*Vector3 offcet = direction * Mathf.Sqrt(height * -2f * _gravity) * Time.fixedDeltaTime;
        Debug.Log($"{offcet} {Mathf.Sqrt(height * -2f * _gravity) * Time.fixedDeltaTime}");
        _blender.AddOffcet(offcet);*/
        _gravityVector = direction * Mathf.Sqrt(height * -2f * _gravity) * Time.fixedDeltaTime; //Mathf.Sqrt(height * -2f * _gravity) * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        _gravityVector = Vector3.zero;
        _offcetMove = Vector3.zero;
    }

}

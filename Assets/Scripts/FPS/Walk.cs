using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : PlayerMovement
{
    private CharacterController _character;
    [SerializeField]
    private float _speed = 3;

    private Vector2 _inputMovement;
    private Vector3 _moveVectorForward;
    private Vector3 _moveVectorRight;
    private Vector3 _airStrafeVector;
    private Vector3 _velocityVector;
    private Vector3 _resultVector;

    [SerializeField]
    private float _airStarafeCof = 0.1f;
    private float _gravity = -9.8f;

    #region Ground
    private int _chekersCount = 8;
    private int _ringCount = 2;
    private float _distance = 0.1f;
    private float _degToRad = 0;

    private bool _isGrounded = false;
    #endregion

    #region Jump
    [SerializeField]
    private AnimationCurve _jumpCurve;
    private float _experationCurTime = 0;
    private float _experationLastTime = 0;
    [SerializeField]
    private float _duration = 1;
    [SerializeField]
    private float _jumpHeight = 1;

    #endregion

    private PlayerAirStates _playerStates = PlayerAirStates.fall;

    public override void Start()
    {
        base.Start();
        _velocityVector = new Vector3(0, -0.002f, 0);
        _degToRad = Mathf.PI / 180.0f;
        _distance = _character.skinWidth;
    }

    public override void Init(CharacterController character)
    {
        base.Init(character);
        _character = character;
    }

    public override void Update()
    {
        base.Update();
        
        CheckGround();

        if (_isGrounded)
        {
            Move();
        }
        else
        {
            {
                switch (_playerStates)
                {
                    case PlayerAirStates.fall:
                        Fall();
                        break;
                    case PlayerAirStates.jump:
                        Jump(); 
                        break;
                    case PlayerAirStates.climbing:
                        break;
                    default:
                        break;
                }
            }

            /**/

        }

        _resultVector = _moveVectorForward + _moveVectorRight + _velocityVector + _airStrafeVector;
        _character.Move(_resultVector);

    }

    public override void MoveValue(Vector2 inputMovement)
    {
        base.MoveValue(inputMovement);
        _inputMovement = inputMovement;
    }

    public override void JumpValue()
    {
        if (_isGrounded)
        {
            //if(_inputMovement.x == )
            // _moveVector - transform.right * _inputMovement.x * _speed * Time.deltaTime;
            if (_inputMovement.y > 0)
            {
                _moveVectorForward += _moveVectorRight;
            }

            _moveVectorRight = Vector3.zero;

            _playerStates = PlayerAirStates.jump;
            _velocityVector = new Vector3(0, 0.1f, 0);
            _character.Move(_velocityVector);
            _isGrounded = false;
        }
        else
        {
            _moveVectorForward += transform.forward * 0.001f;
        }
        
    }

    public void Move()
    {
        _velocityVector = new Vector3(0, -0.002f, 0);
        _airStrafeVector = new Vector3(0, 0, 0);
        _moveVectorRight = transform.right * _inputMovement.x * _speed * Time.deltaTime ;
        _moveVectorForward = transform.forward * _inputMovement.y * _speed * Time.deltaTime;
    }

    public void Jump()
    {
        _experationCurTime += Time.deltaTime;
        if (_experationCurTime > _duration)
        {
            _experationCurTime = 0;
            _experationLastTime = 0;
            _playerStates = PlayerAirStates.fall;
        }
        else
        {
            float progress = _experationCurTime / _duration;
            float lastprogress = _experationLastTime / _duration;
            _velocityVector = new Vector3(0, (_jumpCurve.Evaluate(progress) - _jumpCurve.Evaluate(lastprogress)) * _jumpHeight, 0);
            _experationLastTime = _experationCurTime;
        }
        AirStrafe();
        //_velocityVector = new Vector3(0, 0.08f, 0);
    }

    public void Fall()
    {
        _velocityVector = new Vector3(0, _velocityVector.y + _gravity * Time.deltaTime * Time.deltaTime, 0);
        AirStrafe();
    }

    public void AirStrafe()
    {
        if (_inputMovement.y == 0)
        {
            _airStrafeVector = transform.right * _inputMovement.x * _speed * Time.deltaTime * _airStarafeCof;
        }
        
    }

    public void CheckGround()
    {
        if (_playerStates == PlayerAirStates.fall)
        {
            int count = 0;
            float angle = 0;
            float sin = 0;
            float cos = 0;

            for (int j = 1; j < _ringCount + 1; j++)
            {
                for (int i = 0; i < _chekersCount; i++)
                {
                    angle = 360 * i / _chekersCount;

                    if (angle >= 360 || angle <= -360)
                    {
                        angle = 0;
                    }

                    sin = Mathf.Sin(_degToRad * angle);
                    cos = Mathf.Cos(_degToRad * angle);

                    Vector3 origin = new Vector3(transform.position.x + _character.radius * cos / j, transform.position.y - (_character.height * 0.5f), transform.position.z + _character.radius * sin / j);
                    Vector3 direction = transform.TransformDirection(Vector3.down);

                    if (Physics.Raycast(origin, direction, out RaycastHit hit, _distance))
                    {
                        Debug.DrawRay(origin, direction * _distance, Color.red);
                        count++;
                    }
                    else
                    {
                        Debug.DrawRay(origin, direction * _distance, Color.green);
                    }
                }
            }


            if (count > 0)
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }

            if (_character.isGrounded == true)
            {
                _isGrounded = true;
            }
        }
    }

    private enum PlayerAirStates
    {
        fall = 0,
        jump = 1,
        climbing =2
    }
}

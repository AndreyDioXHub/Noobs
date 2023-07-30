using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInput : KeyboardInput
{

    [SerializeField]
    private float _timeToJump = 5;
    [SerializeField]
    private float _timeToJumpCur = 0;

    [SerializeField]
    private float _timeToForward = 5;
    [SerializeField]
    private float _timeToForwardCur = 0;

    [SerializeField]
    private float _timeToBackward = 5;
    [SerializeField]
    private float _timeToBackwardCur = 0;

    [SerializeField]
    private float _timeToLeft = 5;
    [SerializeField]
    private float _timeToLeftCur = 0;

    [SerializeField]
    private float _timeToRight = 5;
    [SerializeField]
    private float _timeToRightCur = 0;

    private bool _isForward = false;
    private bool _isBackward = false;

    private bool _isLeft = false;
    private bool _isRight = false;

    private bool _needCare = false;



    private void Start()
    {

    }

    public override void Update()
    {
        _timeToJumpCur += Time.deltaTime;

        if(_timeToJumpCur > _timeToJump)
        {
            _timeToJumpCur = 0;
            _timeToJump = Random.Range(0, 2);
            _movement.Jump(true);
        }


    }

    public override void FixedUpdate()
    {
        _timeToForwardCur += Time.fixedDeltaTime;
        _timeToLeftCur += Time.fixedDeltaTime;
        _needCare = true;

        if (_timeToForwardCur > _timeToForward)
        {
            _timeToForwardCur = 0;
            _timeToForward = Random.Range(0, 1f);

            int rf = Random.Range(0, 2);
            _isForward = rf == 0;
            _isBackward = !_isForward;
        }

        if (_timeToLeftCur > _timeToLeft)
        {
            _timeToLeftCur = 0;
            _timeToLeft = Random.Range(0, 1f);

            int rl = Random.Range(0, 2);
            _isLeft = rl == 0;
            _isRight = !_isLeft;
        }

        float horizontal = 0;
        float vertical = 0;

        if (_isForward)
        {
            if (_accelerationVerticalBackwardTimeCur == 0)
            {
                _accelerationVerticalForwardTimeCur += Time.deltaTime;
                _accelerationVerticalForwardTimeCur = _accelerationVerticalForwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationVerticalForwardTimeCur;
            }
        }
        else
        {
            _accelerationVerticalForwardTimeCur = _accelerationVerticalForwardTimeCur < 0 ? 0 : _accelerationVerticalForwardTimeCur - Time.deltaTime;
        }

        if (_isBackward)
        {
            if (_accelerationVerticalForwardTimeCur == 0)
            {
                _accelerationVerticalBackwardTimeCur = _accelerationVerticalBackwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationVerticalBackwardTimeCur + Time.deltaTime;
            }
        }
        else
        {
            _accelerationVerticalBackwardTimeCur = _accelerationVerticalBackwardTimeCur < 0 ? 0 : _accelerationVerticalBackwardTimeCur - Time.deltaTime;
        }
        _accelerationVerticalForwardDuration = _accelerationVerticalForwardTimeCur / _accelerationTime;
        _accelerationVerticalBackwardDuration = _accelerationVerticalBackwardTimeCur / _accelerationTime;

        vertical = _accelerationCurve.Evaluate(_accelerationVerticalForwardDuration) - _accelerationCurve.Evaluate(_accelerationVerticalBackwardDuration);


        if (_isLeft)
        {
            if (_accelerationHorizontalBackwardTimeCur == 0)
            {
                _accelerationHorizontalForwardTimeCur += Time.deltaTime;
                _accelerationHorizontalForwardTimeCur = _accelerationHorizontalForwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationHorizontalForwardTimeCur;
            }
        }
        else
        {
            _accelerationHorizontalForwardTimeCur = _accelerationHorizontalForwardTimeCur < 0 ? 0 : _accelerationHorizontalForwardTimeCur - Time.deltaTime;
        }

        if (_isRight)
        {
            if (_accelerationHorizontalForwardTimeCur == 0)
            {
                _accelerationHorizontalBackwardTimeCur = _accelerationHorizontalBackwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationHorizontalBackwardTimeCur + Time.deltaTime;
            }
        }
        else
        {
            _accelerationHorizontalBackwardTimeCur = _accelerationHorizontalBackwardTimeCur < 0 ? 0 : _accelerationHorizontalBackwardTimeCur - Time.deltaTime;
        }

        _accelerationHorizontalForwardDuration = _accelerationHorizontalForwardTimeCur / _accelerationTime;
        _accelerationHorizontalBackwardDuration = _accelerationHorizontalBackwardTimeCur / _accelerationTime;

        horizontal = _accelerationCurve.Evaluate(_accelerationHorizontalForwardDuration) - _accelerationCurve.Evaluate(_accelerationHorizontalBackwardDuration);

        if (horizontal != 0 || vertical != 0)
        {
            //Vector3 dir = new Vector3(horizontal, 0, vertical);
            _lookDirection.position = transform.position + transform.forward * vertical + transform.right * horizontal;
            _model.LookAt(_lookDirection, _model.up);//.rotation = Quaternion.LookRotation(_lookDirection.position - _model.position, _model.up);
        }

        _animatorController.SetRun(horizontal, vertical);

        _movement.Move(horizontal, vertical);
    }
}

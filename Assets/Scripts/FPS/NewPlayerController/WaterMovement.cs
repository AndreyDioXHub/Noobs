using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : Movement
{
    [SerializeField]
    private Transform _cameraTransform;

    private UnderWaterVolume _volume { get => UnderWaterVolume.Instance; }
    [SerializeField]
    private Transform _anchor;
    [SerializeField]
    private bool _playerOnPlace;

    [SerializeField]
    private Vector3 _positionNext;
    [SerializeField]
    private Vector3 _positionCurent;
    [SerializeField]
    private float _timeBetweenNewCall = 0.5f;
    [SerializeField]
    private float _timeCurent;
    private bool _positionInited;

    public override void Start()
    {
        base.Start();
        if(CharacterGlobals.Instance != null) {
            _anchor = CharacterGlobals.Instance.PlayerWaterAnchor;
        }
    }

    public override void FixedUpdate()
    {
        if (_isTransition)
        {
            _anchor.position = new Vector3(transform.position.x, _anchor.position.y, transform.position.z);
        }

        if (_playerOnPlace)
        {
            _timeCurent += Time.fixedDeltaTime;

            if (_timeCurent > _timeBetweenNewCall)
            {
                _timeCurent = 0;
                _positionCurent = _positionNext;

                float delta = _volume.Cross.y - transform.position.y;
                //Debug.Log(delta);
                delta = delta < 0 ? 0 : delta;

                _positionNext = Ocean.Instance.WaterPosition(new Vector3(_anchor.position.x, transform.position.y, _anchor.position.z), delta);
                _positionNext.y += 0.5f;
            }

            Vector3 newPosition = Vector3.Lerp(_positionCurent, _positionNext, _timeCurent / _timeBetweenNewCall);
            _blender.AddOffcet(newPosition - transform.position);
            //transform.position = 
        }


        if (_isCrouch)
        {
            _blender.AddOffcet(-_cameraTransform.up * _playerSpeed.Speed * Time.fixedDeltaTime);
        }

        if (_isJump)
        {
            if (transform.position.y - 0.5f < _volume.Cross.y)
            {
                _blender.AddOffcet(_cameraTransform.up * _playerSpeed.Speed * Time.fixedDeltaTime);
            }
            else
            {
                _isJump = false;
            }
        }

    }
    public override void Init()
    {

        _playerSpeed.SetSpeed(_speedWalk);
        //base.Init();
    }

    public override void Move(float horizontal, float vertical)
    {
        _offcetMove = (_cameraTransform.right * horizontal + _cameraTransform.forward * vertical) * _playerSpeed.Speed * Time.fixedDeltaTime;

        if (transform.position.y + _offcetMove.y > _volume.Cross.y && !_playerOnPlace)
        {
            _offcetMove.y = -0.1f * _playerSpeed.Speed * Time.fixedDeltaTime;
        }

        _blender.AddOffcet(_offcetMove);

        if (horizontal == 0 && vertical == 0 && !(_isCrouch || _isJump))
        {
            _playerOnPlace = true;

            if (!_positionInited)
            {
                _positionInited = true;
                _timeCurent = 0;
                _positionNext = transform.position; //Ocean.Instance.WaterPosition(_anchor.position);
                _positionCurent = transform.position;
            }

        }
        else
        {
            _playerOnPlace = false;
            _anchor.position = new Vector3(transform.position.x, _anchor.position.y, transform.position.z);
            _positionInited = false;
        }
    }

    public override void Run(bool isRun)
    {
        //base.Run(isRun);
    }
    public override void Crouch(bool isCrouch)
    {
        //Debug.Log($"isCrouch {isCrouch}");
        _isCrouch = isCrouch;
        //base.Crouch(isCrouch);
    }

    public override void Jump(bool isJump)
    {
        //Debug.Log($"isJump {isJump}");
        _isJump = isJump;
        //base.Jump(direction);
    }
}

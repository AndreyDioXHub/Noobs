using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField]
    private CharacterController _character;
    [SerializeField]
    private Camera _characterCamera;

    #region CharSetting
    [SerializeField]
    private float _standHeight = 1.8f;
    [SerializeField]
    private float _seatHeight = 1.35f;
    [SerializeField]
    private float _cameraStandPosition = 0.8f;
    [SerializeField]
    private float _cameraSeatPosition = 0.575f;
    #endregion


    #region Move
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private float _walkSpeed = 3;
    [SerializeField]
    private float _runSpeed = 5;
    [SerializeField]
    private float _crouchSpeed = 1;
    [SerializeField]
    private KeyCode _runKey = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode _crouchKey = KeyCode.LeftControl;
    private Vector3 _moveVector;
    private float x;
    private float z;
    #endregion

    #region Jump
    [SerializeField]
    private AnimationCurve _jumpHeightCurve;
    [SerializeField]
    private float _jumpDuration = 1;
    [SerializeField]
    private float _jumpHeight = 1.5f;
    [SerializeField]
    private KeyCode _jumpKey = KeyCode.Space;

    private float _expiratedTime = 0;

    private float _velosity;

    
    private bool _isJump = false;
    private bool _isGrounded = false;
    private bool _isRun = false;
    private bool _isCrouch = false;

    private bool _canStand = true;

    public int _standCollisions = 0;

    private float _progressCurFrame;
    private float _progressPrevFrame;
    #endregion

    private float _gravity = -9.8f;

    public CharacterController Controller
    {
        get => _character;
    }

    private void Awake()
    {
    }

    void Start()
    {
        
    }

    void Update()
    {
        _isGrounded = _character.isGrounded;

        #region Jump
        if (Input.GetKeyDown(_jumpKey))// && _isGrounded)
        {
            _isJump = true;
            _isCrouch = false;
            _expiratedTime = 0;
        }

        if (_isJump == true)
        {
            _expiratedTime += Time.deltaTime;

            if (_expiratedTime > _jumpDuration || _canStand == false)
            {
                _expiratedTime = 0;
                _isJump = false;
                _velosity = -0.002f;
            }
            else
            {
                _progressCurFrame = _expiratedTime / _jumpDuration;
                _velosity = (_jumpHeightCurve.Evaluate(_progressCurFrame) - _jumpHeightCurve.Evaluate(_progressPrevFrame)) * _jumpHeight;
                _progressPrevFrame = _progressCurFrame;
            }
        }
        else
        {
            _velosity = _velosity + _gravity * Time.deltaTime * Time.deltaTime;
        }

        if (_isGrounded && _isJump == false)
        {
            _velosity = -0.002f;
        }
        #endregion

        #region Move
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if(Input.GetKey(_runKey))
        {
            _isRun = true;
        }
        else 
        {
            _isRun = false;
        }

        if(Input.GetKeyDown(_crouchKey))
        {
            _isCrouch = true;
        }

        if(Input.GetKeyUp(_crouchKey))
        {
            _isCrouch = false;
        }
        #endregion

        #region SpeedSetting
        if(_isRun == true && _isCrouch == false && _canStand == true)
        {
            _speed = _runSpeed;
        }

        if(_isCrouch == true)
        {
            _speed = _crouchSpeed;
            _character.height = _seatHeight;
            StartCoroutine(SetCameraDown());
        }
        else
        {
            if (_canStand == true)
            {
                _character.height = _standHeight;
                StartCoroutine(SetCameraUp());
            }
        }

        if(_isRun == false && _isCrouch == false && _canStand == true)
        {
            _speed = _walkSpeed;
        }
        #endregion

        _moveVector = transform.right * x * _speed * Time.deltaTime + transform.forward * z * _speed * Time.deltaTime + new Vector3(0, _velosity, 0);
        _character.Move(_moveVector);
        
    }

    public void SetCanStand(int i)
    {
        _standCollisions+=i;

        if (_standCollisions > 0)
        {
            _canStand = false;
        }
        else
        {
            _canStand = true;
        }
           
    }

    private IEnumerator SetCameraDown()
    {
        StopCoroutine(SetCameraUp());
        float seatspeed = 1f;
        while(_characterCamera.transform.localPosition.y > _cameraSeatPosition)
        {
            yield return new WaitForEndOfFrame();
            _characterCamera.transform.localPosition -= new Vector3(0, seatspeed*Time.deltaTime, 0);
        }
        _characterCamera.transform.localPosition = new Vector3(0, _cameraSeatPosition, 0);
        StopCoroutine(SetCameraDown());
    }
    private IEnumerator SetCameraUp()
    {
        StopCoroutine(SetCameraDown());
        float seatspeed = 1f;
        while (_characterCamera.transform.localPosition.y < _cameraStandPosition)
        {
            yield return new WaitForEndOfFrame();
            _characterCamera.transform.localPosition += new Vector3(0, seatspeed * Time.deltaTime, 0);
        }
        _characterCamera.transform.localPosition = new Vector3(0, _cameraStandPosition, 0);
        StopCoroutine(SetCameraUp());
    }
}

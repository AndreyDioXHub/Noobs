using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _character;

    [SerializeField]
    private Transform _playerCamera;
    [SerializeField]
    private float _mouseSensitivity = 100f;
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _speedWalk = 3f;
    [SerializeField]
    private float _speedRun = 5f;

    [SerializeField]
    private Vector2 _minMaxAngle;
    private float _xRotation;

    private Vector3 _gravityVector;
    [SerializeField]
    private float _jumpheight = 2f;
    [SerializeField]
    private float _gravity = -9.8f;

    private Vector3 _resultMoveVector;
    private bool _isGrounded;


    void Start()
    {
        _speed = _speedWalk;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;     
        
        _xRotation -=mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        //_yRotation = Mathf.Clamp(_yRotation, _minMaxAngle.x, _minMaxAngle.y);
        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        float xMovement = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        float yMovement = Input.GetAxis("Vertical") * _speed * Time.deltaTime;

        Vector3 move = transform.right * xMovement + transform.forward * yMovement;

        if (_isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _gravityVector.y = Mathf.Sqrt(_jumpheight * -2f * _gravity) * Time.deltaTime;
            }
        }
        else
        {
            _gravityVector.y += _gravity * Time.deltaTime * Time.deltaTime;
        }

        

        if (Input.GetButton("Run"))
        {
            _speed = _speedRun;
        }
        else
        {
            _speed = _speedWalk;
        }

        _resultMoveVector = _gravityVector + move;// * _speed * Time.deltaTime;

        _character.Move(_resultMoveVector);
    }

    public void SetGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }
}

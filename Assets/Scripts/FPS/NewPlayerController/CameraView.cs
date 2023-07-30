using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    [SerializeField]
    protected Transform _playerCamera;

    [SerializeField]
    protected float _mouseSensitivity = 300f;
    [SerializeField]
    protected Vector2 _minMaxAngle = new Vector2(-90, 90);

    protected float _xRotation;
    [SerializeField]
    protected bool _lockMouse;


    public virtual void Start()
    {
        if (_lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public virtual void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}

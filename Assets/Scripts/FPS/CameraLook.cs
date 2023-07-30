using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _inputs;
    [SerializeField]
    private float _mouseSensitivity = 100f;
    [SerializeField]
    private Transform _playerBody;
    [SerializeField]
    private Vector2 _minMaxAngle;
    [SerializeField]
    private Vector2 _mouse;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _xRotation -= _mouse.y * _mouseSensitivity * Time.deltaTime;
        _yRotation = _mouse.x * _mouseSensitivity * Time.deltaTime;

        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerBody.Rotate(Vector3.up * _yRotation);

        /*float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);*/
    }

    public void OnMouseLookX(InputAction.CallbackContext value)
    {
        _mouse.x = value.ReadValue<float>();
        //Debug.Log(value);
    }

    public void OnMouseLookY(InputAction.CallbackContext value)
    {
        _mouse.y = value.ReadValue<float>();
        // Debug.Log(value);
    }
}

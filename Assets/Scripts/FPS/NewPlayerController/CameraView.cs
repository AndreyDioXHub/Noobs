using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraView : MonoBehaviour
{
    [SerializeField]
    protected Transform _playerCamera;

    [SerializeField]
    protected float _mouseSensitivity = 300f;
    [SerializeField]
    protected float _mouseSensitivityBase = 300f;
    [SerializeField]
    protected Vector2 _minMaxAngle = new Vector2(-90, 90);

    protected float _xRotation;
    [SerializeField]
    protected bool _lockMouse;
    [SerializeField]
    protected CursorLockMode _defaultCursorState;

    private Vector2 _axisLook;
    private bool _cursorLocked = true;

    public bool LookAround;

    private float _mouseX, _mouseY = 0;


    public virtual void Start()
    {
        if (_lockMouse)
        {
            _defaultCursorState = Cursor.lockState;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void SetSensitivity(float sliderSensitivityValue)
    {
        _mouseSensitivity = sliderSensitivityValue * _mouseSensitivityBase;
    }
    public void SetViewParam(bool lockMouse, float mouseSensitivity = 600)
    {
        _lockMouse = lockMouse;
        _mouseSensitivityBase = mouseSensitivity;
    }

    public virtual void Update()
    {

        bool adsView = false;
        /*
        if (AdsButtonView.Instance != null)
        {
            adsView = AdsButtonView.Instance.Parent.activeSelf;
        }

        if (SettingScreen.Instance.gameObject.activeSelf || AdsScreen.Instance.gameObject.activeSelf || adsView || CheckPointManager.Instance.IsWin)
        {
            Cursor.lockState = _defaultCursorState;
            return;
        }*/

        //Debug.Log($"{Input.GetAxis("Mouse X")} { Input.GetAxis("Mouse Y")}");

        if (_lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            /*
            if (BlockCountManager.Instance.BlocksCount > 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = _defaultCursorState;
                return;
            }*/

        }




        float mouseX = 0;
        float mouseY = 0;

        if (LookAround)
        {
            _mouseX++;
            _mouseY++;

            mouseX = _mouseX * _mouseSensitivity * Time.deltaTime;
            mouseY = _mouseY * _mouseSensitivity * Time.deltaTime;
        }
        else
        {
            /*mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;*/

            mouseX = _axisLook.x * _mouseSensitivity * Time.deltaTime;
            mouseY = _axisLook.y * _mouseSensitivity * Time.deltaTime;
        }

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Read.
        _axisLook = _cursorLocked ? context.ReadValue<Vector2>() : default;
    }
}

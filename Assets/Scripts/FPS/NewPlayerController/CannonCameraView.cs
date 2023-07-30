using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCameraView : CameraView
{
    [SerializeField]
    private float _activationAngle;

    [SerializeField]
    private float _timeTransition = 2f;
    [SerializeField]
    private float _timeTransitionCur;


    private void OnEnable()
    {
        _activationAngle = _playerCamera.localEulerAngles.x;
    }

    public override void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _timeTransitionCur += Time.deltaTime;
        _timeTransitionCur = _timeTransitionCur > _timeTransition ? _timeTransition : _timeTransitionCur;

        _playerCamera.localRotation = Quaternion.Euler(_activationAngle - _activationAngle * _timeTransitionCur / _timeTransition, 0f, 0f);

        /*_xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);*/

        transform.Rotate(Vector3.up * mouseX);
    }
}

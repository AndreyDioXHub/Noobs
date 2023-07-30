using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaterView : CameraView
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        /*_xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);*/

        transform.Rotate(Vector3.up * mouseX);
        _playerCamera.Rotate(Vector3.left * mouseY);
    }
}

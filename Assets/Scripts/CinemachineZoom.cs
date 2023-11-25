using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineZoom : MonoBehaviour
{
    CinemachineVirtualCamera cinemaCamera;
    //CinemachineComponentBase componentBase;
    CinemachineFramingTransposer componentBase;
    [SerializeField]
    Vector2 zoomBound;
    [SerializeField]
    private Vector2 zoomOffset = Vector2.zero;
    [SerializeField]
    private Vector2 zoomHeight = Vector2.zero;
    
    private float mouseDelta = 0;
    [SerializeField]
    private bool invertMouse = false;
    private int invertvalue = 1;
    [SerializeField]
    private float zoomSpeed = 0.001f;

    bool _isMobile = false;

    // Start is called before the first frame update
    void Awake() {
        cinemaCamera = GetComponent<CinemachineVirtualCamera>();
        componentBase = cinemaCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if(invertMouse) {
            invertvalue = -1;
        }
        
    }

    public void InitMobile(bool mobile) {
        _isMobile = mobile;
    }

    private void Update() {
        if(mouseDelta != 0) {
            float dist = componentBase.m_CameraDistance;
            dist = Mathf.Clamp(dist+mouseDelta, zoomBound.x, zoomBound.y);
            componentBase.m_CameraDistance = dist;
            mouseDelta = 0;
            //Offset
            float t = (dist - zoomBound.x) / (zoomBound.y - zoomBound.x);
            float offsetx = zoomOffset.x + (zoomOffset.y - zoomOffset.x) * t;
            componentBase.m_TrackedObjectOffset.x = Mathf.Clamp(offsetx, zoomOffset.x, zoomOffset.y);
            float hoffset = zoomHeight.x + (zoomHeight.y - zoomHeight.x) * t;
            componentBase.m_TrackedObjectOffset.y = Mathf.Clamp(hoffset, zoomHeight.x, zoomHeight.y);

        }
    }

    public void OnZoom(InputAction.CallbackContext context) {
        if(_isMobile) {
            return;
        }
        mouseDelta += context.ReadValue<Vector2>().y * zoomSpeed * invertvalue;
        
    }
}

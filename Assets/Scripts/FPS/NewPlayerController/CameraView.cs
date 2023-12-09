using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Video;

public class CameraView : MonoBehaviour
{
    public static CameraView Instance;

    [SerializeField]
    protected Transform _playerCamera;
    [SerializeField]
    protected List<Renderer> _renderers = new List<Renderer>();


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

    [SerializeField]
    private GameObject CamNode1;
    [SerializeField]
    private GameObject CameraFVP;
    [SerializeField]
    private GameObject CameraThrdVP;

    CinemachineVirtualCameraBase vcam;

    public int PriorityBoostAmount = 10;
    public ViewState VState { get; private set; } = ViewState.TrdPS;

    bool _isMoving = false;
    [SerializeField]
    Transform model;
    float _sliderSensitivityValue;
    private void Awake()
    {
        Instance = this;
    }

    public virtual void Start()
    {
        if (_lockMouse)
        {
            _defaultCursorState = Cursor.lockState;
            Cursor.lockState = CursorLockMode.Locked;
        }

        SetSensitivity(MouseSensitivityManager.Instance.SliderValue);
        CamNode1 = Camera.main.transform.gameObject;
    }

    public void Init(GameObject cameraFVP, GameObject cameraThrdVP)
    {
        CameraFVP = cameraFVP;
        CameraThrdVP = cameraThrdVP;
        vcam = CameraFVP.GetComponent<CinemachineVirtualCameraBase>();
        CameraFVP.GetComponent<CinemachineRotation>().SetSensitivity(_sliderSensitivityValue);
        CameraThrdVP.GetComponent<CinemachineRotation>().SetSensitivity(_sliderSensitivityValue);
        Set3rdView();
    }

    public void SetSensitivity(float sliderSensitivityValue)
    {
        _mouseSensitivity = sliderSensitivityValue * _mouseSensitivityBase;

        if(CameraFVP == null || CameraThrdVP == null)
        {
            _sliderSensitivityValue = sliderSensitivityValue;
        }
        else
        {
            CameraFVP.GetComponent<CinemachineRotation>().SetSensitivity(sliderSensitivityValue);
            CameraThrdVP.GetComponent<CinemachineRotation>().SetSensitivity(sliderSensitivityValue);
        }
    }


    public void SetViewParam(bool lockMouse, float mouseSensitivity = 600)
    {
        _lockMouse = lockMouse;
        _mouseSensitivityBase = mouseSensitivity;
    }

    public void Update()
    {
        if (AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin || SettingScreen.IsActive || CamNode1 == null 
            || AdsManager.AdsPlaying)// || TutorialCanvas.Instance.gameObject.activeSelf)// || BlockCountManager.Instance.BlocksCount == 0)
        {
            Cursor.lockState = _defaultCursorState;
            return;
        }

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


        Vector3 r = transform.rotation.eulerAngles;
        r.y = CamNode1.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(r);
        /*
        if (!_isMoving && model != null)
        {
            r.y = 180 - CamNode1.transform.rotation.eulerAngles.y;
            model.rotation = Quaternion.Euler(r);
        }*/

        if (Vector3.Distance(_playerCamera.position, CamNode1.transform.position) < 1f)
        {
            foreach (var rend in _renderers)
            {
                rend.enabled = false;
            }
        }
        else
        {
            foreach (var rend in _renderers)
            {
                rend.enabled = true;
            }
        }

        return;
    }

    public void SetModel(Transform model)
    {
        this.model = model;
        _renderers.Clear();
        _renderers = new List<Renderer>();
        //_renderers = this.model.GetComponent<SkinRenderer>().Renderers;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Read.
        _axisLook = _cursorLocked ? context.ReadValue<Vector2>() : default;
        //Debug.Log(_axisLook);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _isMoving = true;
                break;
            case InputActionPhase.Canceled:
                _isMoving = false;
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Waiting:
                break;
        }


    }

    [ContextMenu("FPS")]
    public void SetFPSView()
    {
        if (vcam.Priority < PriorityBoostAmount)
        {
            vcam.Priority += PriorityBoostAmount;
        }
        VState = ViewState.FPS;
    }

    [ContextMenu("3rdPS")]
    public void Set3rdView()
    {
        if (vcam.Priority > PriorityBoostAmount)
        {
            vcam.Priority -= PriorityBoostAmount;
        }
        VState = ViewState.TrdPS;
    }

    public void SwitchView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && VState != ViewState.Transition)
        {
            if (VState == ViewState.TrdPS)
            {
                SetFPSView();
            }
            else if (VState == ViewState.FPS)
            {
                Set3rdView();
            }
        }
    }

    public enum ViewState
    {
        Transition = 0,
        FPS = 1,
        TrdPS = 2,
    }
}

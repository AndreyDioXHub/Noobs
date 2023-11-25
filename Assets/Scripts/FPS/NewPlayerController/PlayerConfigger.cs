using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigger : MonoBehaviour
{
    [SerializeField]
    private RobloxController _controller;

    [SerializeField]
    private GameObject TPSPrefab;
    [SerializeField]
    private GameObject FPSPrefab;
    [SerializeField]
    private Transform _cameraCenterTPS;
    [SerializeField]
    private Transform _cameraCenterFPS;
    [SerializeField]
    private GameObject TPS;
    [SerializeField]
    private GameObject FPS;
    [SerializeField]
    private CameraView _cameraView;
    [SerializeField]
    private PlayerInput _inputs;
    [SerializeField]
    InputActionReference PCLook;
    [SerializeField]
    InputActionReference MobileLook;


    void Start()
    {
        MouseSensitivityManager.Instance.Init(_cameraView);
        TPS = Instantiate(TPSPrefab);
        TPS.GetComponent<CinemachineVirtualCamera>().Follow = _cameraCenterTPS;
        FPS = Instantiate(FPSPrefab);
        FPS.GetComponent<CinemachineVirtualCamera>().Follow = _cameraCenterFPS;
        _cameraView.Init(FPS, TPS);
        InputSchemeSwitcher.Instance.Init(FPS.GetComponent<CinemachineInputProvider>(), 
            TPS.GetComponent<CinemachineInputProvider>(),
            _inputs,
            _cameraView,
            PCLook,
            MobileLook);
        InputSchemeSwitcher.Instance.RequestingEnvironmentData();
        CheckPointManager.Instance.Init(transform);
        _controller.OnEscDown.AddListener(SettingScreen.Instance.SwitchScreenState);
    }


    void Update()
    {
        
    }
}

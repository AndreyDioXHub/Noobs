using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class InputSchemeSwitcher : MonoBehaviour
{
    [SerializeField]
    private bool _isMobileDebug;
    [SerializeField]
    private PlayerInput _inputs;
    [SerializeField]
    private CameraView _cameraView;
    [SerializeField]
    private GameObject _mobileInputCanvas;
    [SerializeField]
    private SettingScreen _setting;

    [SerializeField]
    CinemachineZoom _zoom;

    [SerializeField]
    CinemachineInputProvider FPSInput;
    [SerializeField]
    CinemachineInputProvider TPSInput;
    [SerializeField]
    InputActionReference PCLook;
    [SerializeField]
    InputActionReference MobileLook;


    void Start()
    {
        //_inputs.SwitchCurrentActionMap("PlayerMobile");
    }

    void Update()
    {

    }

    public void RequestingEnvironmentData()
    {
        //string device = YandexGame.EnvironmentData.;

        if (_isMobileDebug)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            _setting.SetIsMobile(true);
            _zoom.InitMobile(true);
            FPSInput.XYAxis = MobileLook;
            TPSInput.XYAxis = MobileLook;
            //SaveManager.Instance.SwitchAutoSave();
            return;
        }

        if (YandexGame.EnvironmentData.isDesktop)
        {
            _inputs.SwitchCurrentActionMap("Player");
            _cameraView.SetViewParam(true, 300);
            _mobileInputCanvas.SetActive(false);
            _setting.SetIsMobile(false);
            _zoom.InitMobile(false);
            FPSInput.XYAxis = PCLook;
            TPSInput.XYAxis = PCLook;
            return;
        }

        if (YandexGame.EnvironmentData.isMobile)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            _setting.SetIsMobile(true);
            _zoom.InitMobile(true);
            FPSInput.XYAxis = MobileLook;
            TPSInput.XYAxis = MobileLook;
            //SaveManager.Instance.SwitchAutoSave();
            return;
        }

        if (YandexGame.EnvironmentData.isTablet)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            _setting.SetIsMobile(true);
            _zoom.InitMobile(true);
            FPSInput.XYAxis = MobileLook;
            TPSInput.XYAxis = MobileLook;
            //SaveManager.Instance.SwitchAutoSave();
            return;
        }

    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class InputSchemeSwitcher : MonoBehaviour
{
    public static InputSchemeSwitcher Instance;

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
    CinemachineInputProvider FPSInput;
    [SerializeField]
    CinemachineInputProvider TPSInput;
    [SerializeField]
    InputActionReference PCLook;
    [SerializeField]
    InputActionReference MobileLook;

    [SerializeField]
    private MeshRenderer _ocean;
    [SerializeField]
    private Material _oceanMTLPC;
    [SerializeField]
    private Material _oceanMTLM;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //_inputs.SwitchCurrentActionMap("PlayerMobile");
    }

    void Update()
    {

    }

    public void Init(CinemachineInputProvider fpsInput , CinemachineInputProvider tpsInput, PlayerInput inputs, CameraView cameraView, InputActionReference pcLook, InputActionReference mobileLook)
    {
        FPSInput = fpsInput;
        TPSInput = tpsInput;
        _inputs = inputs;
        _cameraView = cameraView;
        PCLook = pcLook;
        MobileLook = mobileLook;
    }

    private bool AllFieldsNotNull()
    {
        if (_inputs == null)
        {
            return false;
        }
        else
        {
            if(_cameraView == null)
            {
                return false;
            }
            else
            {
                if (_mobileInputCanvas == null)
                {
                    return false;
                }
                else
                {
                    if (_setting == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (FPSInput == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (TPSInput == null)
                            {
                                return false;
                            }
                            else
                            {
                                if (PCLook == null)
                                {
                                    return false;
                                }
                                else
                                {
                                    if (MobileLook == null)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        if (_ocean == null)
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            if (_oceanMTLPC == null)
                                            {
                                                return false;
                                            }
                                            else
                                            {
                                                if (_oceanMTLM == null)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void RequestingEnvironmentData()
    {
        if (!AllFieldsNotNull())
        {
            Debug.Log("return");
            return;
        }

        //string device = YandexGame.EnvironmentData.;

        if (_isMobileDebug)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            _setting.SetIsMobile(true);
            //_zoom.InitMobile(true);
            FPSInput.XYAxis = MobileLook;
            TPSInput.XYAxis = MobileLook;

            if(ChatTexts.Instance != null)
            {
                ChatTexts.Instance.SetMobile();
            }

            //SaveManager.Instance.SwitchAutoSave();
            _ocean.material = _oceanMTLM;
            return;
        }

        if (YandexGame.EnvironmentData.isDesktop)
        {
            _inputs.SwitchCurrentActionMap("Player");
            _cameraView.SetViewParam(true, 300);
            _mobileInputCanvas.SetActive(false);
            _setting.SetIsMobile(false);
            //_zoom.InitMobile(false);
            FPSInput.XYAxis = PCLook;
            TPSInput.XYAxis = PCLook;
            if (ChatTexts.Instance != null)
            {
                ChatTexts.Instance.IsMobile = false;
            }
            _ocean.material = _oceanMTLPC;
            return;
        }

        if (YandexGame.EnvironmentData.isMobile)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            _setting.SetIsMobile(true);
            //_zoom.InitMobile(true);
            FPSInput.XYAxis = MobileLook;
            TPSInput.XYAxis = MobileLook;
            if (ChatTexts.Instance != null)
            {
                ChatTexts.Instance.SetMobile();
            }
            _ocean.material = _oceanMTLM;
            //SaveManager.Instance.SwitchAutoSave();
            return;
        }

        if (YandexGame.EnvironmentData.isTablet)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            _setting.SetIsMobile(true);
            //_zoom.InitMobile(true);
            FPSInput.XYAxis = MobileLook;
            TPSInput.XYAxis = MobileLook;
            if (ChatTexts.Instance != null)
            {
                ChatTexts.Instance.SetMobile();
            }
            _ocean.material = _oceanMTLM;
            //SaveManager.Instance.SwitchAutoSave();
            return;
        }

    }
}

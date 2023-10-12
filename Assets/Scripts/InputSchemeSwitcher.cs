using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class InputSchemeSwitcher : MonoBehaviour
{

    [SerializeField]
    private PlayerInput _inputs;
    [SerializeField]
    private CameraView _cameraView;
    [SerializeField]
    private GameObject _mobileInputCanvas;
    /*
    [SerializeField]
    private SettingScreen _setting;*/
    /*
    [SerializeField]
    private GameObject _switchButtonJ;
    [SerializeField]
    private GameObject _switchButtonK;*/

    void Start()
    {
        if (YandexGame.Instance != null)
        {
            RequestingEnvironmentData();
        }

        //_inputs.SwitchCurrentActionMap("PlayerMobile");
    }

    void Update()
    {
        
    }

    public void RequestingEnvironmentData()
    {
        //string device = YandexGame.EnvironmentData.;
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _inputs.SwitchCurrentActionMap("Player");
            _cameraView.SetViewParam(true, 300);
            _mobileInputCanvas.SetActive(false);
            //_setting.SetIsMobile(false);
            /*
            _switchButtonJ.SetActive(false);
            _switchButtonK.SetActive(false);*/
            return;
        }

        if (YandexGame.EnvironmentData.isMobile)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            //_setting.SetIsMobile(true);
            /*
            _switchButtonJ.SetActive(true);
            _switchButtonK.SetActive(true);*/
            return;
        }

        if (YandexGame.EnvironmentData.isTablet)
        {
            _inputs.SwitchCurrentActionMap("PlayerMobile");
            _cameraView.SetViewParam(false, 150);
            _mobileInputCanvas.SetActive(true);
            //_setting.SetIsMobile(true);
            /*
            _switchButtonJ.SetActive(true);
            _switchButtonK.SetActive(true);*/
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScreen : MonoBehaviour
{


    public static bool IsActive
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            else
            {
                return Instance.gameObject.activeSelf;
            }
        }
    }

    public static SettingScreen Instance;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private GameObject _escButton;
    [SerializeField]
    private float _time = 0.5f, _timeCur;
    [SerializeField]
    private bool _isMobile;
    /*
    [SerializeField]
    private GameObject _mobileInputCanvas;*/


    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (AdsScreen.Instance.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }

        _timeCur += Time.deltaTime;

        if(_timeCur > _time && _isMobile)
        {
            _escButton.SetActive(true);
        }
    }

    public void SetIsMobile(bool isMobile)
    {
        _isMobile = isMobile;
    }

    public void SwitchScreenState()
    {
        if (ChatTexts.IsActive)
        {
            return;
        }

        gameObject.SetActive(!gameObject.activeSelf);

        if (_camera != null)
        {
            _camera.gameObject.SetActive(gameObject.activeSelf);
        }
    }

    private void OnEnable()
    {/*
        if(_isMobile && _mobileInputCanvas != null)
        {
            _mobileInputCanvas.SetActive(false);
        }*/
    }

    private void OnDisable()
    {
        //PlayerPrefs.Save();
        _escButton.SetActive(false);
        _timeCur = 0;
        /*
        if (_isMobile && _mobileInputCanvas != null)
        {
            _mobileInputCanvas.SetActive(true);
        }*/
    }
}

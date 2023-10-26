using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScreen : MonoBehaviour
{
    public static SettingScreen Instance;

    [SerializeField]
    private GameObject _escButton;
    [SerializeField]
    private float _time = 0.5f, _timeCur;
    [SerializeField]
    private bool _isMobile;

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

        bool adsView = false;

        if (AdsButtonView.Instance != null)
        {
            adsView = AdsButtonView.Instance.Parent.activeSelf;
        }

        if (AdsScreen.Instance.gameObject.activeSelf || adsView || CheckPointManager.Instance.IsWin)// || BlockCountManager.Instance.BlocksCount == 0)
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
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        //PlayerPrefs.Save();
        _escButton.SetActive(false);
        _timeCur = 0;
    }
}

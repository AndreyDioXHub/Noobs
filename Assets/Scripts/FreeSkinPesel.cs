using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSkinPesel : MonoBehaviour
{
    [SerializeField]
    private bool _isSetup;

    [SerializeField]
    private GameObject _activateButton;
    [SerializeField]
    private GameObject _screen;

    void Start()
    {
        _isSetup = _isSetup || PlayerPrefs.GetInt("freeskin", 0) == 1;

        if (_isSetup)
        {
            _activateButton.SetActive(false);
            _screen.SetActive(false);
        }
    }

    public void HasPesel()
    {
        _isSetup = true;

        if (_isSetup)
        {
            _activateButton.SetActive(false);
            _screen.SetActive(false);
            PlayerPrefs.SetInt("freeskin", 1);
        }
    }

    void Update()
    {
        
    }

    public void ShowReward()
    {
        _isSetup = true;
        AdsManager.Instance.ShowRewardedAd();
    }


    public void OnReward()
    {
        if (_isSetup)
        {
            _activateButton.SetActive(false);
            _screen.SetActive(false);
            SkinManager.Instance.EquipPesel();
            PlayerPrefs.SetInt("freeskin", 1);
        }
    }
}

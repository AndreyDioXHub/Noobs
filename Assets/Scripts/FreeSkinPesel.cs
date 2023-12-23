using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreeSkinPesel : MonoBehaviour
{
    [SerializeField]
    private bool _isSetup;

    [SerializeField]
    private GameObject _activateButton;
    [SerializeField]
    private GameObject _screen;
    private bool _bool = false;

    void Start()
    {
        if (_bool)
        {
            _activateButton.SetActive(false);
        }

        StartCoroutine(OnSceeneLoadedCoroutine());

    }

    IEnumerator OnSceeneLoadedCoroutine()
    {
        if (SceneManager.GetActiveScene().name.Equals("NoobLevelObstacleCourseNetwork"))
        {
            while (RobloxController.Instance == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
            Load();
        }
        else
        {
            Load();
        }
    }

    public void Load()
    {
        PlayerSave.Instance.ExecuteMyDelegateInQueue(GetLoad);
    }

    public void GetLoad()
    {
        _isSetup = _isSetup || PlayerSave.Instance.progress.freeskin == 1;

        if (_isSetup)
        {
            _activateButton.SetActive(false);
            _screen.SetActive(false);
        }

        if (_bool)
        {
            _activateButton.SetActive(false);
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
        if (_bool)
        {
            _activateButton.SetActive(false);
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


    public void OnReward(int count, string key)
    {
        if (key.Equals("Pesel"))
        {
            _activateButton.SetActive(false);
            _screen.SetActive(false);
            SkinManager.Instance.EquipPesel();
            PlayerPrefs.SetInt("freeskin", 1);
        }
        /*
        if (_isSetup)
        {
        }*/
    }
}

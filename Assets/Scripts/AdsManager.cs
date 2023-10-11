using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YG;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    public UnityEvent OnReward;

    [SerializeField]
    private InfoYG _infoYG;

    [SerializeField]
    private YandexGame _sdk;

    public bool IsPaused { get; private set; }
    public string TimeString { get
        {
            string result = "";

            if (_timeCur < 10)
            {
                result = $"0{ (int)_timeCur }/60";
            }
            else
            {
                result = $"{ (int)_timeCur }/60";
            }
            //_timeCur.ToString(".#");
            /*
            double t = Math.Round(_timeCur, 2);
            int timeD = (int)t;
            string timestring = $"{timeD}:{t - timeD}";*/
            return result;
        }
    }

    [SerializeField]
    private float _time, _timeCur, _timeBig, _timeBigCur;
    [SerializeField]
    private bool _isWasFullScreen;

  private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        //_time = _infoYG.fullscreenAdInterval; 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("ads"))
        {
            _sdk._FullscreenShow();
        }
        //do stuff
    }

    public void ShowRewardedAd()
    {
        //_sdk._FullscreenShow();
        _sdk._RewardedShow(1);
    }

    public void ShowFullscreen()
    {
        _sdk._FullscreenShow();
        /*
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            _sdk._RewardedShow(1);
        }
        else
        {
        }*/
        _isWasFullScreen = true;
    }

    public void GetReward()
    {
        if (_isWasFullScreen)
        {
            _isWasFullScreen = false;
            //return;
        }

        OnReward?.Invoke();
        //BlockCountManager.Instance.ResetBlockCount();
        IsPaused = true;
    }

    void Update()
    {
        if (IsPaused)
        {
            _timeCur += Time.deltaTime;

            if(_timeCur > _time)
            {
                _timeCur = 0;
                IsPaused = false;
            }
        }

        if (_timeBig > 0)
        {
            _timeBigCur += Time.deltaTime;

            if (_timeBigCur > _timeBig)
            {
                _timeBigCur = 0;
                if (AdsScreen.Instance != null)
                {
                    AdsScreen.Instance.gameObject.SetActive(true);
                }
            }
        }
    }
}

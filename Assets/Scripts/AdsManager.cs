using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    public static bool AdsPlaying
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }

            return Instance._adsPlaying;
        }
    }


    //public UnityEvent OnReward;
    public UnityEvent OnAdsSwitched;

    public UnityEvent OnLoadStarted = new UnityEvent();
    public UnityEvent OnFailedToLoad = new UnityEvent();
    public UnityEvent OnLoadComplete = new UnityEvent();
    public UnityEvent OnImpression = new UnityEvent();
    public UnityEvent OnDismissed = new UnityEvent();
    public UnityEvent<bool> OnShow = new UnityEvent<bool>();
    public UnityEvent OnFailedToShow = new UnityEvent();
    public UnityEvent OnClicked = new UnityEvent();
    public UnityEvent<int, string> OnReward = new UnityEvent<int, string>();

    /*
    [SerializeField]
    private InfoYG _infoYG;

    [SerializeField]
    private YandexGame _sdk;*/

    public bool IsPaused { get; private set; }
    public string TimeString { get
        {
            string result = "";

            if (_timeRewardedCur < 10)
            {
                result = $"0{ (int)_timeRewardedCur }/60";
            }
            else
            {
                result = $"{ (int)_timeRewardedCur }/60";
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
    private float _timeRewarded, _timeRewardedCur, _timeBetweenCheckpoints, _timeBetweenCheckpointsCur, _timeFullScreen, _timeFullScreenCur;
    [SerializeField]
    private GameObject _adsScreen;
    [SerializeField]
    private bool _adsPlaying;
    [SerializeField]
    private bool _isReward;

    private void Awake()
    {
        Instance = this;
        /*
        if (Instance == null)
        {
        }
        else
        {
            Destroy(this);
        }*/
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    void Start()
    {
        //ShowFullscreen();
        //_time = _infoYG.fullscreenAdInterval; 
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //_sdk._FullscreenShow();
        /*
        if (scene.name.Equals("ads"))
        {
            _sdk._FullscreenShow();
        }*/
        //do stuff
    }

    public void ShowRewardedAd()
    {
        //_sdk._FullscreenShow();
        if(_adsScreen != null)
        {
            _adsScreen.SetActive(true);
            _adsPlaying = true;
        }

        //_sdk._RewardedShow(1);
    }

    public void ShowRewardedAd(string key)
    {
        //_sdk._FullscreenShow();

        _isReward = true;

        if (_adsScreen != null)
        {
            _adsScreen.SetActive(true);
            _adsPlaying = true;
        }
        YandexAdsRewardedAd.Instance.RequestRewardedAd(key);
        //_sdk._RewardedShow(1);
    }



    [ContextMenu("ShowFullscreen")]
    public void ShowFullscreen()
    {
        if (_adsScreen != null)
        {
            _adsScreen.SetActive(true);
            _adsPlaying = true;
        }

        //_sdk._FullscreenShow();
        /*
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            _sdk._RewardedShow(1);
        }
        else
        {
        }*/
    }

    public void AdsWasShowed()
    {
        if (_adsScreen != null)
        {
            OnAdsSwitched?.Invoke();
            _adsScreen.SetActive(false);
            _adsPlaying = false;
        }

    }

    public void ShowFullscreenButton()
    {
        if(_timeBetweenCheckpointsCur == _timeBetweenCheckpoints)
        {
            _timeBetweenCheckpointsCur = 0;
            if (_adsScreen != null)
            {
                OnAdsSwitched?.Invoke();
                _adsScreen.SetActive(true);
                _adsPlaying = true;
            }
            //_sdk._FullscreenShow();
        }
    }

    public void GetReward()
    {
        //OnReward?.Invoke();
        //BlockCountManager.Instance.ResetBlockCount();
        Debug.Log($"YandexMobileAds: {_isReward}");
        if (_isReward)
        {
            IsPaused = true;
            _isReward = false;
        }
        Debug.Log($"YandexMobileAds: {_isReward} {IsPaused}");
    }

    void Update()
    {
        if (IsPaused)
        {
            _timeRewardedCur += Time.deltaTime;

            if(_timeRewardedCur > _timeRewarded)
            {
                _timeRewardedCur = 0;
                IsPaused = false;
            }
        }

        if (_timeFullScreen > 0)
        {
            _timeFullScreenCur += Time.deltaTime;

            if (_timeFullScreenCur > _timeFullScreen)
            {
                _timeFullScreenCur = 0;

                if (AdsScreen.Instance != null)
                {
                    AdsScreen.Instance.gameObject.SetActive(true);
                }
            }
        }

        if (SettingScreen.IsActive)
        {
            if (_adsScreen != null)
            {
                _adsScreen.SetActive(false);
            }
        }
        else
        {
            if (_adsScreen != null)
            {
                _adsScreen.SetActive(_adsPlaying);
            }
        }

        _timeBetweenCheckpointsCur += Time.deltaTime;
        _timeBetweenCheckpointsCur = _timeBetweenCheckpointsCur > _timeBetweenCheckpoints ? _timeBetweenCheckpoints : _timeBetweenCheckpointsCur;

    }
}

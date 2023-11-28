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
        _sdk._RewardedShow(1);
    }

    [ContextMenu("ShowFullscreen")]
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
    }

    public void ShowFullscreenButton()
    {
        if(_timeBetweenCheckpointsCur == _timeBetweenCheckpoints)
        {
            _timeBetweenCheckpointsCur = 0;
            _sdk._FullscreenShow();
        }
    }

    public void GetReward()
    {
        OnReward?.Invoke();
        //BlockCountManager.Instance.ResetBlockCount();
        IsPaused = true;
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

        _timeBetweenCheckpointsCur += Time.deltaTime;
        _timeBetweenCheckpointsCur = _timeBetweenCheckpointsCur > _timeBetweenCheckpoints ? _timeBetweenCheckpoints : _timeBetweenCheckpointsCur;

    }
}

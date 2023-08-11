using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class PlatformAdsManager : MonoBehaviour
{
    public static PlatformAdsManager Instance;

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
    private float _time;
    [SerializeField]
    private float _timeCur;

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
        _time = _infoYG.fullscreenAdInterval;
    }

    public void ShowRewardedAd()
    {
        //_sdk._FullscreenShow();
        _sdk._RewardedShow(1);
    }

    public void GetReward()
    {
        OnReward?.Invoke();
        CoinManagerStart.Instance.AddCoins(100);
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
    }
}

using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyTargetReward : MonoBehaviour
{
    [SerializeField]
    private UInt32 _slotId = 1474902;

    [SerializeField]
    private TMP_Text _viewText;

    private InterstitialAd _interstitialAd;

    private InterstitialAd CreateInterstitialAd()
    {
        // Включение режима отладки
        MyTargetManager.DebugMode = true;

        // Создаем экземпляр InterstitialAd
        return new InterstitialAd(_slotId);
    }


    private void InitAd()
    {
        _viewText.text = "InitAd";
        // Создаем экземпляр InterstitialAd
        _interstitialAd = CreateInterstitialAd();
        // Устанавливаем обработчики событий
        _interstitialAd.AdLoadCompleted += OnLoadCompleted;
        _interstitialAd.AdDisplayed += OnAdDisplayed;
        _interstitialAd.AdDismissed += OnAdDismissed;
        _interstitialAd.AdVideoCompleted += OnAdVideoCompleted;
        _interstitialAd.AdClicked += OnAdClicked;
        _interstitialAd.AdLoadFailed += OnAdLoadFailed;

        _viewText.text = "Load";
        // Запускаем загрузку данных
        _interstitialAd.Load();
    }

    private void OnLoadCompleted(System.Object sender, EventArgs e)
    {
        _viewText.text = "OnLoadCompleted";
        _interstitialAd.Show();
    }
    private void OnAdDisplayed(System.Object sender, EventArgs e)
    {

        _viewText.text = "OnAdDisplayed";
    }

    private void OnAdDismissed(System.Object sender, EventArgs e)
    {
        _viewText.text = "OnAdDismissed";

    }

    private void OnAdVideoCompleted(System.Object sender, EventArgs e)
    {

        _viewText.text = "OnAdVideoCompleted";
    }

    private void OnAdClicked(System.Object sender, EventArgs e)
    {

        _viewText.text = "OnAdClicked";
    }

    private void OnAdLoadFailed(System.Object sender, ErrorEventArgs e)
    {
        _viewText.text = "OnAdLoadFailed: " + e.Message;
        Debug.Log("OnAdLoadFailed: " + e.Message);
    }

    void Start()
    {
        InitAd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexAdsInterstitial : MonoBehaviour
{
    [SerializeField]
    private string _adUnitId = "R-M-4609579-3";

    private String message = "";

    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;

    public void Awake()
    {
        this.interstitialAdLoader = new InterstitialAdLoader();
        this.interstitialAdLoader.OnAdLoaded += this.HandleAdLoaded;
        this.interstitialAdLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
    }
/*
    public void OnGUI()
    {
        var fontSize = (int)(0.05f * Math.Min(Screen.width, Screen.height));

        var labelStyle = GUI.skin.GetStyle("label");
        labelStyle.fontSize = fontSize;

        var buttonStyle = GUI.skin.GetStyle("button");
        buttonStyle.fontSize = fontSize;

#if UNITY_EDITOR
        this.message = "Mobile ads SDK is not available in editor. Only Android and iOS environments are supported";
#else
            if (GUILayout.Button("Request Interstitial", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
            {
                this.RequestInterstitial();
            }

            if (this.interstitial != null)
            {
                if (GUILayout.Button("Show Interstitial", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
                {
                    this.ShowInterstitial();
                }
            }
            if(this.interstitial != null)
            {
                if (GUILayout.Button("Destroy Interstitial", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
                {
                    this.interstitial.Destroy();
                }
            }
#endif

        GUILayout.Label(this.message, labelStyle);
    }
*/
    public void RequestInterstitial()
    {
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        // Replace demo Unit ID 'demo-interstitial-yandex' with actual Ad Unit ID
        string adUnitId = _adUnitId;//  demo-interstitial-yandex";

        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        this.interstitialAdLoader.LoadAd(this.CreateAdRequest(adUnitId));
        this.DisplayMessage("Interstitial is requested");
    }

    private void ShowInterstitial()
    {
        if (this.interstitial == null)
        {
            this.DisplayMessage("Interstitial is not ready yet");
            return;
        }

        this.interstitial.OnAdClicked += this.HandleAdClicked;
        this.interstitial.OnAdShown += this.HandleAdShown;
        this.interstitial.OnAdFailedToShow += this.HandleAdFailedToShow;
        this.interstitial.OnAdImpression += this.HandleImpression;
        this.interstitial.OnAdDismissed += this.HandleAdDismissed;

        this.interstitial.Show();
    }

    private AdRequestConfiguration CreateAdRequest(string adUnitId)
    {
        return new AdRequestConfiguration.Builder(adUnitId).Build();
    }

    private void DisplayMessage(String message)
    {
        Debug.Log($"YandexMobileAds: {message}");
        /*this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);*/
    }

    public void HandleAdLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        this.DisplayMessage("HandleAdLoaded event received");

        this.interstitial = args.Interstitial;
        AdsManager.Instance.OnLoadComplete?.Invoke();
        ShowInterstitial();
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.DisplayMessage($"HandleAdFailedToLoad event received with message: {args.Message}");
        AdsManager.Instance.OnFailedToLoad?.Invoke();
    }
    public void HandleAdClicked(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdClicked event received");
        AdsManager.Instance.OnClicked?.Invoke();
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdShown event received");
        AdsManager.Instance.OnShow?.Invoke(false);
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdDismissed event received");

        this.interstitial.Destroy();
        this.interstitial = null;
        AdsManager.Instance.OnDismissed?.Invoke();
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayMessage($"HandleImpression event received with data: {data}");
        AdsManager.Instance.OnImpression?.Invoke();
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage($"HandleAdFailedToShow event received with message: {args.Message}");
        AdsManager.Instance.OnFailedToShow?.Invoke();
    }
}

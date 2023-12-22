using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexAppOpenAd : MonoBehaviour
{
    public UnityEvent OnFailedToLoad = new UnityEvent();

    [SerializeField]
    private string _adUnitId = "R-M-4609579-4";

    private String message = "";

    private AppOpenAdLoader appOpenAdLoader;
    private AppOpenAd appOpenAd;

    public void Awake()
    {
        this.appOpenAdLoader = new AppOpenAdLoader();
        this.appOpenAdLoader.OnAdLoaded += this.HandleAdLoaded;
        this.appOpenAdLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;

        // Use the AppStateObserver to listen to application open/close events.
        AppStateObserver.OnAppStateChanged += HandleAppStateChanged;

        RequestAppOpenAd();
    }

    public void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks.
        AppStateObserver.OnAppStateChanged -= HandleAppStateChanged;
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
            if (GUILayout.Button("Request AppOpenAd", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
            {
                this.RequestAppOpenAd();
            }
#endif

        GUILayout.Label(this.message, labelStyle);
    }*/

    private void RequestAppOpenAd()
    {
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        // Replace demo Unit ID 'demo-appOpenAd-yandex' with actual Ad Unit ID
        string adUnitId = _adUnitId;//"demo-appopenad-yandex";

        if (this.appOpenAd != null)
        {
            this.appOpenAd.Destroy();
        }

        this.appOpenAdLoader.LoadAd(this.CreateAdRequestConfiguration(adUnitId));
        this.DisplayMessage("AppOpenAd is requested");
    }

    private void ShowAppOpenAd()
    {
        if (this.appOpenAd == null)
        {
            this.DisplayMessage("AppOpenAd is not ready yet");
            return;
        }

        this.appOpenAd.OnAdClicked += this.HandleAdClicked;
        this.appOpenAd.OnAdShown += this.HandleAdShown;
        this.appOpenAd.OnAdFailedToShow += this.HandleAdFailedToShow;
        this.appOpenAd.OnAdImpression += this.HandleImpression;
        this.appOpenAd.OnAdDismissed += this.HandleAdDismissed;

        this.appOpenAd.Show();
    }

    private AdRequestConfiguration CreateAdRequestConfiguration(string adUnitId)
    {
        return new AdRequestConfiguration.Builder(adUnitId).Build();
    }

    private void DisplayMessage(String message)
    {
        Debug.Log($"YandexMobileAds: {message}");
        /*this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);*/
    }

    public void HandleAppStateChanged(object sender, AppStateChangedEventArgs args)
    {
        if (this.appOpenAd != null && args.IsInBackground == false)
        {
            ShowAppOpenAd();
        }
    }

    public void HandleAdLoaded(object sender, AppOpenAdLoadedEventArgs args)
    {
        this.DisplayMessage("HandleAdLoaded event received");

        this.appOpenAd = args.AppOpenAd;
        AdsManager.Instance.OnLoadComplete?.Invoke();
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.DisplayMessage("HandleAdFailedToLoad event received with message: " + args.Message);
        OnFailedToLoad?.Invoke();
        AdsManager.Instance.OnFailedToLoad?.Invoke();
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdClicked event received");
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdShown event received");
        AdsManager.Instance.OnShow?.Invoke(false);
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdDismissed event received");

        this.appOpenAd.Destroy();
        this.appOpenAd = null;
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

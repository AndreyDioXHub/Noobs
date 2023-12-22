using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YandexMobileAds;
using YandexMobileAds.Base;


public class YandexAdsRewardedAd : MonoBehaviour
{
    public static YandexAdsRewardedAd Instance;

    [SerializeField]
    private string _adUnitId = "R-M-4609579-1";
    [SerializeField]
    private Dictionary<string, string> _adUnitIds = new Dictionary<string, string>() 
    {
        {"coins" , "R-M-4609579-1"}
    };//"R-M-4609579-1";

    private String message = "";

    private RewardedAdLoader rewardedAdLoader;
    private RewardedAd rewardedAd;


    public void Awake()
    {
        Instance = this;
        this.rewardedAdLoader = new RewardedAdLoader();
        this.rewardedAdLoader.OnAdLoaded += this.HandleAdLoaded;
        this.rewardedAdLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
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
            if (GUILayout.Button("Request Rewarded Ad", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
            {
                this.RequestRewardedAd();
            }

            if (this.rewardedAd != null) {
                if (GUILayout.Button("Show Rewarded Ad", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
                {
                    this.ShowRewardedAd();
                }
            }
            if (this.rewardedAd != null)
            {
                if (GUILayout.Button("Destroy Rewarded Ad", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 8)))
                {
                    this.rewardedAd.Destroy();
                }
            }
#endif

        GUILayout.Label(this.message, labelStyle);
    }
*/
    public void RequestRewardedAd()
    {
        this.DisplayMessage("RewardedAd is not ready yet");
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        if (this.rewardedAd != null)
        {
            this.rewardedAd.Destroy();
        }

        // Replace demo Unit ID 'demo-rewarded-yandex' with actual Ad Unit ID
        string adUnitId = _adUnitId;// "R-M-4609579-1";//"demo-rewarded-yandex";

        this.rewardedAdLoader.LoadAd(this.CreateAdRequest(adUnitId));
        this.DisplayMessage("Rewarded Ad is requested");
    }

    public void RequestRewardedAd(string key)
    {
        this.DisplayMessage("RewardedAd is not ready yet");
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        if (this.rewardedAd != null)
        {
            this.rewardedAd.Destroy();
        }

        // Replace demo Unit ID 'demo-rewarded-yandex' with actual Ad Unit ID
        string adUnitId = _adUnitIds[key];
        this.rewardedAdLoader.LoadAd(this.CreateAdRequest(adUnitId));
        this.DisplayMessage("Rewarded Ad is requested");
        AdsManager.Instance.OnFailedToLoad?.Invoke();
    }



    public void ShowRewardedAd()
    {
        if (this.rewardedAd == null)
        {
            this.DisplayMessage("RewardedAd is not ready yet");
            return;
        }

        this.rewardedAd.OnAdClicked += this.HandleAdClicked;
        this.rewardedAd.OnAdShown += this.HandleAdShown;
        this.rewardedAd.OnAdFailedToShow += this.HandleAdFailedToShow;
        this.rewardedAd.OnAdImpression += this.HandleImpression;
        this.rewardedAd.OnAdDismissed += this.HandleAdDismissed;
        this.rewardedAd.OnRewarded += this.HandleRewarded;

        this.rewardedAd.Show();
        AdsManager.Instance.OnShow?.Invoke(true);
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

    public void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        this.DisplayMessage("HandleAdLoaded event received");
        this.rewardedAd = args.RewardedAd;
        ShowRewardedAd();
        AdsManager.Instance.OnLoadComplete?.Invoke();
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.DisplayMessage(
            $"HandleAdFailedToLoad event received with message: {args.Message}");
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

        this.rewardedAd.Destroy();
        this.rewardedAd = null;

        AdsManager.Instance.OnDismissed?.Invoke();
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayMessage($"HandleImpression event received with data: {data}");
        AdsManager.Instance.OnImpression?.Invoke();
    }

    public void HandleRewarded(object sender, Reward args)
    {
        this.DisplayMessage($"HandleRewarded event received: amout = {args.amount}, type = {args.type}");
        AdsManager.Instance.OnReward?.Invoke(args.amount, args.type);
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage(
            $"HandleAdFailedToShow event received with message: {args.Message}");
        AdsManager.Instance.OnFailedToShow?.Invoke();
    }
}

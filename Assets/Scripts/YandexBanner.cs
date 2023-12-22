using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexBanner : MonoBehaviour
{
    [SerializeField]
    private string _adUnitId = "R-M-4609579-2";

    private String message = "";

    private Banner banner;

    private void Start()
    {
    }

    private void OnEnable()
    {
        if(this.banner == null)
        {
            RequestBanner();
        }
        else
        {
            this.banner.Show();
        }
    }

    private void OnDisable()
    {
        this.banner?.Hide();
    }

    private void RequestBanner()
    {

        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        // Replace demo Unit ID 'demo-banner-yandex' with actual Ad Unit ID
        string adUnitId = _adUnitId;// "demo-banner-yandex";

        if (this.banner != null)
        {
            this.banner.Destroy();
        }
        // Set sticky banner width
        BannerAdSize bannerSize = BannerAdSize.StickySize(GetScreenWidthDp());
        // Or set inline banner maximum width and height
        // BannerAdSize bannerSize = BannerAdSize.InlineSize(GetScreenWidthDp(), 300);
        this.banner = new Banner(adUnitId, bannerSize, AdPosition.BottomCenter);

        this.banner.OnAdLoaded += this.HandleAdLoaded;
        this.banner.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.banner.OnReturnedToApplication += this.HandleReturnedToApplication;
        this.banner.OnLeftApplication += this.HandleLeftApplication;
        this.banner.OnAdClicked += this.HandleAdClicked;
        this.banner.OnImpression += this.HandleImpression;

        this.banner.LoadAd(this.CreateAdRequest());
        this.DisplayMessage("Banner is requested");
    }

    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void DisplayMessage(String message)
    {
        Debug.Log($"YandexMobileAds: {message}");
        /*
        this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);*/
    }

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdLoaded event received");
        this.banner.Show();
    }

    public void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage("HandleAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleLeftApplication(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleLeftApplication event received");
    }

    public void HandleReturnedToApplication(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleReturnedToApplication event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdLeftApplication event received");
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdClicked event received");
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayMessage("HandleImpression event received with data: " + data);
    }
}
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
    private TMP_Text _viewText;
    [SerializeField]
    private TMP_InputField _inputField;
    private Banner banner;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RequestBanner()
    {
        string adUnitId = _inputField.text;

        // Set flexible banner maximum width and height
        BannerAdSize bannerMaxSize = BannerAdSize.InlineSize(GetScreenWidthDp(), 100);
        // Or set sticky banner maximum width
        //AdSize bannerMaxSize = AdSize.StickySize(GetScreenWidthDp());

        banner = new Banner(adUnitId, bannerMaxSize, AdPosition.BottomCenter);
        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);

        banner.OnAdLoaded += HandleAdLoaded;
        banner.OnAdFailedToLoad += HandleAdFailedToLoad;
        banner.OnReturnedToApplication += HandleReturnedToApplication;
        banner.OnLeftApplication += HandleLeftApplication;
        banner.OnAdClicked += HandleAdClicked;
        banner.OnImpression += HandleImpression;
    }

    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        _viewText.text +="\nHandleAdLoaded event received";
        banner.Show();
    }

    public void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        _viewText.text += "\nHandleAdFailedToLoad event received with message: " + args.Message;
        MonoBehaviour.print("HandleAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleLeftApplication(object sender, EventArgs args)
    {
        _viewText.text += "\nHandleLeftApplication event received";
        MonoBehaviour.print("HandleLeftApplication event received");
    }

    public void HandleReturnedToApplication(object sender, EventArgs args)
    {
        _viewText.text += "\nHandleReturnedToApplication event received";
        MonoBehaviour.print("HandleReturnedToApplication event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        _viewText.text += "\nHandleAdLeftApplication event received";
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        _viewText.text += "\nHandleAdClicked event received";
        MonoBehaviour.print("HandleAdClicked event received");
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        MonoBehaviour.print("HandleImpression event received with data: " + data);
        _viewText.text += "\nHandleImpression event received with data: " + data;
    }

    public void DestroyBaner()
    {
        banner.Destroy();
    }
}
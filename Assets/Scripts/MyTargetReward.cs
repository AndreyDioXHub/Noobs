using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MyTargetReward : MonoBehaviour
{
    [SerializeField]
    private UInt32 _slotId = 38837;//1474902;

    [SerializeField]
    private TMP_Text _viewText;
    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private TMP_InputField _inputField2;
    [SerializeField]
    private string _testDevice = "ac84c9a0-519c-4bb5-aafb-fb0eb330594f";


    private InterstitialAd _interstitialAd;

    private readonly System.Object _syncRoot = new System.Object();
    private volatile RewardedAd _rewardedAd;

    private void Awake()
    {
        MyTargetManager.DebugMode = true;
        //MyTargetManager.Config = new MyTargetConfig.Builder().WithTestDevices("TEST_DEVICE_ID").Build();
        MyTargetManager.Config = new MyTargetConfig.Builder().WithTestDevices(_testDevice, "TEST_DEVICE_ID", "", _inputField2.text).Build();
    }


    public void UpdateValue(string value)
    {
        UInt32.TryParse(value, out _slotId);
    }

    private void OnDestroy()
    {
        if (_rewardedAd == null)
        {
            return;
        }

        lock (_syncRoot)
        {
            _rewardedAd?.Dispose();
            _rewardedAd = null;
        }
    }

    public void Show()
    {
        Show(_slotId);
    }
    private void Show(UInt32 slotId)
    {
        if (_rewardedAd != null)
        {
            return;
        }

        lock (_syncRoot)
        {
            if (_rewardedAd != null)
            {
                return;
            }

            _rewardedAd = new RewardedAd(slotId)
            {
                CustomParams =
                                      {
                                          Age = 23,
                                          Gender = GenderEnum.Male,
                                          Lang = "ru-RU"
                                      }
            };


            _rewardedAd.AdClicked += OnAdClicked;
            _rewardedAd.AdDismissed += OnAdDismissed; 
            _rewardedAd.AdDisplayed += OnAdDisplayed;
            _rewardedAd.AdLoadFailed += OnAdLoadFailed;
            _rewardedAd.AdRewarded += OnReward;
            _rewardedAd.AdLoadCompleted += OnLoadCompleted;

            _rewardedAd.Load();
        }
    }

    private void OnReward(System.Object sender, EventArgs e)
    {

        _viewText.text += "\nOnReward";
    }

    private void OnLoadCompleted(System.Object sender, EventArgs e)
    {
        /*var isAutoClose = FindObjectsOfType<Toggle>().Where(toggle => toggle.name == "Autoclose")
                                                     .Select(toggle => toggle.isOn)
                                                     .FirstOrDefault();
        */
        ThreadPool.QueueUserWorkItem(async state =>
        {
            _rewardedAd?.Show();
            /*
            if (!isAutoClose)
            {
                return;
            }*/

            await Task.Delay(120000);

            _rewardedAd?.Dismiss();
        });
    }
    private void OnAdDisplayed(System.Object sender, EventArgs e)
    {
        _viewText.text += "\nOnAdDisplayed";
    }

    private void OnAdDismissed(System.Object sender, EventArgs e)
    {
        _viewText.text += "\nOnAdDismissed";
        OnDestroy();
    }

    private void OnAdVideoCompleted(System.Object sender, EventArgs e)
    {

        _viewText.text += "\nOnAdVideoCompleted";
    }

    private void OnAdClicked(System.Object sender, EventArgs e)
    {
        _viewText.text += "\nOnAdClicked";
    }

    private void OnAdLoadFailed(System.Object sender, ErrorEventArgs e)
    {
        _viewText.text += "\nOnAdLoadFailed: " + e.Message;
        Debug.Log("OnAdLoadFailed: " + e.Message);
        OnDestroy();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdsView : MonoBehaviour
{
    [SerializeField]
    private Button _adsButton;

    [SerializeField]
    private GameObject _timer; 
    [SerializeField]
    private GameObject _button;

    [SerializeField]
    private TextMeshProUGUI _timerText0;
    [SerializeField]
    private TextMeshProUGUI _timerText1;

    void Start()
    {
        _adsButton.onClick.AddListener(PlatformAdsManager.Instance.ShowRewardedAd);
    }

    void Update()
    {
        if (PlatformAdsManager.Instance.IsPaused)
        {
            _timer.SetActive(true);
            _button.SetActive(false);

            _timerText0.text = PlatformAdsManager.Instance.TimeString;
            _timerText1.text = PlatformAdsManager.Instance.TimeString;
        }
        else
        {
            _timer.SetActive(false);
            _button.SetActive(true);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public UnityEvent<int> OnMoneyAdded = new UnityEvent<int>();
    public UnityEvent<int> OnMoneyRemoved = new UnityEvent<int>();

    public int Coins => _coins;

    [SerializeField]
    private int _coins;
    [SerializeField]
    private bool _isMyReward;

    /*
    [SerializeField]
    private TextMeshProUGUI _text;*/

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadCoins();
        //_coins = PlayerPrefs.GetInt("coins", 0);
    }

    public void LoadCoins()
    {
        PlayerSave.Instance.ExecuteMyDelegateInQueue(GetCoinsCount);
    }

    public void GetCoinsCount()
    {
        if (YandexGame.SDKEnabled)
        {
            _coins = YandexGame.savesData.coins;
        }
    }

    void Update()
    {
        //_text.text = _coins.ToString();
    }

    public void ShowRewardForMoney()
    {
        _isMyReward = true;
        AdsManager.Instance.ShowRewardedAd();
    }

    public void Rewarded()
    {
        if (_isMyReward)
        {
            AddMoney(50);
        }
    }

    public bool CheckMoney(int cost)
    {
        bool result = false;

        if (cost <= _coins)
        {
            result = true;
        }

        return result;
    }

    public bool TryToBuy(int cost)
    {
        bool result = false;

        if (cost <= _coins)
        {
            result = true;
            _coins -= cost;

            if (_coins < 0)
            {
                result = false;
                _coins += cost;
            }

            if (result)
            {
                OnMoneyRemoved?.Invoke(cost);
            }

            if (YandexGame.SDKEnabled)
            {
                YandexGame.savesData.coins = _coins;
                PlayerSave.Instance.Save();
            }
            //PlayerPrefs.SetInt("coins", _coins);
        }

        return result;
    }

    public void AddMoney(int count)
    {
        _coins += count;

        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.coins = _coins;
            PlayerSave.Instance.Save();
        }

        /*
        PlayerPrefs.SetInt("coins", _coins);*/

        OnMoneyAdded?.Invoke(count);
    }

    [ContextMenu("AddMoney")]
    public void AddMoney()
    {
        AddMoney(500);
    }


}

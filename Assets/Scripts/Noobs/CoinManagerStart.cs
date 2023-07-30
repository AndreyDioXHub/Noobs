using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManagerStart : MonoBehaviour
{
    [SerializeField]
    private int _coins;

    [SerializeField]
    private TextMeshProUGUI _coinsText;

    void Start()
    {
        _coins = PlayerPrefs.GetInt("PlatformCoins", 0);
        _coinsText.text = $"{_coins}";
    }


    public bool TryBuy(int coins)
    {
        if(coins > _coins)
        {
            return false;
        }
        else
        {
            _coins = _coins - coins;
            PlayerPrefs.SetInt("PlatformCoins", _coins);

            return true;
        }

        return false;
    }


    void Update()
    {
        _coinsText.text = $"{_coins}";
    }
}

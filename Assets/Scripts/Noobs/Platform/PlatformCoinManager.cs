using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformCoinManager : MonoBehaviour
{
    public static PlatformCoinManager Instance;

    [SerializeField]
    private int _coins;
    public int EarnedCoins;

    [SerializeField]
    private TextMeshProUGUI _coinsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _coins = PlayerPrefs.GetInt(PlayerPrefsConsts.COINS, 0);
        _coinsText.text = $"{_coins}";

    }

    void Update()
    {
        
    }
    public void AddCoin()
    {
        EarnedCoins++;
        _coins++;
        _coinsText.text = $"{_coins}";
        PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, _coins);
    }
    
    public void AddCoin(int coins)
    {
        _coins+= coins;
        _coinsText.text = $"{_coins}";
        PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, _coins);
    }
}

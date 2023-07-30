using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int EarnedCoins { get => _earnedCoins; }

    [SerializeField]
    private int _coins;
    [SerializeField]
    private int _earnedCoins;
    [SerializeField]
    private int _coinsLVL;

    [SerializeField]
    private TextMeshProUGUI _coinsText;

    private void Awake()
    {
        if(Instance == null)
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
        _coins = PlayerPrefs.GetInt("PlatformCoins", 0);
        _coinsText.text = $"{_coins}";
    }

    [ContextMenu("Zerable")]
    public void Zerable()
    {
        PlayerPrefs.SetInt("PlatformCoins", 0);
    }

    void Update()
    {
        
    }

    public void AddCoin()
    {
        _earnedCoins++;
        _coins++;
        _coinsLVL--;
        _coinsText.text = $"{_coins}";
        PlayerPrefs.SetInt("PlatformCoins", _coins);
    }

    public void AddCoin(int count)
    {
        _earnedCoins += count;
        _coins += count;
        _coinsLVL-= count;
        _coinsText.text = $"{_coins}";
        PlayerPrefs.SetInt("PlatformCoins", _coins);
    }
    
    public void RegisterCoin()
    {
        _coinsLVL++;
    }


}

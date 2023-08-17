using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManagerStart : MonoBehaviour
{
    public static CoinManagerStart Instance;

    [SerializeField]
    private int _coins;

    [SerializeField]
    private TextMeshProUGUI _coinsText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _coins = PlayerPrefs.GetInt(PlayerPrefsConsts.COINS, 0);
        _coinsText.text = $"{_coins}";

        //Cursor.lockState = CursorLockMode.None;
    }

    public void AddCoins(int coins)
    {
        _coins += coins;
        PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, _coins);
    }

    [ContextMenu("Zerable")]
    public void Zerable()
    {
        PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, 0);
        PlayerPrefs.SetString(PlayerPrefsConsts.SKINS, "");
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
            PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, _coins);

            return true;
        }

        return false;
    }


    void Update()
    {
        _coinsText.text = $"{_coins}";
    }
}

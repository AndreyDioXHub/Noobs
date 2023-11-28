using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField]
    private int _coins;
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _coins = PlayerPrefs.GetInt("coins", 0);
    }

    void Update()
    {
        _text.text = _coins.ToString();
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

            PlayerPrefs.SetInt("coins", _coins);
        }

        return result;
    }

    public void AddMoney(int count)
    {
        _coins += count;
        PlayerPrefs.SetInt("coins", _coins);
    }

    [ContextMenu("AddMoney")]
    public void AddMoney()
    {
        AddMoney(500);
    }


}

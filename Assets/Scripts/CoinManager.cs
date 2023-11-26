using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField]
    private int _coins;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {

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
        }

        return result;

    }
}

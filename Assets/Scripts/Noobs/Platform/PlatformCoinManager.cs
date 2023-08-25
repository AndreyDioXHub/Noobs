using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformCoinManager : MonoBehaviour
{
    public static PlatformCoinManager Instance;
    [SerializeField]
    private AudioSource _coinAudio;
    [SerializeField]
    private int _coins;
    [SerializeField]
    private Transform _coinsCanvas;
    [SerializeField]
    private GameObject _coinsUIPrefab;
    public int EarnedCoins;
    public int RewaindCoins;

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
        StartCoroutine(ShowCoinsCoroutine(1));
    }
    
    public void AddCoin(int coins)
    {
        RewaindCoins += coins;
        StartCoroutine(ShowCoinsCoroutine(coins));
    }

    public void SpeedUp()
    {
        StopAllCoroutines();
        switch (PlatformGameManager.Instance.winState)
        {
            case WinState.play:
                break;
            case WinState.win:
                _coins += RewaindCoins;
                PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, _coins);
                break;
            case WinState.lose:
                EarnedCoins = 0;
                break;
            default:
                break;
        }
    }

    IEnumerator ShowCoinsCoroutine(int count)
    {
        switch (PlatformGameManager.Instance.winState)
        {
            case WinState.play:
                int index = 0;

                while (index < count)
                {
                    var go = Instantiate(_coinsUIPrefab, _coinsCanvas);
                    Destroy(go, 0.5f);
                    _coinAudio.Play();
                    EarnedCoins++;
                    RewaindCoins--;
                    RewaindCoins = RewaindCoins < 0 ? 0 : RewaindCoins;
                    _coins++;
                    _coinsText.text = $"{_coins}";
                    PlayerPrefs.SetInt(PlayerPrefsConsts.COINS, _coins);
                    index++;
                    yield return new WaitForSeconds(0.2f);
                }
                break;
            case WinState.win:
                break;
            case WinState.lose:
                EarnedCoins = 0;
                break;
            default:
                break;
        }        
    }
}

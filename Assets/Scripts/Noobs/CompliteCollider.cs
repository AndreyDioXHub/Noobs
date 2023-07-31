using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompliteCollider : MonoBehaviour
{
    [SerializeField]
    private int _id; 

    [SerializeField]
    private GameObject _ilind;
    [SerializeField]
    private int _playerCount;
    [SerializeField]
    private bool _isProcess;

    [SerializeField]
    private GameObject _floor;

    private bool _isEnd;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player") || other.tag.Equals("Avatar") || other.tag.Equals("Bot"))
        {
            _playerCount++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*
        if (_playerCount == 1 && !_isProcess)
        {
            _isProcess = true;

            if (other.tag.Equals("Player") || other.tag.Equals("Avatar") || other.tag.Equals("Bot"))
            {
                Debug.Log($"Win lvl id {_id} {_playerCount}");
                WinLVL(other.gameObject);
            }
        }/**/
    }

    public void WinLVL(GameObject player)
    {
        StartCoroutine(WinLVLCoroutine(player));
    }

    IEnumerator WinLVLCoroutine(GameObject player)
    {
        if(player== null)
        {

        }
        else
        {
            var platforms = _ilind.GetComponentsInChildren<Platform>();

            List<Coin> coins = new List<Coin>();
            List<PlatformDestroyer> platformDestroyers = new List<PlatformDestroyer>();

            int coinsCount = 0;

            foreach (var platform in platforms)
            {
                if (platform != null)
                {
                    var coin = platform.gameObject.GetComponentInChildren<Coin>();
                    var platformDestroyer = platform.gameObject.GetComponentInChildren<PlatformDestroyer>();

                    coins.Add(coin);
                    platformDestroyers.Add(platformDestroyer);

                    if(coin != null)
                    {
                        if (coin.HaveCoin)
                        {
                            coinsCount++;
                        }
                    }
                }
            }

            if (player.tag.Equals("Player"))
            {
                WinScreen.Instance.Show(LocalizationStrings.game_scene_lvl_win_t0_curent, $"{LocalizationStrings.game_scene_lvl_win_t1_curent} {coinsCount}", "");

                if (_id == 0)
                {
                    _isEnd = true;
                    //GameManager.Instance.IsWin = true;
                    CoinManager.Instance.AddCoin(coinsCount);

                    WinScreen.Instance.Show(LocalizationStrings.game_scene_game_win_t0_curent, $"{LocalizationStrings.game_scene_game_win_t1_curent} {CoinManager.Instance.EarnedCoins}", LocalizationStrings.game_scene_game_win_t2_curent);
                    _floor.SetActive(true);

                    StopCoroutine(WinLVLCoroutine(player));
                }
            }

            yield return new WaitForSeconds(2f);

            if (!_isEnd && !GameManager.Instance.IsWin)
            {
                foreach (var platform in platforms)
                {
                    if (platform != null)
                    {
                        platform.DisableGround();
                    }
                }

                for (int i = 0; i < platforms.Length; i++)
                {
                    if (platforms[i] != null)
                    {
                        Debug.Log("coin to player");

                        coins[i].GetCoin(player);
                        platformDestroyers[i].DestroyPlatform();

                        if (coins[i].HaveCoin)
                        {
                            yield return new WaitForSeconds(0.1f);
                        }
                        else
                        {
                            //yield return new WaitForSeconds(0.02f);
                        }

                    }
                }
            }    
        }


        Destroy(this);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player") || other.tag.Equals("Avatar") || other.tag.Equals("Bot"))
        {
            _playerCount--;
        }
    }


    void Update()
    {
        
    }
}

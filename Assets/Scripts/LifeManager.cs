using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    [SerializeField]
    private int _life = 3;
    [SerializeField]
    private List<GameObject> _lifeGOs = new List<GameObject>();
    [SerializeField]
    private GameObject _dieStreen;
    [SerializeField]
    private GameObject _respButton;
    [SerializeField]
    private GameObject _reloadButton;
    [SerializeField]
    private bool _isDead;
    [SerializeField]
    private bool _isMyReward;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_life > 0)
        {
            _respButton.SetActive(true);
            _reloadButton.SetActive(false);
        }
        else
        {
            _respButton.SetActive(false);
            _reloadButton.SetActive(true);
        }

        if (_isDead)
        {
            if (SettingScreen.IsActive || AdsManager.AdsPlaying)
            {
                _dieStreen.SetActive(false);
            }
            else
            {
                _dieStreen.SetActive(true);
            }
        }
        else
        {
            _dieStreen.SetActive(false);
        }
    }

    public void PlayerDead()
    {
        //Debug.Log("ProcessLife");
        _isDead = true;
        //Respawn();
    }

    public void Restart()
    {
        _life = 3;
        _isDead = false;

        for (int i = 0; i < _lifeGOs.Count; i++)
        {
            _lifeGOs[i].SetActive(false);
        }

        for (int i = 0; i < _life; i++)
        {
            _lifeGOs[i].SetActive(true);
        }

        CheckPointManager.Instance.SelectCheckPoint(0);

    }

    public void MinusLife()
    {
        if (_life > 0)
        {
            _life--;

            for(int i=0; i< _lifeGOs.Count; i++)
            {
                _lifeGOs[i].SetActive(false);
            }

            for(int i=0; i< _life; i++)
            {
                _lifeGOs[i].SetActive(true);
            }

            Respawn();
        }
        else
        {

        }
    }

    public void ShowRewardForLife()
    {
        _isMyReward = true;
        AdsManager.Instance.ShowRewardedAd();
    }

    public void OnRewarded(int count, string key)
    {
        if (key.Equals("Life"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        _isDead = false;
        CheckPointManager.Instance.ReturnToCheckPoint();
    }

}

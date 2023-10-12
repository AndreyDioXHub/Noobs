using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformGameManager : MonoBehaviour
{
    public static PlatformGameManager Instance;

    public int Index { get => _index; }
    /*
    [SerializeField]
    private TextMeshProUGUI _text0;*/

    public WinState winState { get; private set; } 

    [SerializeField]
    private PlatformLVLAnimatorController _animatorController;
    [SerializeField]
    private int _index;
    [SerializeField]
    private List<KeyBoardRecorder> _keyBoardRecorders = new List<KeyBoardRecorder>();
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private DistributionHat _playerHat;
    [SerializeField]
    private bool _needReward = true;
    [SerializeField]
    private float _time = 2;
    [SerializeField]
    private float _timeCur = 0;

    [SerializeField]
    private bool _needPlayer;
    [SerializeField]
    private bool _needRandomIndex;


   private List<List<string>> _animationSequences = new List<List<string>>()
    {
        { 
            new List<string>()
            {
                "Casino pt 1 Animation",
                "Casino pt 2 Animation",
                "Casino pt 3 Animation",
                "Casino pt 4 Animation",
                "Casino pt 5 Animation",
                "Casino pt 6 Animation",
                "Casino pt 7 Animation",
                "Casino pt 8 Animation",
                "Casino pt 9 Animation",
                "Casino pt 10 Animation",
            } 
        },
        { 
            new List<string>()
            {
                "Casino pt 16 Animation",
                "Casino pt 15 Animation",
                "Casino pt 14 Animation",
                "Casino pt 13 Animation",
                "Casino pt 12 Animation",
                "Casino pt 11 Animation",
                "Casino pt 10 Animation",
                "Casino pt 1 Animation",
                "Casino pt 2 Animation",
                "Casino pt 3 Animation",
            } 
        },
        { 
            new List<string>()
            {
                "Casino pt 3 Animation",
                "Casino pt 4 Animation",
                "Casino pt 2 Animation",
                "Casino pt 6 Animation",
                "Casino pt 9 Animation",
                "Casino pt 14 Animation",
                "Casino pt 12 Animation",
                "Casino pt 10 Animation",
                "Casino pt 8 Animation",
                "Casino pt 1 Animation",
            } 
        },
        { 
            new List<string>()
            {
                "Casino pt 7 Animation",
                "Casino pt 6 Animation",
                "Casino pt 5 Animation",
                "Casino pt 1 Animation",
                "Casino pt 3 Animation",
                "Casino pt 16 Animation",
                "Casino pt 12 Animation",
                "Casino pt 2 Animation",
                "Casino pt 13 Animation",
                "Casino pt 10 Animation",
            } 
        },
    };

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
        _playerHat.Pause();
        winState = WinState.play;

        if (_needRandomIndex)
        {
            _index = Random.Range(0, _animationSequences.Count);

            int indexPrev = PlayerPrefs.GetInt(PlayerPrefsConsts.PREV_INDEX, 0);

            if (_index == indexPrev)
            {
                _index++;
                _index = _index > 3 ? 0 : _index;
            }

            PlayerPrefs.SetInt(PlayerPrefsConsts.PREV_INDEX, _index);

        }

        StartCoroutine(PrepareCoroutine());
    }

    IEnumerator PrepareCoroutine()
    {
        int playerIndex = Random.Range(0, _keyBoardRecorders.Count);
        int index = 0;

        while(index < _keyBoardRecorders.Count)
        {
            float randomTime = Random.Range(0.05f, 0.5f);

            if (playerIndex == index && _needPlayer)
            {
                _player.transform.position = _keyBoardRecorders[index].gameObject.transform.position;
                _player.SetActive(true);
                PlayerCount.Instance.RegisterPlayer();
                ConnectionScreen.Instance.Show(1);
            }
            else
            {
                _keyBoardRecorders[index].Prepare();
                PlayerCount.Instance.RegisterPlayer();
                ConnectionScreen.Instance.Show(1);
            }

            index++;
            yield return new WaitForSeconds(randomTime);
        }

        foreach(var rec in _keyBoardRecorders)
        {
            rec.StartProcess();
        }

        ConnectionScreen.Instance.gameObject.SetActive(false);
        _animatorController.Init(_animationSequences[_index]);

        _playerHat.Play();
        //Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        _timeCur += Time.deltaTime;


        if (winState != WinState.play)
        {
            if (_needReward)
            {
                _playerHat.Pause();
                _needReward = false;

                _timeCur = 0;

                switch (winState)
                {
                    case WinState.win:
                        string t0 = LocalizationStrings.Strings["winlvl1"];
                        string t1 = $"{LocalizationStrings.Strings["winlvl2"]}{PlatformCoinManager.Instance.EarnedCoins}";
                        PlatformCoinManager.Instance.AddCoin(PlatformCoinManager.Instance.EarnedCoins);
                        WinScreen.Instance.Show(t0, t1);
                        WinScreen.Instance.ShowWinScreen();
                        break;
                    case WinState.lose:
                        WinScreen.Instance.ShowLoseScreen();
                        break;
                    default:
                        break;
                }
            }
        }

        if(_timeCur > _time && winState != WinState.play)
        {
            WinScreen.Instance.ShowEscape();

            if (Input.anyKey)
            {
                PlatformCoinManager.Instance.SpeedUp();
                SceneManager.LoadScene(0);
            }
        }

        

    }

    [ContextMenu("GenerateSequence")]
    public void GenerateSequence()
    {
        List<string> allAnimationSequence = new List<string>()
        {
            "Platform Flip 0 Animation",
            "Platform Flip 1 Animation",
            "Platform Flip 2 Animation",
            "Platform Flip 3 Animation",
            "Platform Flip 4 Animation",
            "Platform Flip 5 Animation",
            "Platform Flip 6 Animation",
            "Platform Flip 7 Animation",
            "Platform Flip 8 Animation",
            "Platform Lazer 0 Animation",
            "Platform Lazer 1 Animation",
            "Platform Lazer 2 Animation",
            "Platform Lazer 3 Animation",
            "Platform Lazer 4 Animation",
            "Platform Lazer 5 Animation",
            "Platform Lazer 6 Animation"
        };

        string text = "";

        for(int i=0; i<9; i++)
        {
            Debug.Log(allAnimationSequence[Random.Range(0, allAnimationSequence.Count)]);
        }
    }

    public void SetWin()
    {
        if(winState == WinState.play)
        {
            winState = WinState.win;
        }
    }

    public void SetLose()
    {
        if (winState == WinState.play)
        {
            winState = WinState.lose;
        }

    }

}

public enum WinState
{
    play,
    win,
    lose
}

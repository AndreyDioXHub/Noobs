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


   private List<List<string>> _animationSequences = new List<List<string>>()
    {
        { 
            new List<string>()
            {
                "Platform Flip 7 Animation",
                "Platform Flip 8 Animation",
                "Platform Flip 0 Animation",
                "Platform Lazer 3 Animation",
                "Platform Lazer 2 Animation",
                "Platform Flip 2 Animation",
                "Platform Flip 8 Animation",
                "Platform Lazer 5 Animation",
                "Platform Lazer 3 Animation",
            } 
        },
        { 
            new List<string>()
            {
                "Platform Flip 8 Animation",
                "Platform Lazer 0 Animation",
                "Platform Flip 0 Animation",
                "Platform Flip 0 Animation",
                "Platform Flip 7 Animation",
                "Platform Flip 8 Animation",
                "Platform Flip 5 Animation",
                "Platform Lazer 5 Animation",
                "Platform Lazer 6 Animation",
            } 
        },
        { 
            new List<string>()
            {
                "Platform Flip 8 Animation",
                "Platform Lazer 6 Animation",
                "Platform Lazer 0 Animation",
                "Platform Flip 1 Animation",
                "Platform Lazer 1 Animation",
                "Platform Lazer 3 Animation",
                "Platform Flip 5 Animation",
                "Platform Flip 3 Animation",
                "Platform Lazer 3 Animation",
            } 
        },
        { 
            new List<string>()
            {
                "Platform Lazer 6 Animation",
                "Platform Lazer 3 Animation",
                "Platform Lazer 5 Animation",
                "Platform Flip 4 Animation",
                "Platform Flip 6 Animation",
                "Platform Lazer 3 Animation",
                "Platform Flip 8 Animation",
                "Platform Flip 7 Animation",
                "Platform Lazer 3 Animation",
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
        
        _index = Random.Range(0, _animationSequences.Count);

        int indexPrev = PlayerPrefs.GetInt(PlayerPrefsConsts.PREV_INDEX, 0);

        if(_index == indexPrev)
        {
            _index++;
            _index = _index > 3 ? 0 : _index;
        }

        PlayerPrefs.SetInt(PlayerPrefsConsts.PREV_INDEX, _index);
         
        StartCoroutine(PrepareCoroutine());
    }

    IEnumerator PrepareCoroutine()
    {
        int playerIndex = Random.Range(0, _keyBoardRecorders.Count);
        int index = 0;

        while(index < _keyBoardRecorders.Count)
        {
            float randomTime = Random.Range(0.05f, 0.5f);

            if (playerIndex == index)
            {
                PlayerCount.Instance.RegisterPlayer();
                _playerHat.Play();
                _player.SetActive(true);
                ConnectionScreen.Instance.Show(1);
                yield return new WaitForSeconds(randomTime);
            }

            _keyBoardRecorders[index].Prepare();
            index++;
            ConnectionScreen.Instance.Show(1);
            PlayerCount.Instance.RegisterPlayer();
            yield return new WaitForSeconds(randomTime);
        }

        foreach(var rec in _keyBoardRecorders)
        {
            rec.StartProcess();
        }

        ConnectionScreen.Instance.gameObject.SetActive(false);
        _animatorController.Init(_animationSequences[_index]);

        Cursor.lockState = CursorLockMode.Locked;
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
                        string t0 = LocalizationStrings.game_scene_game_win_t0_curent;
                        string t1 = $"{LocalizationStrings.game_scene_game_win_t1_curent}{PlatformCoinManager.Instance.EarnedCoins}";
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

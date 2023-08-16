using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoGameManager : MonoBehaviour
{
    public static CasinoGameManager Instance;

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
    private List<GameObject> _platforms = new List<GameObject>();
    [SerializeField]
    private bool _needReward = true;
    [SerializeField]
    private float _time = 2;
    [SerializeField]
    private float _timeCur = 0;
    [SerializeField]
    private bool _usePlayer;


    private List<List<string>> _animationSequences = new List<List<string>>()
    {
        {
            new List<string>()
            {
                "LVL Animation V 0",
                "LVL Animation V 15",
                "LVL Animation V 7",
                "LVL Animation V 8",
                "LVL Animation V 2",
                "LVL Animation V 13",
                "LVL Animation V 12",
                "LVL Animation V 10",
                "LVL Animation V 6",
                "LVL Animation V 4"
            }
        },
        {
            new List<string>()
            {
                "LVL Animation V 4",
                "LVL Animation V 12",
                "LVL Animation V 3",
                "LVL Animation V 8",
                "LVL Animation V 15",
                "LVL Animation V 11",
                "LVL Animation V 9",
                "LVL Animation V 8",
                "LVL Animation V 7",
                "LVL Animation V 5"
            }
        },
        {
            new List<string>()
            {
                "LVL Animation V 14",
                "LVL Animation V 15",
                "LVL Animation V 13",
                "LVL Animation V 12",
                "LVL Animation V 11",
                "LVL Animation V 10",
                "LVL Animation V 9",
                "LVL Animation V 8",
                "LVL Animation V 7",
                "LVL Animation V 5"
            }
        },
        {
            new List<string>()
            {
                "LVL Animation V 1",
                "LVL Animation V 3",
                "LVL Animation V 6",
                "LVL Animation V 12",
                "LVL Animation V 10",
                "LVL Animation V 8",
                "LVL Animation V 6",
                "LVL Animation V 4",
                "LVL Animation V 2",
                "LVL Animation V 0"
            }
        },
    };

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
        
    }

    void Update()
    {
        
    }
}

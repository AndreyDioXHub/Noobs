using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformGameManager : MonoBehaviour
{
    public static PlatformGameManager Instance;

    public int Index { get => _index; }

    [SerializeField]
    private TextMeshProUGUI _text0;

    [SerializeField]
    private PlatformLVLAnimatorController _animatorController;
    [SerializeField]
    private int _index;
    [SerializeField]
    private List<KeyBoardRecorder> _keyBoardRecorders = new List<KeyBoardRecorder>();


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
        _index = Random.Range(0, _animationSequences.Count);
         
        StartCoroutine(PrepareCoroutine());
    }

    IEnumerator PrepareCoroutine()
    {
        int index = 0;

        while(index< _keyBoardRecorders.Count)
        {
            _keyBoardRecorders[index].Prepare();
            index++;
            ConnectionScreen.Instance.Show(1);
            float randomTime = Random.Range(0.05f, 0.5f);
            yield return new WaitForSeconds(randomTime);
        }

        foreach(var rec in _keyBoardRecorders)
        {
            rec.StartProcess();
        }

        ConnectionScreen.Instance.gameObject.SetActive(false);
        _animatorController.Init(_animationSequences[_index]);
    }


    void Update()
    {
        
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
}

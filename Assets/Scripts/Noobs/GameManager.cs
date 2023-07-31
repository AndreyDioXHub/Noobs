using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsWin { get; private set; } = false;
    public bool IsLose;

    [SerializeField]
    private GameObject _loseScreen;
    [SerializeField]
    private float _time = 2;
    [SerializeField]
    private float _timeCur = 0;
    [SerializeField]
    private GameObject _leaveText;

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
        if(PlayerCount.Instance.Count == 1)
        {
            IsWin = true;
        }

        if (IsWin || IsLose)
        {
            _timeCur += Time.deltaTime;

            if(_timeCur > _time)
            {
                _leaveText.SetActive(true);

                if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}

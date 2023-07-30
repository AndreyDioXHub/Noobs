using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    public static PlayerCount Instance;

    public int Count { get => _playerCount; }

    [SerializeField]
    private int _playerCount;
    [SerializeField]
    private TextMeshProUGUI _peopleCountText;

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
        if (GameManager.Instance.IsWin)
        {
            _peopleCountText.text = $"{1}";
        }
        
    }

    public void RegisterPlayer()
    {
        _playerCount++;
        _peopleCountText.text = $"{_playerCount}";
    }

    public void RemovePlayer()
    {
        _playerCount--;
        _peopleCountText.text = $"{_playerCount}";
    }


}

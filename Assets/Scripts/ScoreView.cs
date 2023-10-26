using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    public static ScoreView Instance;

    [SerializeField]
    private TextMeshProUGUI _text0;
    [SerializeField]
    private int _scoreMax;
    [SerializeField]
    private int _score;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _scoreMax = PlayerPrefs.GetInt("score", _scoreMax);
        _text0.text = $"{_score} / {_scoreMax}";
    }

    public void AddScore()
    {
        _score++;

        if (_score > _scoreMax)
        {
            _scoreMax = _score;
            PlayerPrefs.SetInt("score", _scoreMax);
        }

        _text0.text = $"{_score} / {_scoreMax}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

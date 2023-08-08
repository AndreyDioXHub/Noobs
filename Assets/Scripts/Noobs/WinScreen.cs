using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public static WinScreen Instance;

    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private GameObject _loseScreen;

    [SerializeField]
    private TextMeshProUGUI _text0;
    [SerializeField]
    private TextMeshProUGUI _text1;
    [SerializeField]
    private TextMeshProUGUI _text2;
    [SerializeField]
    private float _showTime = 2;
    [SerializeField]
    private float _showTimeCur = 0;

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

    public void Show(string t0, string t1, string t2)
    {
        _showTimeCur = 0;
        _text0.text = t0;
        _text1.text = t1;
        _text2.text = t2;
    }

    void Start()
    {
        
    }

    void Update()
    {
        //TODO Заменить на событие от GameManager

        /**

        _showTimeCur += Time.deltaTime;

        if (GameManager.Instance.IsWin)
        {
            _winScreen.SetActive(true);
        }
        else
        {
            if (_showTimeCur > _showTime)
            {
                _winScreen.SetActive(false);
            }
            else
            {
                _winScreen.SetActive(true);
            }
        }

        
        if (GameManager.Instance.IsLose)
        {
            _loseScreen.SetActive(true);
        }/**/

    }

    internal void ShowWinScreen() {
        _winScreen.SetActive(true);
    }

    internal void ShowLoseScreen() {
        _loseScreen.SetActive(true);
    }
}

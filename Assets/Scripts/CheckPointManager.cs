using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CheckPointManager : MonoBehaviour
{

    public static CheckPointManager Instance;

    public bool IsWin;
    /*
    [SerializeField]
    private LeaderboardYG leaderboardYG;*/
    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private List<CheckPoint> _checkPoints = new List<CheckPoint>();
    [SerializeField]
    private float _distance, _curvalue, _curvaluePrev;
    [SerializeField]
    private Image _progressEmpty, _progressCur, _progressCheckPoints;
    [SerializeField]
    private float _score;
    [SerializeField]
    private float _scoreBonus;
    [SerializeField]
    private TextMeshProUGUI _text;
    private int _scoreTotal;

    [SerializeField]
    private TextMeshProUGUI _newcheckpointText;
    [SerializeField]
    private float _duration = 3;


    private void Awake()
    {
        Instance = this;
    }
    
    public void Init(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    void Start()
    {
        _newcheckpointText.CrossFadeAlpha(0,0,true);
        _distance = _checkPoints[_checkPoints.Count - 1].transform.position.z - _checkPoints[0].transform.position.z;
        //_score = _distance;
        _scoreBonus = _distance;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > 220)
        {
            _scoreBonus -= 10 * Time.deltaTime;
            _scoreBonus = _scoreBonus < 0 ? 0 : _scoreBonus;
        }

        _curvalue = _playerTransform.position.z - _checkPoints[0].transform.position.z;
        _curvalue = _curvalue < 0? 0: _curvalue / _distance;
        _curvalue = _curvalue < _curvaluePrev ? _curvaluePrev : _curvalue;
        _progressCur.fillAmount = _curvalue;
        _progressEmpty.fillAmount = 1 - _curvalue;
        _curvaluePrev = _curvalue;
        _score = _curvalue * _distance;

        if (IsWin)
        {
            if (SettingScreen.IsActive)
            {
                _winScreen.SetActive(false);
            }
            else
            {
                _winScreen.SetActive(true);
            }
        }
        else
        {
            _winScreen.SetActive(false);
        }
    }



    public void ReturnToCheckPoint()
    {
        if(_playerTransform != null)
        {
            _playerTransform.GetComponent<RobloxController>().ReturnToCheckPoint(_checkPoints.Find(cp => cp.State == CheckPointState.active).transform.position);
        }
    }

    /*
    public void SetNewRecord()
    {
        int score = (int)_score + (int)_scoreBonus;

        if(score > _scoreTotal)
        {
            //SetNewRecord(_scoreTotal);
            _scoreTotal = score;
        }
    }*/
    /*
    public void SetNewRecord(int record)
    {
        YandexGame.NewLeaderboardScores(leaderboardYG.nameLB, record);
    }*/

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void ShowNewPointMessage()
    {
        StartCoroutine(ShowNewPointMessageCoroutine());
    }

    IEnumerator ShowNewPointMessageCoroutine()
    {
        _newcheckpointText.CrossFadeAlpha(1, _duration, true);
        yield return new WaitForSeconds(_duration);
        _newcheckpointText.CrossFadeAlpha(0, _duration, true);

    }

    public void SetActiveCheckPoint(CheckPoint active)
    {
        foreach(var checkPoint in _checkPoints)
        {
            checkPoint.State = CheckPointState.passive;
        }
        var cpcur = _checkPoints.Find(cp => cp == active);
        cpcur.State = CheckPointState.active;

        int index = _checkPoints.FindIndex(cp => cp.State == CheckPointState.active);
        float value = (_checkPoints[index].transform.position.z - _checkPoints[0].transform.position.z) / _distance;
        _progressCheckPoints.fillAmount = value;

        if (cpcur.IsFinish)
        {
            IsWin = true;
            int score = (int)_score + (int)_scoreBonus;
            _text.text = $"{score}";
            _winScreen.SetActive(true);

            if (score > _scoreTotal)
            {
                //SetNewRecord(_scoreTotal);
                _scoreTotal = score;
            }
        }
    }

    public void CloseWinScreen()
    {
        IsWin = false;
    }

    public void SelectCheckPoint(int index)
    {
        if (_playerTransform != null)
        {
            _playerTransform.GetComponent<RobloxController>().ReturnToCheckPoint(_checkPoints[index].transform.position);
            _curvaluePrev = 0;
            AdsManager.Instance.ShowFullscreenButton();

            if (SettingScreen.IsActive)
            {
                SettingScreen.Instance.SwitchScreenState();
            }

            if (index == 0)
            {
                IsWin = false;
            }

            var cpcur = _checkPoints.Find(cp => cp.IsFinish);
            cpcur.gameObject.SetActive(true);
        }
    }
}

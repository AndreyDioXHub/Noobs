using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private TextMeshProUGUI _textChanged;
    [SerializeField]
    private RectTransform _changed;
    [SerializeField]
    private Vector2 _start;
    [SerializeField]
    private Vector2 _end;

    [SerializeField]
    private float _duration = 1, _durationCur;

    [SerializeField]
    private bool _wait;


    void Start()
    {
        _textChanged.CrossFadeAlpha(0, 0, true);

        if (_wait)
        {
            CoinManager.Instance.OnMoneyAdded.AddListener(Added);
            CoinManager.Instance.OnMoneyRemoved.AddListener(Removed);
            _wait = false;
        }
    }

    void Update()
    {

        _text.text = CoinManager.Instance.Coins.ToString();
    }

    private void OnEnable()
    {
        if (CoinManager.Instance == null)
        {
            _wait = true;
        }
        else
        {
            CoinManager.Instance.OnMoneyAdded.AddListener(Added);
            CoinManager.Instance.OnMoneyRemoved.AddListener(Removed);
        }

        _textChanged.CrossFadeAlpha(0, 0, true);
    }

    private void OnDisable()
    {
        if(CoinManager.Instance == null)
        {
            return;
        }

        CoinManager.Instance.OnMoneyAdded.RemoveListener(Added);
        CoinManager.Instance.OnMoneyRemoved.RemoveListener(Removed);
        StopAllCoroutines();
        _textChanged.CrossFadeAlpha(0, 0, true);
    }


    public void Added(int count)
    {
        StopAllCoroutines();
        _textChanged.text = $"+{count}";
        _durationCur = 0;
        _textChanged.CrossFadeAlpha(0, 0, true);
        _changed.anchoredPosition = Vector2.Lerp(_start, _end, 1);
        StartCoroutine(Coroutine(true));
    }

    public void Removed(int count)
    {
        StopAllCoroutines();
        _textChanged.text = $"-{count}";
        _durationCur = 0;
        _textChanged.CrossFadeAlpha(1, 0, true);
        _changed.anchoredPosition = Vector2.Lerp(_end, _start, 1);
        StartCoroutine(Coroutine(false));
    }

    IEnumerator Coroutine(bool up)
    {
        if (up)
        {
            _textChanged.CrossFadeAlpha(1, _duration, true);
        }
        else
        {
            _textChanged.CrossFadeAlpha(0, _duration, true);
        }

        while (_durationCur < _duration)
        {
            if (up)
            {
                _changed.anchoredPosition = Vector2.Lerp(_start, _end, _durationCur / _duration);
                //_textChanged
            }
            else
            {
                _changed.anchoredPosition = Vector2.Lerp(_end, _start, _durationCur / _duration);
            }
            _durationCur += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        _durationCur = 0;
        _textChanged.CrossFadeAlpha(0, 0, true);
    }
}

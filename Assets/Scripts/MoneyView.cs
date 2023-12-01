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

    void Start()
    {
        _textChanged.CrossFadeAlpha(0, 0, true);
        CoinManager.Instance.OnMoneyAdded.AddListener(Added);
        CoinManager.Instance.OnMoneyRemoved.AddListener(Removed);
    }

    void Update()
    {
        _text.text = CoinManager.Instance.Coins.ToString();
    }

    public void Added(int count)
    {
        StopAllCoroutines();
        _textChanged.text = $"+{count}";
        _durationCur = 0;
        _textChanged.CrossFadeAlpha(0, 0, true);
        _changed.anchoredPosition = Vector2.Lerp(_start, _end, 1);

        if (gameObject.activeSelf)
        {
            StartCoroutine(Coroutine(true));
        }
    }

    public void Removed(int count)
    {
        StopAllCoroutines();
        _textChanged.text = $"-{count}";
        _durationCur = 0;
        _textChanged.CrossFadeAlpha(1, 0, true);
        _changed.anchoredPosition = Vector2.Lerp(_end, _start, 1);

        if (gameObject.activeSelf)
        {
            StartCoroutine(Coroutine(false));
        }
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

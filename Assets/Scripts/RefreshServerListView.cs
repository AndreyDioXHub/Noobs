using cyraxchel.network;
using cyraxchel.network.server;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RefreshServerListView : MonoBehaviour
{
    public UnityEvent OnServersRefreshed = new UnityEvent();

    public static RefreshServerListView Instance;

    [SerializeField]
    private GameObject _timer;
    [SerializeField]
    private GameObject _button;

    [SerializeField]
    private TextMeshProUGUI _textTime;
    [SerializeField]
    private bool _isPaused;
    [SerializeField]
    private string _timeString;
    [SerializeField]
    private float _time, _timwCur;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {

        if (_isPaused)
        {
            _timer.SetActive(true);
            _button.SetActive(false);

            _textTime.text = _timeString;

            if (_timwCur < 10)
            {
                _timeString = $"0{ (int)_timwCur }/{_time}";
            }
            else
            {
                _timeString = $"{ (int)_timwCur }/{_time}";
            }

            _timwCur += Time.fixedDeltaTime;

            if(_timwCur > _time)
            {
                _timwCur = 0;
                _isPaused = false;
            }
        }
        else
        {
            _timer.SetActive(false);
            _button.SetActive(true);
        }
    }

    public void RefreshServers()
    {
        _timwCur = 0;
        _isPaused = true;
        OnServersRefreshed?.Invoke();
        ObstacleNetworkManager.singleton.gameObject.GetComponent<ConnectionSovler>()?.RefreshgameServerList();
    }
}

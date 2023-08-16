using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScreen : MonoBehaviour
{
    public static ConnectionScreen Instance;

    [SerializeField]
    private GameObject _screen;

    [SerializeField]
    private List<ConnectionStatusView> _connections = new List<ConnectionStatusView>();
    [SerializeField]
    private int _index = 0;
    [SerializeField]
    private float _time = 1;
    [SerializeField]
    private float _timeCur = 0;
    [SerializeField]
    private bool _needHide;


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
        ResetViews();
    }

    void Update()
    {
        if (_needHide)
        {
            _timeCur += Time.deltaTime;

            if(_timeCur > _time)
            {
                _timeCur = 0;
                _needHide = false;
                _screen.SetActive(false);
            }
        }
    }

    [ContextMenu("Collect")]
    public void Collect()
    {
        var tr = _screen.GetComponentsInChildren<ConnectionStatusView>();

        foreach(var t in tr)
        {
            _connections.Add(t);
        }
    }

    [ContextMenu("Show")]
    public void Show()
    {
        Show(1);
    }
    [ContextMenu("Reset")]
    public void ResetViews()
    {
        _index = 0;
        _timeCur = 0;
        _needHide = false;

        foreach (var v in _connections)
        {
            v.ResetView();
        }
    }


    public void Show(int status)
    {
        _connections[_index].Show(status);

        _index++;
        _index = _index > _connections.Count - 1 ? _connections.Count - 1 : _index;

        if (_index == _connections.Count - 1)
        {
            _needHide = true;
        }
    }
}

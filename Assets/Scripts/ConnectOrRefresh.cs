using cyraxchel.network.server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConnectOrRefresh : MonoBehaviour
{
    public static ConnectOrRefresh Instance;

    public UnityEvent<GameServerData> CallConnectToServer = new UnityEvent<GameServerData>();

    [SerializeField]
    private GameObject _refreshButton;
    [SerializeField]
    private GameObject _connectButton;
    [SerializeField]
    private GameObject _cantConnectButton;

    GameServerData _lastServer;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _connectButton.GetComponent<Button>().onClick.AddListener(TryConnectToServer);
    }

    private void TryConnectToServer() {
        CallConnectToServer?.Invoke(_lastServer);
    }

    void Update()
    {
    }

    public void Refresh()
    {
        _refreshButton.SetActive(true);
        _connectButton.SetActive(false);
        _cantConnectButton.SetActive(false);
    }

    public void Connect(GameServerData _server)
    {
        _lastServer = _server;
        _refreshButton.SetActive(false);
        _connectButton.SetActive(true);
        _cantConnectButton.SetActive(false);

    }


}

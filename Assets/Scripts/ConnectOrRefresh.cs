using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectOrRefresh : MonoBehaviour
{
    public static ConnectOrRefresh Instance;

    [SerializeField]
    private GameObject _refreshButton;
    [SerializeField]
    private GameObject _connectButton;
    [SerializeField]
    private GameObject _cantConnectButton;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
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

    public void Connect(bool serverFull)
    {
        _refreshButton.SetActive(false);
        _connectButton.SetActive(!serverFull);
        _cantConnectButton.SetActive(serverFull);
    }


}

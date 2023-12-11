using cyraxchel.network;
using cyraxchel.network.server;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ServerConnectButton : MonoBehaviour
{

    [SerializeField]
    private string _serverNameEn = "Server 1";
    [SerializeField]
    private string _serverNameRu = "Сервер 1";
    private string _serverName = "Сервер 1";
    [SerializeField]
    private TextMeshProUGUI _serverNameText;
    [SerializeField]
    private TextMeshProUGUI _serverFullnessText;
    [SerializeField]
    private int _count = 300;
    [SerializeField]
    private int _countCur = 3;

    private GameServerData _server;

    void Start()
    {
        RefreshServerListView.Instance.OnServersRefreshed.AddListener(Refreshstate);
        Refreshstate();
    }

    void Update()
    {
        
    }

    public void Init(GameServerData gmsrv) {
        _server = gmsrv;
        if(!int.TryParse(_server.Players,out _countCur)) {
            _countCur = -1;  //FOR TEST
        }
        if(!LocalizationStrings.Strings.TryGetValue(_server.Name, out _serverName)) {
            string _locale = PlayerPrefs.GetString(PlayerPrefsConsts.local, "ru");
            _serverName = _server.GetServerName(_locale);
        }
    }

    public void Connect()
    {
        ConnectOrRefresh.Instance.Connect(_server);
        
    }

    public void Refreshstate()
    {
        _serverNameText.text = _serverName;
        _serverFullnessText.text = _countCur>=0 ? $"{_countCur}/{_count}" : "";

    }
}

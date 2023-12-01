using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerConnectButton : MonoBehaviour
{

    [SerializeField]
    private string _serverNameEn = "Server 1";
    [SerializeField]
    private string _serverNameRu = "Сервер 1";
    [SerializeField]
    private TextMeshProUGUI _serverNameText;
    [SerializeField]
    private TextMeshProUGUI _serverFullnessText;
    [SerializeField]
    private int _count = 300;
    [SerializeField]
    private int _countCur = 3;


    void Start()
    {
        RefreshServerListView.Instance.OnServersRefreshed.AddListener(Refreshstate);
        Refreshstate();
    }

    void Update()
    {
        
    }

    public void Connect()
    {
        ConnectOrRefresh.Instance.Connect(_countCur == _count);
    }

    public void Refreshstate()
    {
        string local = PlayerPrefs.GetString(PlayerPrefsConsts.local, "ru");

        if (local.Equals("ru"))
        {
            _serverNameText.text = _serverNameRu;
        }
        else
        {
            _serverNameText.text = _serverNameEn;
        }
        _serverFullnessText.text = $"{_countCur}/{_count}";

    }
}

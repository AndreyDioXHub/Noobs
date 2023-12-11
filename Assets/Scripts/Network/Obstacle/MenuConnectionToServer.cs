using cyraxchel.network.server;
using Mirror;
using Mirror.SimpleWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConnectionToServer : MonoBehaviour
{
    [SerializeField]
    TMP_Text infoText;

    [SerializeField]
    Button cancelButton;

    [SerializeField]
    GameObject ConnectionPanel;

    [SerializeField]
    GameObject ErrorConnectPanel;

    float awaitTime = 0;

    SimpleWebTransport transport;

    string connmessage;

    Queue<GameServerData> serverData;

    int totalSeconds = 0;

    bool canselectserver = true;

    // Start is called before the first frame update
    void Start()
    {
        cancelButton.onClick.AddListener(CancelConnection);
        transport = (SimpleWebTransport)ObstacleNetworkManager.singleton.transport;
        transport.OnClientError += OnClientErrorConnect;

        string key = infoText.gameObject.GetComponent<TextLocalizer>().Key;
        LocalizationStrings.Strings.TryGetValue(key, out connmessage);
        if(ConnectionSovler.SharedGameServers != null && ConnectionSovler.SharedGameServers.Count > 0) {
            ApplyExternalList(ConnectionSovler.SharedGameServers);
        } else {
            var solver = ObstacleNetworkManager.singleton.gameObject.GetComponent<ConnectionSovler>();
            if (solver != null) {
                solver.OnReceiveListFromServer.AddListener(ApplyExternalList);
            }
        }
        
    }

    private void ApplyExternalList(List<GameServerData> newlist) {
        serverData = new Queue<GameServerData>(newlist);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountTimeConnection() {
        while (NetworkClient.isConnecting) {
            yield return new WaitForSeconds(1);
            totalSeconds++;
            infoText.text = $"{connmessage}.. {totalSeconds}";
        }
        OnClientErrorConnect();
    }

    public void PlayNetworkGame() {
        canselectserver = true;
        if (NetworkClient.isConnected || NetworkClient.isConnecting) return;
        Log.logger = new SilentLogger();
        infoText.text = connmessage;
        ConnectionPanel.SetActive(true);
        if(serverData.Count > 0) {
            var sdata = serverData.Dequeue();
            ObstacleNetworkManager.singleton.networkAddress = sdata.Address;
            if(!string.IsNullOrEmpty(sdata.Port)) {
                transport.Port = sdata.GetPort();
            }
        }
        ObstacleNetworkManager.singleton.StartClient();
        StartCoroutine(CountTimeConnection());


    }

    public void PlayNetworkGame(GameServerData prefferserver) {
        canselectserver = false;
        StopAllCoroutines();
        if (NetworkClient.isConnected || NetworkClient.isConnecting) return;
        Log.logger = new SilentLogger();
        infoText.text = connmessage;
        ConnectionPanel.SetActive(true);
        ObstacleNetworkManager.singleton.networkAddress = prefferserver.Address;
        if (!string.IsNullOrEmpty(prefferserver.Port)) {
            transport.Port = prefferserver.GetPort();
        }
        ObstacleNetworkManager.singleton.StartClient();
        StartCoroutine(CountTimeConnection());
    }

    public void PlayOfflineGame() 
    {
        SceneManager.LoadScene(2);
        //TODO
    }

    private void CancelConnection() {
        StopAllCoroutines();
        totalSeconds = 0;
        ConnectionPanel.SetActive(false);
        ObstacleNetworkManager.singleton.StopClient();
        Log.logger = Debug.unityLogger;
    }

    private void OnClientErrorConnect(TransportError error = 0, string arg2 = "") {
        StopAllCoroutines();
        if(HasAttempt()) {
            //Reconect to next
            PlayNetworkGame();
        } else {
            //Show Error panel
            totalSeconds = 0;
            ConnectionPanel.SetActive(false);
            ErrorConnectPanel.SetActive(true);
            Log.logger = Debug.unityLogger;
        }
    }

    private bool HasAttempt() {
        return serverData.Count>0 && canselectserver;
    }

    private void OnDestroy() {
        if(transport != null) transport.OnClientError -= OnClientErrorConnect;
        StopAllCoroutines();
        Log.logger = Debug.unityLogger;
    }
}

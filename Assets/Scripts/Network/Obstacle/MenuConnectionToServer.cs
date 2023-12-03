using Mirror;
using Mirror.SimpleWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        cancelButton.onClick.AddListener(CancelConnection);
        transport = (SimpleWebTransport)ObstacleNetworkManager.singleton.transport;
        transport.OnClientError += OnClientErrorConnect;

        string key = infoText.gameObject.GetComponent<TextLocalizer>().Key;
        Debug.Log(key);
        LocalizationStrings.Strings.TryGetValue(key, out connmessage);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountTimeConnection() {
        int seconds = 0;
        while (NetworkClient.isConnecting) {
            yield return new WaitForSeconds(1);
            seconds++;
            infoText.text = $"{connmessage}.. {seconds}";
        }
        OnClientErrorConnect();
    }

    public void PlayNetworkGame() {
        //TODO
        infoText.text = connmessage;
        ConnectionPanel.SetActive(true);
        ObstacleNetworkManager.singleton.StartClient();
        StartCoroutine(CountTimeConnection());
    }

    private void CancelConnection() {
        StopAllCoroutines();
        ConnectionPanel.SetActive(false);
        ObstacleNetworkManager.singleton.StopClient();
    }

    private void OnClientErrorConnect(TransportError error = 0, string arg2 = "") {
        StopAllCoroutines();
        ConnectionPanel.SetActive(false);
        ErrorConnectPanel.SetActive(true);
    }

    private void OnDestroy() {
        if(transport != null) transport.OnClientError -= OnClientErrorConnect;
        StopAllCoroutines();
    }
}

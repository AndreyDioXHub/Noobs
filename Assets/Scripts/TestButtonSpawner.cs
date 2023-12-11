using cyraxchel.network;
using cyraxchel.network.server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform conten;

    ConnectionSovler csolver;

    void Start()
    {
        
    }

    private void OnEnable() {
        Debug.Log("on enable called");
        if(ConnectionSovler.SharedGameServers!= null && ConnectionSovler.SharedGameServers.Count > 0) {
            RefreshServerList(ConnectionSovler.SharedGameServers);
        }
        if (csolver == null) csolver = ObstacleNetworkManager.singleton.gameObject.GetComponent<ConnectionSovler>();
        csolver.OnReceiveListFromServer.AddListener(RefreshServerList);
        Debug.Log("Refresh list subscribed");

    }

    private IEnumerator RefreshListLater() {
        yield return new WaitForEndOfFrame();

        RefreshServerList(NoobsLobby.Instance.Lobbies);
    }

    private void OnDisable() {
        if (csolver != null) {
            csolver.OnReceiveListFromServer.RemoveListener(RefreshServerList);
            Debug.Log("Refresh list UNsubscribed");
        }
    }

    private void RefreshServerList(List<GameServerData> servers) {
        Debug.Log("RefreshServerList receive");
        if(conten.childCount > 0) {
            while(conten.childCount > 0) {
                Destroy(conten.GetChild(0));
            }
        }
        for (int i = 0; i < servers.Count; i++) {
            var go = Instantiate(prefab, conten);
            var servbt = go.GetComponent<ServerConnectButton>();
            servbt.Init(servers[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

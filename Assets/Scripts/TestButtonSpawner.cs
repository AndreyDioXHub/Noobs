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

    void Start()
    {
        
    }

    private void OnEnable() {
        Debug.Log("on enable called");
        NoobsLobby.Instance.OnListLobbiesLoaded.AddListener(RefreshServerList);
        if(NoobsLobby.Instance.HasLobbies) {
            StartCoroutine(RefreshListLater());
        }
    }

    private IEnumerator RefreshListLater() {
        yield return new WaitForEndOfFrame();

        RefreshServerList(NoobsLobby.Instance.Lobbies);
    }

    private void OnDisable() {
        NoobsLobby.Instance.OnListLobbiesLoaded.RemoveListener(RefreshServerList);
    }

    private void RefreshServerList(List<GameServer> servers) {
        Debug.Log("RefreshServerList call");
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

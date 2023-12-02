using cyraxchel.network.server;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

namespace cyraxchel.network {
    public class NoobsLobby : MonoBehaviour {

        public UnityEvent OnAuthenticationFailed = new UnityEvent();
        public UnityEvent<List<GameServer>> OnListLobbiesLoaded = new UnityEvent<List<GameServer>>();
        public static NoobsLobby Instance { get; private set; }
        public bool HasLobbies { get => Lobbies != null && Lobbies.Any(); }

        public List<GameServer> Lobbies;

        Lobby currentLobby;

        private void Awake() {
            if(Application.platform == RuntimePlatform.LinuxServer || Application.platform == RuntimePlatform.WindowsServer) {
                //TODO Destroy
            }
            Instance = this;
        }

        // Start is called before the first frame update
        async void Start() {
            try {
                await ExecuteGetLobby();
            } catch(Exception ex) {
                Debug.LogError(ex.Message);
            }
        }

        async Task ExecuteGetLobby() {
            await UnityServices.InitializeAsync();

            Player loggedPlayer = await NoobsLobbyServer.GetPlayerFromAnonymousLoginAsync();
            
            if (loggedPlayer != null) {
                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(new QueryLobbiesOptions() {
                    Count = 10
                });
                List<Lobby> foundLobbies = queryResponse.Results;

                if(foundLobbies.Any()) {
                    Debug.Log("Found lobbies:\n" + JsonConvert.SerializeObject(foundLobbies));
                    Lobbies = new List<GameServer>();
                    foreach (var item in foundLobbies) {
                        string addr = "--";
                        DataObject dobj;
                        if(item.Data.TryGetValue(NoobsLobbyServer.D_ADDRESS, out dobj)) {
                            addr = dobj.Value;
                        }
                        string playersCount = "-1";
                        if(item.Data.TryGetValue(NoobsLobbyServer.D_PLAYERS_COUNT, out dobj)) {
                            playersCount = dobj.Value;
                        }
                        GameServer gs = new GameServer() {
                            Name = item.Name,
                            Id = item.Id,
                            Address = addr,
                            Players = playersCount
                        };
                        Lobbies.Add(gs);
                    }
                    OnListLobbiesLoaded?.Invoke(Lobbies);
                }

            } else {
                OnAuthenticationFailed?.Invoke();
            }
            
        }

        

        public async void RefreshLobbyListAsync() {
            try {
                await ExecuteGetLobby();
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message);
            }
        }

        private void OnDestroy() {
            
        }

        public void ConnectToServer(string id, string address) {
            Debug.Log($"Start Connect to server {address}. Id is {id}");
            ObstacleNetworkManager.singleton.networkAddress = address;
            ObstacleNetworkManager.singleton.StartClient();
        }
    }

}
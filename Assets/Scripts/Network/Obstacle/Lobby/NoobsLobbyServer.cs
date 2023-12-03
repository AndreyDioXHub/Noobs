using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace cyraxchel.network.server {
    public class NoobsLobbyServer : MonoBehaviour {

        #region Const
        public static readonly string D_ADDRESS = "Address";
        public static readonly string D_PORT = "Port";
        public static readonly string D_NAME = "Name";
        public static readonly string D_PLAYERS_COUNT = "Players";
        #endregion

        private Lobby serverLobby;


        [SerializeField]
        int maxPlayers = 100;

        GameServerData serverDataConfig;

        public async void StartLobby(GameServerData gameServerData) {
            serverDataConfig = gameServerData;

            try {
                await ExecuteServerLobby();
            } catch (Exception e) {
                Debug.Log(e.Message);   //TODO
            }
            ObstacleNetworkManager.singleton.ServerPlayerCountChanged.AddListener(OnPlayersCountChanged);
        }

        async Task ExecuteServerLobby() {
            await UnityServices.InitializeAsync();

            Player serverLoggedPlayer = await GetPlayerFromAnonymousLoginAsync();

            serverLoggedPlayer.Data.Add("Type", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Private, "server"));


            var lobbydata = new Dictionary<string, DataObject>() {
                [D_NAME] = new DataObject(DataObject.VisibilityOptions.Public, serverDataConfig.Name, DataObject.IndexOptions.S1),
                [D_ADDRESS] = new DataObject(DataObject.VisibilityOptions.Public, serverDataConfig.Address, DataObject.IndexOptions.S2),
                [D_PORT] = new DataObject(DataObject.VisibilityOptions.Public, serverDataConfig.Port , DataObject.IndexOptions.S3),
                [D_PLAYERS_COUNT] = new DataObject(DataObject.VisibilityOptions.Public, ObstacleNetworkManager.singleton.CurrentPlayersCount.ToString(), DataObject.IndexOptions.N1)
            };
            serverLobby = await LobbyService.Instance.CreateLobbyAsync(
                lobbyName: serverDataConfig.Name,
                maxPlayers: maxPlayers,
                options: new CreateLobbyOptions() {
                    //TODO
                    Data = lobbydata,
                    IsPrivate = false,
                    Player = serverLoggedPlayer
                });
            StartCoroutine(HeartbeatLobbyCoroutine(serverLobby.Id, 15));
            Debug.Log($"Create new lobby {serverLobby.Name} ({serverLobby.Id})");
            //await LobbyService.Instance.UpdateLobbyAsync()
        }

        IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) {
            var delay = new WaitForSecondsRealtime(waitTimeSeconds);
            bool lobbyExist = true;
            while (lobbyExist) {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
                lobbyExist = serverLobby != null;
            }
        }
        private async void OnPlayersCountChanged(int playercount) {
            Debug.Log($"Try update to new count {playercount}");
            if(serverLobby != null) {
                await UpdateLobbyData(playercount);
            }
        }

        private async Task UpdateLobbyData(int playercount) {
            serverLobby.Data[D_PLAYERS_COUNT] = new DataObject(DataObject.VisibilityOptions.Public, playercount.ToString(),DataObject.IndexOptions.N1);
            serverLobby = await LobbyService.Instance.UpdateLobbyAsync(
                lobbyId: serverLobby.Id,
                options: new UpdateLobbyOptions() {
                    Data = serverLobby.Data
                });
        }

        private async void OnDestroy() {
            if(serverLobby != null) {
                await LobbyService.Instance.DeleteLobbyAsync(serverLobby.Id);
            }
        }

        public static async Task<Player> GetPlayerFromAnonymousLoginAsync() {
            if (!AuthenticationService.Instance.IsSignedIn) {
                Debug.Log($"Trying to log in a player ...");

                // Use Unity Authentication to log in
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (!AuthenticationService.Instance.IsSignedIn) {
                    throw new InvalidOperationException("Player was not signed in successfully; unable to continue without a logged in player");
                }
            }

            Debug.Log("Player signed in as " + AuthenticationService.Instance.PlayerId);

            // Player objects have Get-only properties, so you need to initialize the data bag here if you want to use it
            return new Player(AuthenticationService.Instance.PlayerId, null, data: new Dictionary<string, PlayerDataObject>());
        }
    }
}
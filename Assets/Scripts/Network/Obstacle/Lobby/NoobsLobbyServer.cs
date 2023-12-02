using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace cyraxchel.network.server {
    public class NoobsLobbyServer : MonoBehaviour {

        private Lobby serverLobby;

        public async void StartLobby() {
            try {
                await ExecuteServerLobby();
            } catch (Exception e) {
                Debug.Log(e.Message);   //TODO
            }
        }

        async Task ExecuteServerLobby() {
            await UnityServices.InitializeAsync();



        }
    }
}
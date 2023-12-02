using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyraxchel.network.server {
    public class ServerDisabler : NetworkBehaviour {


        [SerializeField] GameObject target;

        [SerializeField] NoobsLobbyServer serverLobby;

        public override void OnStartServer() {
            base.OnStartServer();
            serverLobby.StartLobby();
        }

        public override void OnStopServer() {
            base.OnStopServer();

        }


    }
}
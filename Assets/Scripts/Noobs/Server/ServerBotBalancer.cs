using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyraxchel.network.server {
    public class ServerBotBalancer : MonoBehaviour {
        public static ServerBotBalancer Instance { get; private set; }

        internal void GetBotForGame(ServerGame serverGame, int requiredNumberBots) {
            //TODO Assign bots for this game
        }

        internal void ReleaseBotsFromGame(ServerGame serverGame) {
            //TODO Remove bots from this game
        }

        void Awake() {
            Instance = this;
        }

        public void Start() {
            //TODO Add bots

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
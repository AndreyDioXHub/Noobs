using cyraxchel.network.rooms;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace cyraxchel.network.server 
{
    public class ServerBotBalancer : MonoBehaviour {

        
        [SerializeField]
        private GameObject _botPrefab;

        private Dictionary<ServerGame, List<GameObject>> _reservedBots = new Dictionary<ServerGame, List<GameObject>>();

        [SerializeField]
        private Vector3 outagePosition = new Vector3(5000, 0, 5000);

        
        public GameObject BotPrefab { get => _botPrefab; set => _botPrefab = value; }


        [ContextMenu("Generate 15 bots in main scene")]
        public void OnCanvasGroupChanged() {
            //Spawner.SpawnBots(gameObject.scene, 15);
        }
    }
}


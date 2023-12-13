using Mirror;
using Mirror.SimpleWeb;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace cyraxchel.network.server {

    public class ServerConfiguration : MonoBehaviour {
        
        [SerializeField]
        string configfile = "server.cfg";

        NetworkManager networkManager;

        [SerializeField]
        ServerConfigItem serverConfigItem;

        // Start is called before the first frame update
        void Start() {
            CheckConfig();
        }

        private void CheckConfig() {
#if UNITY_SERVER
            string path = Path.Combine(Application.dataPath, configfile);
            ObstacleNetworkManager.singleton.ServerPlayerCountChanged.AddListener(SendPlayerCountChanged);
            if (File.Exists(path)) {
                string configtext = File.ReadAllText(path);
                try {
                    serverConfigItem = JsonConvert.DeserializeObject<ServerConfigItem>(configtext);
                } catch {
                    RunServer();
                    return;
                }
                var nmanager = NetworkManager.singleton;
                nmanager.maxConnections = serverConfigItem.MaxPlayers;
                var transport = (SimpleWebTransport)nmanager.transport;
                if (serverConfigItem.Port > 0) transport.port = serverConfigItem.Port;
                transport.ClientUseDefaultPort = serverConfigItem.UseDefaultPort;
                transport.clientUseWss = serverConfigItem.UseWss;
            } else {
            Debug.Log("Server config not founded");
                RunServer();
            }
        
#endif

        }

        private void SendPlayerCountChanged(int playercount) {
            //TODO Send to rest api
        }

        void RunServer() {
            NetworkManager.singleton.StartServer();
        }

        [ContextMenu("Export config")]
        void ExportConfig() {
#if UNITY_EDITOR
            if(serverConfigItem != null) {
                string exportconfig = JsonConvert.SerializeObject(serverConfigItem, Formatting.Indented);
                string path = EditorUtility.SaveFilePanel("Export config", Application.dataPath, "config.cfg", ",cfg");
                File.WriteAllText(path, exportconfig);
            }
#endif
        }
    }

    [Serializable]
    public class ServerConfigItem {
        [JsonProperty("name"), SerializeField]
        public string ServerName;
        [JsonProperty("port"), SerializeField]
        public ushort Port;
        [JsonProperty("maxplayers"), SerializeField]
        public int MaxPlayers;
        [JsonProperty("usedefaultport"), SerializeField]
        public bool UseDefaultPort = false;
        [JsonProperty("usewss"), SerializeField]
        public bool UseWss = false;
    }
}
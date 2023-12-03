using Mirror;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace cyraxchel.network.server {
    public class ServerDisabler : NetworkBehaviour {


        [SerializeField] GameObject target;

        [SerializeField] NoobsLobbyServer serverLobby;

        [SerializeField]
        string configFile = "config.cfg";

        [SerializeField]
        GameServerData toSaveData;

        public override void OnStartServer() {
            base.OnStartServer();
            //GameServerData gameServerData = LoadDefaults();
            //serverLobby.StartLobby(gameServerData);
        }

        private GameServerData LoadDefaults() {
            GameServerData g = new GameServerData();
            string path = Path.Combine(Application.persistentDataPath, configFile);
            if (File.Exists(path)) {
                string f = File.ReadAllText(path);
                g = JsonConvert.DeserializeObject<GameServerData>(f);
            }

            return g;
        }

        public override void OnStopServer() {
            base.OnStopServer();

        }

        [ContextMenu("Save data to config")]
        private void SaveDataToConfig() {
#if UNITY_EDITOR
            if(toSaveData != null) {
                string content = JsonConvert.SerializeObject(toSaveData, Formatting.Indented);
                string path = EditorUtility.SaveFilePanel("Save config", Application.persistentDataPath,  configFile, ".cfg");
                if(!string.IsNullOrWhiteSpace(content)) {
                    File.WriteAllText(path, content);
                    EditorUtility.DisplayDialog("Done", $"Файл {configFile} сохранен по пути: {path}", "OK");
                }
            }
#endif
        }

    }
}
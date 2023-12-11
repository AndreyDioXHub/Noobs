using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


namespace cyraxchel.network.server {
    public class ConnectionSovler : MonoBehaviour {

        #region Events
        //Начинаем поиск списка серверов
        public UnityEvent BeginSearchServers;
        public UnityEvent<int> OnSendRequiesToServer;

        public UnityEvent<List<GameServerData>> OnReceiveListFromServer;


        #endregion

        [SerializeField]
        List<string> DataServers;       //Список серверов-источников адресов игровых серверов

        [SerializeField]
        List<GameServerData> GameServers;

        public static List<GameServerData> SharedGameServers { get; private set; }

        const string LAST_SERVER = "cs_last_server";
        const string NAME_SERVER_NODE = "servers";

        string ServerDataPublish = "";  //Where publish server status
        [SerializeField]
        string _netServersList = "";  //Current Address to published servers
        public string NetServersListRaw { get => _netServersList; 
            set {
                _netServersList = value;
                PlayerPrefs.SetString(LAST_SERVER, value);
                PlayerPrefs.Save();
            } }  //Current Address to published servers


        // Start is called before the first frame update
        void Start() {
            //For IL2CPP optimization. Include this code
            GameServers = new List<GameServerData>();
            GameServerData itemеtest = new GameServerData();
            GameServers.Add(itemеtest);

            if (CheckNotServerPlatform()) {
                //string last = PlayerPrefs.GetString(LAST_SERVER, string.Empty);
                //if (!string.IsNullOrEmpty(last)) {
                 //   DataServers.Insert(0, last);        // Первым ставлю последний использованный сервер.
                //}
                StartCoroutine(GetServers());
            } else {
                //Require get publish data list
            }
        }

        public static bool CheckNotServerPlatform() {
            bool isServer = Application.platform == RuntimePlatform.LinuxServer;
            isServer = isServer && Application.platform == RuntimePlatform.WindowsServer;
            return !isServer;
        }

        private IEnumerator GetServers() {
            BeginSearchServers?.Invoke();
            bool listfounded = false;
            int i = 0;
            while(!listfounded) {
                if(i >= DataServers.Count) { 
                    //TODO Call error
                    break; 
                }
                string url = DataServers[i];
                UnityWebRequest www = UnityWebRequest.Get(url);
                OnSendRequiesToServer?.Invoke(i);
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success) {
                    //TODO string serverslist = www.downloadHandler.data;
                    NetServersListRaw = www.downloadHandler.text;
                    ParseData(NetServersListRaw);
                    Debug.Log($"Data success: {NetServersListRaw}");
                    listfounded = true;
                    break;
                } else {
                    i++;
                    
                }

            }
            yield return null;
        }

        private void ParseData(string netServersList) {
            GameServers = JsonConvert.DeserializeObject<List<GameServerData>>(netServersList);
            SharedGameServers = GameServers;
            OnReceiveListFromServer?.Invoke(GameServers);

            /*JObject jo = JObject.Parse(netServersList);
            if(jo.ContainsKey(NAME_SERVER_NODE)) {
                GameServers = JsonConvert.DeserializeObject<List<GameServerData>>(jo[NAME_SERVER_NODE].ToString());
                //TODO
                SharedGameServers = GameServers;
                OnReceiveListFromServer?.Invoke(GameServers);
            }/**/
        }

        [ContextMenu("Export Game list")]
        private void ExportGameList() {
#if UNITY_EDITOR
            if (GameServers != null) {
                
                string gs = JsonConvert.SerializeObject(GameServers);
                var path = EditorUtility.SaveFilePanel("Save Game servers", Application.dataPath, "gameNodes.json", "json");
                if(!string.IsNullOrEmpty(path)) {
                    File.WriteAllText(path, gs);
                    EditorUtility.DisplayDialog("Saved",$"File saved at path: \n{ path}","OK");
                }
            }
#endif
        }

        public void RefreshgameServerList() {
            StopAllCoroutines();
            StartCoroutine(GetServers());
        }
    }
}
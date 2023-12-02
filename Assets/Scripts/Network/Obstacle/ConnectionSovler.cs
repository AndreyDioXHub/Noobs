using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


namespace cyraxchel.network.server {
    public class ConnectionSovler : MonoBehaviour {

        #region Events
        //Ќачинаем поиск списка серверов
        public UnityEvent BeginSearchServers;
        public UnityEvent<int> OnSendRequiesToServer;


        #endregion

        [SerializeField]
        List<string> DataServers;       //—писок серверов-источников адресов игровых серверов

        [SerializeField]
        List<GameServer> GameServers;

        const string LAST_SERVER = "cs_last_server";

        string ServerDataPublish = "";  //Where publish server status
        [SerializeField]
        string _netServersList = "";  //Current Address to published servers
        public string NetServersList { get => _netServersList; 
            set {
                _netServersList = value;
                PlayerPrefs.SetString(LAST_SERVER, value);
                PlayerPrefs.Save();
            } }  //Current Address to published servers


        // Start is called before the first frame update
        void Start() {
            if (CheckNotServerPlatform()) {
                string last = PlayerPrefs.GetString(LAST_SERVER, string.Empty);
                if (!string.IsNullOrEmpty(last)) {
                    DataServers.Insert(0, last);
                }
                StartCoroutine(GetServers());
            } else {
                //Require get publish data list
            }
        }

        private bool CheckNotServerPlatform() {
            bool isServer = Application.platform == RuntimePlatform.LinuxServer;
            isServer = isServer && Application.platform == RuntimePlatform.WindowsServer;
            return !isServer;
        }

        private IEnumerator GetServers() {
            BeginSearchServers?.Invoke();
            bool listfounded = false;
            int i = 0;
            while(!listfounded) {
                string url = DataServers[i];
                UnityWebRequest www = UnityWebRequest.Get(url);
                OnSendRequiesToServer?.Invoke(i);
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success) {
                    //TODO string serverslist = www.downloadHandler.data;
                    NetServersList = www.downloadHandler.text;
                    Debug.Log($"Data success: {NetServersList}");
                    listfounded = true;
                    break;
                } else {
                    i++;
                }

            }
            yield return null;
        }

    }
}
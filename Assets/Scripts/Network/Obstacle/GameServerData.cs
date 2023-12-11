using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace cyraxchel.network.server {
    [Serializable]
    public class GameServerData {
        [JsonProperty("name")]
        public string Name = string.Empty;
        //TODO Name by locals
        [JsonProperty("localenames")]
        public Dictionary<string, string> Names;
        [JsonProperty("addr")]
        public string Address = string.Empty;
        [JsonProperty("port")]
        public string Port = string.Empty;
        [JsonProperty("players")]
        public string Players = string.Empty;
        [JsonProperty("id")]
        public string Id = string.Empty;

        [JsonIgnore]
        public string FullAddress { get => $"{Address}:{Port}"; }
        public string GetServerName(string locale) {
            string _name;
            if(Names == null || !Names.TryGetValue(locale, out _name)) {
                _name = Name;
            }
            return _name;
        }

        public ushort GetPort() {
            if (!string.IsNullOrEmpty(Port)) {
                return ushort.Parse(Port);
            } else {
                return 7777;    //Default port
            }
        }
    }
}
using System;
using System.Collections.Generic;


namespace cyraxchel.network.server {
    [Serializable]
    public class GameServerData {
        public string Name = string.Empty;
        //TODO Name by locals
        public Dictionary<string, string> Names;
        public string Address = string.Empty;
        public string Port = string.Empty;
        public string Players = string.Empty;
        public string Id = string.Empty;

        public string FullAddress { get => $"{Address}:{Port}"; }
        public string GetServerName(string locale) {
            string _name = string.Empty;
            Names.TryGetValue(locale, out _name);
            return _name;
        }
    }
}
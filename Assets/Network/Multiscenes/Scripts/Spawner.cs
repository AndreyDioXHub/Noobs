using cyraxchel.network.server;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cyraxchel.network.rooms {
    internal class Spawner
    {
        [ServerCallback]
        internal static void InitialSpawn(Scene scene)
        {
            foreach (var item in MultiRoomsNetManager.singleton.SpawnsObjects) {
                Vector3 spawnPosition = new Vector3(Random.Range(-19, 20), 1, Random.Range(-19, 20));
                GameObject spawngo = Object.Instantiate(item, spawnPosition, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(spawngo, scene);
                NetworkServer.Spawn(spawngo);
            }
        }

        [ServerCallback]
        internal static List<GameObject> SpawnBots(Scene scene, int count) {
            //TODO Spawn bots in selected scene
            List<GameObject> _bots = new List<GameObject>();
            for (int i = 0; i < count; i++) {
                Vector3 botstartposition = Random.insideUnitSphere * LevelConfig.Instance.STAGE_RADIUS;
                botstartposition.y = LevelConfig.Instance.START_PLAYER_YPOS;
                GameObject _botprefab = MultiRoomsNetManager.singleton.gameObject.GetComponent<ServerBotBalancer>().BotPrefab;
                GameObject spawnbot = Object.Instantiate(_botprefab, botstartposition, Quaternion.identity);
                _bots.Add(spawnbot);
                SceneManager.MoveGameObjectToScene(_botprefab, scene);
                NetworkServer.Spawn(spawnbot);
            }
            return _bots;
        }
    }
}

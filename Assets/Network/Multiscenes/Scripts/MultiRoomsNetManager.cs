using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cyraxchel.network.rooms
{
    [AddComponentMenu("")]
    public class MultiRoomsNetManager : NetworkManager
    {
        [Header("Spawner Setup")]
        [Tooltip("Prefabs for the Spawner")]
        public GameObject[] SpawnsObjects;

        [Header("MultiScene Setup")]
        public int instances = 3;

        [Scene]
        public string gameScene;

        // This is set true after server loads all subscene instances
        bool subscenesLoaded;

        // subscenes are added to this list as they're loaded
        readonly List<Scene> subScenes = new List<Scene>();

        // Sequential index used in round-robin deployment of players into instances and score positioning
        int clientIndex;

        Dictionary<Scene, MultiRoomsGameManager> gameManagers;

        public static new MultiRoomsNetManager singleton { get; private set; }

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;
            SetupClient();
            gameManagers = new Dictionary<Scene, MultiRoomsGameManager>();
        }

        #region Server System Callbacks

        /// <summary>
        /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            StartCoroutine(OnServerAddPlayerDelayed(conn));
        }

        // This delay is mostly for the host player that loads too fast for the
        // server to have subscenes async loaded from OnStartServer ahead of it.
        IEnumerator OnServerAddPlayerDelayed(NetworkConnectionToClient conn)
        {
            // wait for server to async load all subscenes for game instances
            while (!subscenesLoaded)
                yield return null;

            // Send Scene message to client to additively load the game scene
            conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });

            // Wait for end of frame before adding the player to ensure Scene Message goes first
            yield return new WaitForEndOfFrame();

            base.OnServerAddPlayer(conn);

            // Do this only on server, not on clients
            // This is what allows the NetworkSceneChecker on player and scene objects
            // to isolate matches per scene instance on server.
            bool added = false;
            foreach(var pair in gameManagers) {
                if(pair.Value.game.CanAcceptPlayer) {
                    pair.Value.game.AddPlayer(conn.identity.gameObject, clientIndex);
                    SceneManager.MoveGameObjectToScene(conn.identity.gameObject, pair.Value.game.CurrentScene);
                    added = true;
                    break;
                }
            }
            if (added) {
                clientIndex++;
            } else { 
                var gtype = new GameTypeMessage();
                gtype.UseofflineMode = true;
                conn.Send(gtype);
            }

            
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn) {
            Scene playerScene = conn.identity.gameObject.scene;
            if(gameManagers.ContainsKey(playerScene)) {
                var gmanager = gameManagers[playerScene];
                gmanager.Cmd_PlayerDisConnected();
            }
            Debug.Log("Player Disconnected!");
            Debug.Log($"GO:{conn.identity.gameObject}");
            base.OnServerDisconnect(conn);
        }

        #endregion

        #region Start & Stop Callbacks

        /// <summary>
        /// This is invoked when a server is started - including when a host is started.
        /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
        /// </summary>
        public override void OnStartServer()
        {
            StartCoroutine(ServerLoadSubScenes());
        }

        // We're additively loading scenes, so GetSceneAt(0) will return the main "container" scene,
        // therefore we start the index at one and loop through instances value inclusively.
        // If instances is zero, the loop is bypassed entirely.
        IEnumerator ServerLoadSubScenes()
        {
            for (int index = 1; index <= instances; index++)
            {
                yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

                Scene newScene = SceneManager.GetSceneAt(index);
                subScenes.Add(newScene);
                Spawner.InitialSpawn(newScene);
            }

            subscenesLoaded = true;
        }

        /// <summary>
        /// This is called when a server is stopped - including when a host is stopped.
        /// </summary>
        public override void OnStopServer()
        {
            NetworkServer.SendToAll(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.UnloadAdditive });
            StartCoroutine(ServerUnloadSubScenes());
            clientIndex = 0;
        }

        // Unload the subScenes and unused assets and clear the subScenes list.
        IEnumerator ServerUnloadSubScenes()
        {
            for (int index = 0; index < subScenes.Count; index++)
                if (subScenes[index].IsValid())
                    yield return SceneManager.UnloadSceneAsync(subScenes[index]);

            subScenes.Clear();
            subscenesLoaded = false;

            yield return Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// This is called when a client is stopped.
        /// </summary>
        public override void OnStopClient()
        {
            // Make sure we're not in ServerOnly mode now after stopping host client
            if (mode == NetworkManagerMode.Offline)
                StartCoroutine(ClientUnloadSubScenes());
        }

        // Unload all but the active scene, which is the "container" scene
        IEnumerator ClientUnloadSubScenes()
        {
            for (int index = 0; index < SceneManager.sceneCount; index++)
                if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
                    yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
        }

        #endregion

        #region Server GameManagment

        [ServerCallback]
        public void ReloadLevel(Scene scene) {
            //Выгрузить сцену
            StartCoroutine(ReloadLevelCoroutine(scene));
        }

        IEnumerator ReloadLevelCoroutine(Scene scene) {
            int sceneindex = subScenes.IndexOf(scene);
            yield return SceneManager.UnloadSceneAsync((Scene)scene);

            Debug.Log($"Scene {sceneindex}<b> unloaded!</b>");
            //Load new one
            //TODO
            yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount-1);    //Last scene
            subScenes[sceneindex] = newScene;

            Debug.Log($"Scene {sceneindex}<b> loaded!</b>");

            Spawner.InitialSpawn(newScene);

            Debug.Log($"Scene {sceneindex} objects <b>Spawned!</b>");
        }


        [ServerCallback]
        public void RegisterGameManager(MultiRoomsGameManager gameManager) {
            if(gameManagers.ContainsKey(gameManager.game.CurrentScene)) {
                gameManagers[gameManager.game.CurrentScene] = gameManager;
            } else {
                gameManagers.Add(gameManager.game.CurrentScene, gameManager);
            }
        }


        internal void UnregisterGameManager(MultiRoomsGameManager gameManager) {
            if (gameManagers.ContainsKey(gameManager.game.CurrentScene)) {
                gameManagers.Remove(gameManager.game.CurrentScene);
            }
        }

        public void SetupClient() {
            NetworkClient.RegisterHandler<GameTypeMessage>(OnGameTypeChange);
        }

        public void OnGameTypeChange(GameTypeMessage message) {
            Debug.Log($"{nameof(OnGameTypeChange)} receive message. User offline mode: {message.UseofflineMode}");
            if (message.UseofflineMode) {
                Debug.Log("Set User to offline mode!");
                //TODO
                StopClient();
                return;
            }
        }


        public struct GameTypeMessage : NetworkMessage {
            public bool UseofflineMode;
        }

        #endregion

        public static bool IsServer { get => NetworkServer.active && !NetworkClient.activeHost; }
    }
}


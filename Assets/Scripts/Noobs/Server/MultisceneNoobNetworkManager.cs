using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;
using System.Collections;
using cyraxchel.network.server;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class MultisceneNoobNetworkManager : NetworkManager
{

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

    // Overrides the base singleton so we don't
    // have to cast to this type everywhere.
    public static new MultisceneNoobNetworkManager singleton { get; private set; }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;
        SetupClient();
    }

    #region Unity Callbacks

    public override void OnValidate()
    {
        base.OnValidate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Start & Stop

    /// <summary>
    /// Set the frame rate for a headless server.
    /// <para>Override if you wish to disable the behavior or set your own tick rate.</para>
    /// </summary>
    public override void ConfigureHeadlessFrameRate()
    {
        base.ConfigureHeadlessFrameRate();
    }

    /// <summary>
    /// called when quitting the application by closing the window / pressing stop in the editor
    /// </summary>
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    #endregion

    #region Scene Management

    /// <summary>
    /// This causes the server to switch scenes and sets the networkSceneName.
    /// <para>Clients that connect to this server will automatically switch to this scene. This is called automatically if onlineScene or offlineScene are set, but it can be called from user code to switch scenes again while the game is in progress. This automatically sets clients to be not-ready. The clients must call NetworkClient.Ready() again to participate in the new scene.</para>
    /// </summary>
    /// <param name="newSceneName"></param>
    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    /// <summary>
    /// Called from ServerChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows server to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    public override void OnServerChangeScene(string newSceneName) { }

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName) { }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) { }

    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
    }

    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnectionToClient conn) { }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
    }

    /// <summary>
    /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        StartCoroutine(OnServerAddPlayerDelayed(conn));
    }

    // This delay is mostly for the host player that loads too fast for the
    // server to have subscenes async loaded from OnStartServer ahead of it.
    IEnumerator OnServerAddPlayerDelayed(NetworkConnectionToClient conn) {
        // wait for server to async load all subscenes for game instances
        while (!subscenesLoaded)
            yield return null;

        

        int sceneToLoad = ServerNetworkBehaviour.Instance.GetSceneForPlayer(conn);
        if (sceneToLoad == -1) {
            //Не нашли свободный уровень. Надо сообщить игроку, чтобы отключился
            Debug.Log($"No free rooms. {conn.identity.netId} will be disconnect");
            
            var gtype = new GameTypeMessage();
            gtype.UseofflineMode = true;
            conn.Send(gtype);
            ServerNetworkBehaviour.Instance.UnregisterUser(conn, sceneToLoad);
            conn.Disconnect();

            yield return null;
        }
        // Вызов загрузки у пользователя игрового уровня.
        conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });

        // Wait for end of frame before adding the player to ensure Scene Message goes first
        yield return new WaitForEndOfFrame();

        // Регистрация пользователя 
        ServerNetworkBehaviour.Instance.RegisterUser(conn, sceneToLoad);


        base.OnServerAddPlayer(conn);

        //PlayerScore playerScore = conn.identity.GetComponent<PlayerScore>();
        //playerScore.playerNumber = clientIndex;
        //playerScore.scoreIndex = clientIndex / subScenes.Count;
        //playerScore.matchIndex = clientIndex % subScenes.Count;

        // Do this only on server, not on clients
        // This is what allows the NetworkSceneChecker on player and scene objects
        // to isolate matches per scene instance on server.
        if (subScenes.Count > 0)
            //SceneManager.MoveGameObjectToScene(conn.identity.gameObject, subScenes[clientIndex % subScenes.Count]);
            SceneManager.MoveGameObjectToScene(conn.identity.gameObject, subScenes[sceneToLoad]);

        clientIndex++;
    }


    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        clientIndex--;
        Debug.Log("OnServerDisconnect invoke");
        base.OnServerDisconnect(conn);
    }

    /// <summary>
    /// Called on server when transport raises an error.
    /// <para>NetworkConnection may be null.</para>
    /// </summary>
    /// <param name="conn">Connection of the client...may be null</param>
    /// <param name="transportError">TransportError enum</param>
    /// <param name="message">String message of the error.</param>
    public override void OnServerError(NetworkConnectionToClient conn, TransportError transportError, string message) { }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    public override void OnClientDisconnect() {
        Debug.Log($"{nameof(OnClientDisconnect)} invoked!");
        // Здесь будет событие о переходе в режим сингл-плеера
        if(mode == NetworkManagerMode.ClientOnly) {
            //Check offile game start
        }
    }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    public override void OnClientNotReady() { }

    /// <summary>
    /// Called on client when transport raises an error.</summary>
    /// </summary>
    /// <param name="transportError">TransportError enum.</param>
    /// <param name="message">String message of the error.</param>
    public override void OnClientError(TransportError transportError, string message) { }

    #endregion

    #region Start & Stop Callbacks

    // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
    // their functionality, users would need override all the versions. Instead these callbacks are invoked
    // from all versions, so users only need to implement this one case.

    /// <summary>
    /// This is invoked when a host is started.
    /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartHost() { }

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartServer() {
        StartCoroutine(ServerLoadSubScenes());
    }

    // We're additively loading scenes, so GetSceneAt(0) will return the main "container" scene,
    // therefore we start the index at one and loop through instances value inclusively.
    // If instances is zero, the loop is bypassed entirely.
    IEnumerator ServerLoadSubScenes() {
        for (int index = 1; index <= instances; index++) {
            yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

            Scene newScene = SceneManager.GetSceneAt(index);
            subScenes.Add(newScene);
            //Spawner.InitialSpawn(newScene);
            //TODO Fill bots in scene
        }

        subscenesLoaded = true;
    }

    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is called when a host is stopped.
    /// </summary>
    public override void OnStopHost() { }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer() {
        NetworkServer.SendToAll(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.UnloadAdditive });
        StartCoroutine(ServerUnloadSubScenes());
        clientIndex = 0;
    }

    // Unload the subScenes and unused assets and clear the subScenes list.
    IEnumerator ServerUnloadSubScenes() {
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
    public override void OnStopClient() { }

    #endregion

    #region Game Flow

    /// <summary>
    /// Игра на выбранной сцене завершена. Необходимо перегрузить сцену с уровнем.
    /// Подчистить ботов
    /// </summary>
    /// <param name="gameIndex"></param>
    public void GameComplete(int gameIndex) {
        // Высвободить игровых ботов с этой сцены.
        Spawner.Instance.ReleaseBots(gameIndex);
        // Выгрузить сцену
        StartCoroutine(RestartLevel(gameIndex));
    }

    IEnumerator RestartLevel(int level) {
        yield return SceneManager.UnloadSceneAsync(subScenes[level]);

        //И снова загрузить

        yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });


    }

    public struct GameTypeMessage : NetworkMessage {
        public bool UseofflineMode;
    }

    public void SetupClient() {
        NetworkClient.RegisterHandler<GameTypeMessage>(OnGameTypeChange);
    }
    public void OnGameTypeChange(GameTypeMessage message) {
        Debug.Log($"{nameof(OnGameTypeChange)} receive message. User offline mode: {message.UseofflineMode}");
        if(message.UseofflineMode) {
            Debug.Log("User offline mode!");
        }
    }
    #endregion
}

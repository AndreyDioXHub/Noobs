using cyraxchel.network.server;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class ServerNetworkBehaviour : NetworkBehaviour {
    public static ServerNetworkBehaviour Instance { get; private set; }

    [SerializeField]
    List<ServerGame> serverGames = new List<ServerGame>();


    #region Unity Callbacks

    /// <summary>
    /// Add your validation code here after the base.OnValidate(); call.
    /// </summary>
    protected override void OnValidate() {
        base.OnValidate();
    }

    private void Awake() {
        Instance = this;
    }

    #endregion



    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() {
        serverGames = new List<ServerGame>();
        int roomCounts = MultisceneNoobNetworkManager.singleton.instances;
        for (int i = 0; i < roomCounts; i++) {
            var sgame = new ServerGame();
            serverGames.Add(sgame);
            sgame.GameStatusChanged += OnServerGameStatusChanged;
        }
    }

    private void OnServerGameStatusChanged(ServerGame game, ServerGame.Status status) {

    }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() { }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnectionToClient parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    internal int GetSceneForPlayer(NetworkConnectionToClient conn) {
        //Логика выбора сцены для пользователя
        int firstEmpty = -1;
        for (int i = 0; i < serverGames.Count; i++) {
            var game = serverGames[i];
            if (game.CanAcceptPlayer) {
                game.ReservePlayerSlot(conn);
                return i;
            } else if (game.GameStatus == ServerGame.Status.None) {
                firstEmpty = i;
            }
        }

        if (firstEmpty > -1) {
            var game = serverGames[firstEmpty];
            //TODO Get offset
            game.Init(GetSceneOffset(firstEmpty));
            game.ReservePlayerSlot(conn);

            //game.AddPlayer(conn.identity.gameObject.name, (int)conn.identity.netId);
            return firstEmpty;
        }

        return -1;
    }



    internal bool RegisterUser(NetworkConnectionToClient conn, int sceneIndex) {
        var game = serverGames[sceneIndex];
        var noobchar = conn.identity.gameObject.GetComponent<NoobNetworkBehaviour>();
        return game.AddReservedPlayer(noobchar.UserName, conn.connectionId);
    }

    internal void UnregisterUser(NetworkConnectionToClient conn, int sceneToLoad) {
        var game = serverGames[sceneToLoad];
        game.FreeSlot(conn);

    }
    #endregion

    public static Vector3 GetSceneOffset(int index) {
        float _gridStep = LevelConfig.Instance.GridStep;
        return Vector3.forward * _gridStep * index;
    }

    /// <summary>
    /// Регистрация игрового менеджера
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gameManager"></param>
    /// <exception cref="NotImplementedException"></exception>
    internal void RegisterGameManager(Scene scene, GameManager gameManager) {
        foreach (var game in serverGames) {
            if (game.CurrenScene == scene) {
                game.CurrentGameManager = gameManager;
            }
        }
    }

    internal void SetSceneToGame(Scene scene, int level) {
        serverGames[level].CurrenScene = scene;
    }
}

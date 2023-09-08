using System.Collections.Generic;
using UnityEngine;
using Mirror;
using cyraxchel.network.server;
using System;
using System.Collections;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

namespace cyraxchel.network.rooms {

    public class MultiRoomsGameManager : NetworkBehaviour {

        public event Action<int> PlayersCountChange = delegate { };

        public static MultiRoomsGameManager Instance { get; private set; } = null;
        public ServerGame game { get; set; }

        [SyncVar(hook = nameof(hookPlayerCountChanged))]
        public int playerscount = 0;

        [Header("Overrides:")]
        [Tooltip("���� ����, �� �������� �� LevelConfig")]
        [SerializeField]
        internal int MaxPlayerInGame = 0;


        private void hookPlayerCountChanged(int oldvalue, int newvalue) {
            Debug.Log($"Player count changed to {newvalue}");
            PlayersCountChange?.Invoke(newvalue);
        }

        [ClientCallback]
        private void UpdatePlayersValue(int newvalue) {
            PlayersCountChange?.Invoke(newvalue);
        }

        #region Commands

        [Command(requiresAuthority =false)]
        private void Cmd_PlayerConnected() {
            playerscount++;
            Debug.Log("Registered");
        }

        //[Command(requiresAuthority = false)]
        public void Cmd_PlayerDisConnected() {
            playerscount--;
            Debug.Log("UnRegistered");
            if(playerscount <= 0) {
                ReloadLevel();
            }
        }

        #endregion

        public void ReloadLevel() {
            if(game != null) {
                game.OnServerReloadLevel();
            }
            MultiRoomsNetManager.singleton.UnregisterGameManager(this);
            MultiRoomsNetManager.singleton.ReloadLevel(gameObject.scene);
        }

        #region Start & Stop Callbacks

        /// <summary>
        /// This is invoked for NetworkBehaviour objects when they become active on the server.
        /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
        /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
        /// </summary>
        public override void OnStartServer() {
            Debug.Log("OnStartServer()");

            if (MaxPlayerInGame <= 0) {
                MaxPlayerInGame = LevelConfig.Instance.MaxPlayersInRooms;
            }

            game = new ServerGame();
            game.MaxPlayerCount = MaxPlayerInGame;
            game.CurrentScene = gameObject.scene;
            MultiRoomsNetManager.singleton.RegisterGameManager(this);
            //�������� �������!!
        }

        /// <summary>
        /// Invoked on the server when the object is unspawned
        /// <para>Useful for saving object data in persistent storage</para>
        /// </summary>
        public override void OnStopServer() {
            
        }

        /// <summary>
        /// Called on every NetworkBehaviour when it is activated on a client.
        /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
        /// </summary>
        public override void OnStartClient() {
            Debug.Log("OnStartClient()");
            Instance = this;
            Cmd_PlayerConnected();
        }

        /// <summary>
        /// This is invoked on clients when the server has caused this object to be destroyed.
        /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
        /// </summary>
        public override void OnStopClient() {
            //Cmd_PlayerDisConnected();
            Debug.Log("Player Disconnected!");
        }

        private void OnDestroy() {
            
        }

        /// <summary>
        /// Called when the local player object has been set up.
        /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStartLocalPlayer() {
            Debug.Log("OnStartLocalPlayer()");
        }

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

        #endregion

        #region Testing


        [ContextMenu("Reload this level")]
        public void StartDelayedReloadingLevel() {
            Debug.Log("Start delayed reload level. 10 sec.");
            StartCoroutine(TimerReloadLevel());
        }

        IEnumerator TimerReloadLevel() {
            yield return new WaitForSeconds(10);
            Debug.Log("Call Reload Level!");
            ReloadLevel();

        }

        #endregion
    }
}
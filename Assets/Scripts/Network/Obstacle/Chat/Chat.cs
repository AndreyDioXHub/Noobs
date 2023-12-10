using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using DevionGames.UIWidgets;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class Chat : NetworkBehaviour
{
    // This is only set on client to the name of the local player
    public  static string localPlayerName { get; set; }
    public static uint playerNetID { get; private set; }

    public UnityEvent<string> OnChatMessage;
    public UnityEvent<string, string, uint> OnChatMessageExtend;
    public UnityEvent OnMessageSubmitting;
    public UnityEvent<int> ChatUsersChanged;

    [SyncVar(hook = nameof(PlayersCounthangedHook))]
    int playercount = 0;

    public int CurrentPlayers { get => playercount; }

    public static Chat Instance { get; private set; }

    // Server-only cross-reference of connections to player names
    internal static readonly Dictionary<uint, string> connNames = new Dictionary<uint, string>();

    #region Unity Callbacks

    [SerializeField]
    private TMP_InputField _inputField;

    public string UserName { get => localPlayerName; }

    // NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public static void RegisterLocalPlayer(uint _netID, string _name) {
        playerNetID = _netID;
        localPlayerName = _name;
    }

    private void PlayersCounthangedHook(int old_value, int new_value) {
        if(isClient) {
            ChatUsersChanged?.Invoke(new_value);
        }
    }

    #endregion

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() {
        connNames.Clear();
        ObstacleNetworkManager.singleton.ServerPlayerCountChanged.AddListener(UpdateUsersCount);
    }

    private void UpdateUsersCount(int newCount) {
        playercount = newCount;
    }

    [Command(requiresAuthority = false)]
    void CmdSend(string message, uint sender) {
        if (!connNames.ContainsKey(sender)) { 
            //Reject
            Debug.LogWarning($"ID {sender} not registered. Reject user message");
            return;
        }
        if (!string.IsNullOrWhiteSpace(message))
            RpcReceive(connNames[sender], message.Trim(), sender);
    }

    

    [ClientRpc]
    void RpcReceive(string playerName, string message, uint sender) {
        
        OnChatMessageExtend?.Invoke(playerName, message, sender);
        string prettyMessage = playerName == localPlayerName ?
            $"<color=red>{playerName}:</color> {message}" :
            $"<color=blue>{playerName}:</color> {message}";
        OnChatMessage?.Invoke(prettyMessage);
    }

    public void OnEndEdit(string input) 
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
            SendChatMessage(input);
    }

    public void OnEndEdit() 
    {
        SendChatMessage(_inputField.text);
    }

    // Called by OnEndEdit above and UI element SendButton.OnClick
    public void SendChatMessage(string message) {
        if (!string.IsNullOrWhiteSpace(message)) {
            message = CensoredList.ReplaceText(message.Trim());
            CmdSend(message, playerNetID);
        }
        OnMessageSubmitting?.Invoke();
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
    public override void OnStartClient() {
        //TODO May be clear chat list...
    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() {
        //TODO Register action
    }

    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() {}

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
}

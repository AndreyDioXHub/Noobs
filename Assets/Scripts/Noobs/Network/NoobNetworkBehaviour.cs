using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class NoobNetworkBehaviour : NetworkBehaviour
{
    [SerializeField]
    SkinManager skinManager;

    [SyncVar(hook =nameof(SkinIndexChanged))]
    int skin_index = 0;

    public bool enableOffline = false;

    [SyncVar]
    public string UserName = string.Empty;

    public static Scene ActualScene { get; set; }

    private void SkinIndexChanged(int oldindex, int newindex) {
        //TODO
        if(!isLocalPlayer) {
            skinManager.SetAvatarSkin(newindex);
        }
    }

    #region Unity Callbacks

    /// <summary>
    /// Add your validation code here after the base.OnValidate(); call.
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
    }


    #endregion

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() {
        Debug.Log("<b>Start as SERVER BOT</b>");
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
        Debug.Log(">>OnStartClient called");
        
        PlayerCount.Instance.RegisterPlayer();
        Debug.Log($"{nameof(NoobNetworkBehaviour)}:{nameof(OnStartClient)} - Start Local client");
        GameManager.Instance.ShowConnectedUser();
        if(isLocalPlayer) {
            Debug.Log("<b>Start As local player</b>");
            GetComponent<DistributionHat>().Init(CharType.player);
        } else {
            Debug.Log("<b>Start As avatar</b>");
            GetComponent<DistributionHat>().Init(CharType.avatar);
        }
        //����������� ������� ��� ������ � ���������� �����. �� ������� ��� ���� ����� ������. 
        if (ActualScene.IsValid()) {
            //TODO Load scene
            SceneManager.MoveGameObjectToScene(gameObject, ActualScene);
            
            //gameObject.SetActive(false);
            Debug.Log($"Set player offset: {GameManager.ClientWorldOffset}");
            var pos = gameObject.transform.position + GameManager.ClientWorldOffset;
            pos.y = LevelConfig.Instance.START_PLAYER_YPOS;
            gameObject.transform.position = pos;
            gameObject.SetActive(true);

        }
    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() {
        Debug.Log(">>OnStopClient called");
        if(isLocalPlayer) {
            ActualScene = SceneManager.GetActiveScene();
        }
    }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() {
        Debug.Log(">>OnStartLocalPlayer called");
    }

    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() {
        Debug.Log(">>OnStopLocalPlayer called");
    }

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

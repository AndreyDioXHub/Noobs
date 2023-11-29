using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
using Newtonsoft.Json;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class PlayerNetworkResolver : NetworkBehaviour
{

    [SerializeField]
    private List<GameObject> _males = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _females = new List<GameObject>();

    [SerializeField]
    private List<Renderer> _bodysMale = new List<Renderer>();
    [SerializeField]
    private List<Renderer> _bodysFemale = new List<Renderer>();

    [SerializeField]
    private SkinsInfo _info = new SkinsInfo();

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private RobloxController _robloxController;
    [SerializeField]
    private GroundCheck _groundCheck;
    [SerializeField]
    private AnimatorController _animatorController;

    [SerializeField]
    private GameObject TPSPrefab;
    [SerializeField]
    private GameObject FPSPrefab;
    [SerializeField]
    private Transform _cameraCenterTPS;
    [SerializeField]
    private Transform _cameraCenterFPS;
    [SerializeField]
    private GameObject TPS;
    [SerializeField]
    private GameObject FPS;
    [SerializeField]
    private CameraView _cameraView;
    [SerializeField]
    private PlayerInput _inputs;
    [SerializeField]
    InputActionReference PCLook;
    [SerializeField]
    InputActionReference MobileLook;

    [SerializeField]
    ModelDirection _modelDirection;

    [SerializeField]
    TMP_Text nameField;


    [SyncVar(hook = nameof(OnSkinIndexChanged))]
    int skinIndex = 0;

    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    string username = string.Empty;

    public string UserName { get { return username; } }

    [SyncVar(hook = nameof(OnAxisChanged))]
    Vector3 AxisMove;

    [SyncVar(hook = nameof(OnGroundChanged))]
    bool n_IsGrounded = true;

    [SyncVar(hook = nameof(OnBlendChanged))]
    float n_blend = 0;

    [SyncVar(hook = nameof(OnSkinChanged))]
    string skin_data = string.Empty;
    string User_GUID;


    #region Unity Callbacks

    /// <summary>
    /// Add your validation code here after the base.OnValidate(); call.
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
    }

    // NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.
    void Awake()
    {
        nameField.gameObject.SetActive(false);
    }

    void Start()
    {
        #region Конфигурация под АВАТАРА
        Debug.Log("Конфигурация под АВАТАРА отсюда");
        #endregion

    }

    #endregion

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() {
        Debug.Log("Save user settings by it GUID");
        //TODO Здесь сохраняем данные о пользователе на сервере
    }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() {
        
        if (!isLocalPlayer) {
            //Read player name
            //Enable avatar
            _animatorController.IsLocalPlayer = isLocalPlayer;
            _animatorController.enabled = true;
            nameField.gameObject.SetActive(true);
            nameField.text = username;
        }
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
    public override void OnStartLocalPlayer() 
    {
        #region Здесь прописываем скрипты, которые относятся к локальному игроку
        Debug.Log("Конфигурация под Локального Игрока отсюда");

        if (PlayerPrefs.HasKey(PlayerPrefsConsts.USER_GUID_KEY)) { 
            TryGetSave(PlayerPrefs.GetString(PlayerPrefsConsts.USER_GUID_KEY)); 
        }

        _characterController.enabled = true;
        _robloxController.enabled = true;
        _cameraView.enabled = true;
        _inputs.enabled = true;
        _groundCheck.enabled = true;
        
        _animatorController.IsLocalPlayer = isLocalPlayer;
        _animatorController.MovingStateChange += OnLocalplayerChangeMoveState;
        _animatorController.enabled = true;

        MouseSensitivityManager.Instance.Init(_cameraView);
        TPS = Instantiate(TPSPrefab);
        TPS.GetComponent<CinemachineVirtualCamera>().Follow = _cameraCenterTPS;
        FPS = Instantiate(FPSPrefab);
        FPS.GetComponent<CinemachineVirtualCamera>().Follow = _cameraCenterFPS;
        _cameraView.Init(FPS, TPS);
        InputSchemeSwitcher.Instance.Init(FPS.GetComponent<CinemachineInputProvider>(),
            TPS.GetComponent<CinemachineInputProvider>(),
            _inputs,
            _cameraView,
            PCLook,
            MobileLook);
        InputSchemeSwitcher.Instance.RequestingEnvironmentData();
        CheckPointManager.Instance.Init(transform);
        _robloxController.OnEscDown.AddListener(SettingScreen.Instance.SwitchScreenState);
        _robloxController.OnEscDown.AddListener(ChatTexts.Instance.CloseChat);
        _robloxController.OnEnterDown.AddListener(ChatTexts.Instance.OpenChat);
        GetUserSkin();
        Chat.localPlayerName = username;

        #endregion
    }

    

    [Command]
    private void GetUserSkin() {
        skinIndex = PlayerPrefs.GetInt(PlayerPrefsConsts.USER_SKIN_KEY, 0);
        username = PlayerPrefs.GetString(PlayerPrefsConsts.USER_NAME_KEY, GetDefaultName());

        string infoJSON = PlayerPrefs.GetString("user_skin", "");
        _info = JsonConvert.DeserializeObject<SkinsInfo>(infoJSON);

        skin_data = infoJSON;

        EquipSex(_info.eqyuipedSex);
    }

    public void EquipSex(int index)
    {
        _info.eqyuipedSex = index;

        foreach (var male in _males)
        {
            male.SetActive(index == 0);
        }

        foreach (var female in _females)
        {
            female.SetActive(index == 1);
        }
    }


    private void OnSkinChanged(string olddata, string newdata) {
        if(!olddata.Equals(newdata) && ! isLocalPlayer) {
            //Apply to AVATAR
            //TODO
            _info = JsonConvert.DeserializeObject<SkinsInfo>(newdata);
            EquipSex(_info.eqyuipedSex);
        }
    }

    private void OnSkinIndexChanged(int oldindex, int newindex) {
        Debug.Log($"Set user skin {newindex}");
        //TODO Set user skin

    }
    private void OnPlayerNameChanged(string oldname, string newname) {
        Debug.Log($"Set user name {newname}");
        //TODO Set user name
        if (!isLocalPlayer) nameField.text = newname;
        Chat.localPlayerName = newname;
    }

    public void OnMove(InputAction.CallbackContext context) {
        Vector2 val = context.ReadValue<Vector2>();
        AxisMove = new Vector3(val.x, 0, val.y);
    }
    
    private void OnAxisChanged(Vector3 oldval, Vector3 newval) {
        if(!isLocalPlayer) {
            _modelDirection.OnAvatarMove(newval);
            _animatorController.AxisMove = newval;
        }
    }

    private void OnGroundChanged(bool oldval, bool newval) {
        if(!isLocalPlayer) _animatorController.IsGrounded = newval;
    }

    private void OnBlendChanged(float oldval, float newval) {
        if (!isLocalPlayer) _animatorController.Blend = newval;
    }

    private void OnLocalplayerChangeMoveState(bool state) {
        n_IsGrounded = state;
    }


    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() {
        Debug.Log("Try save data about player");
        //TODO Здесь сохраняем пользовательские данные локально, если необходимо.
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

    /// <summary>
    /// Загрузка информации об игроке.
    /// </summary>
    /// <param name="guid"></param>
    [Command]
    private void TryGetSave(string guid) {
        User_GUID = guid;
        if(PlayerPrefs.HasKey(guid)) {
            //TODO Apply to user
            string data = PlayerPrefs.GetString(guid, "");
            TRPC_ApplySavesFromServer(data);
        }
    }

    [TargetRpc]
    private void TRPC_ApplySavesFromServer(string data) {
        Debug.Log("Apply data from server");
    }

    private string GetDefaultName() {
        return "DefaultName";
    }
}

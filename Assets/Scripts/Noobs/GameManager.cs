using cyraxchel.network.rooms;
using cyraxchel.network.server;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    public static Vector3 ClientWorldOffset { get; set; } = Vector3.zero;

    public Vector3 GlobalOffset { get => netOffset; set { netOffset = value; } }

    [SyncVar(hook = nameof(UpdateNetOffset))]
    Vector3 netOffset = Vector3.zero;

    public static GameManager Instance { get;
        set; }    //Используется только в клиентах!

    public bool IsWin { get; private set; } = false;
    public bool IsLose;

    [SerializeField]
    private GameObject CanvasPrefab;

    [SerializeField]
    private WinScreen _winscreen;
    [SerializeField]
    ConnectionScreen _connectionscreen;
    [SerializeField]
    private float _time = 2;
    [SerializeField]
    private float _timeCur = 0;
    [SerializeField]
    private GameObject _leaveText;
    [SerializeField]
    PlayerCount _playerCount;
    [SyncVar(hook =nameof(UpdatePlayersCount))]
    int _readyPlayers = 0;

    public Transform _levelTransform;
    [SerializeField]
    GameObject _startPlatform;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!isClientOnly) {
            return;     // Отрабатывать только на клиентах!
        }
        //TODO Переделать счет игроков на локальный элемент
        if(PlayerCount.Instance.Count == 1)
        {
            //IsWin = true;
            _winscreen?.ShowWinScreen();
        }
        if(IsLose) {
            _winscreen?.ShowLoseScreen();
        }

        if (IsWin || IsLose)
        {
            _timeCur += Time.deltaTime;

            if(_timeCur > _time)
            {
                _leaveText.SetActive(true);

                if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    public void UpdateSceneOffset() {
        Debug.Log($"{nameof(UpdateSceneOffset)}> Scene offset updated to {GlobalOffset}");
        if(isServer) {
            _levelTransform.position = GlobalOffset;
        }
        if(isClientOnly) {
            _levelTransform.position = ClientWorldOffset;
        }
    }

    private void UpdateNetOffset(Vector3 oldvalue, Vector3 newvalue) {
        GlobalOffset = newvalue;
        ClientWorldOffset = newvalue;
        UpdateSceneOffset();
    }

    public override void OnStartClient() {
        Debug.Log($"{nameof(GameManager)}:{nameof(OnStartClient)} - Start Local client");
        base.OnStartClient();
        AddUIElements();
        if(MultiRoomsGameManager.Instance != null ) {
            MultiRoomsGameManager.Instance.StartLocalGame += StartGame;
            MultiRoomsGameManager.Instance.EndLocalGame  += StopGame;
        }
    }

    private void AddUIElements() {
        var canvaGO = Instantiate(CanvasPrefab, null, false);
        //TODO?
    }

    public override void OnStartServer() {
        base.OnStartServer();
        
        Instance = null;

    }


    internal void OnGameStatusChanged(ServerGame game, ServerGame.Status status) {
        switch(status) {
            case ServerGame.Status.Action:
               // _startPlatform.SetActive(false);
                
                //Start Game
                //RPC_StartGame();
                //ServerBotBalancer.Instance.Unpause(game);
                break;
            case ServerGame.Status.Finish: 
                //Complete game
                //RPC_StopGame();
                break;
        }
    }


    private void StartGame() {
        _startPlatform.SetActive(false);
        //TODO
        _connectionscreen.gameObject.SetActive(false);
    }

    private void StopGame() {
        if(NetworkClient.isConnected) {
            MultiRoomsNetManager.singleton.StopClient();
        }
    }

    internal void OnPlayerJoin(ServerGame game, ServerGame.Player player) {
        //_readyPlayers++;
        //Debug.Log($"Add player {player.name}. Index: {_readyPlayers}");
    }

    private void UpdatePlayersCount(int oldvalue, int newvalue) {
        Debug.Log($"Ready Players {newvalue}");
//        _connectionscreen.AddNextPlayer(newvalue); 
    }

    public void ShowConnectedUser() {
        //TODO
        //_connectionscreen.Show(1);
    }

    public void RegisterWinScreen(WinScreen item) {
        _winscreen = item;
    }
}

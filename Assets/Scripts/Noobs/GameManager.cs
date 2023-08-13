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

    public Vector3 GlobalOffset { get; set; } = Vector3.zero;

    public static GameManager Instance { get; set; }    //Используется только в клиентах!

    public bool IsWin { get; private set; } = false;
    public bool IsLose;

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

    private void Awake() {
        
        if(isClientOnly) {
            Instance = this;
        }
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
        if(_playerCount.Count == 1)
        {
            //IsWin = true;
            _winscreen.ShowWinScreen();
        }
        if(IsLose) {
            _winscreen.ShowLoseScreen();
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

    public override void OnStartClient() {
        base.OnStartClient();
        CmdRegisterGame();
    }

    public override void OnStartServer() {
        base.OnStartServer();
        StartCoroutine(CmdRegisterGame());
    }

    //[Command]
    private IEnumerator CmdRegisterGame() {
        yield return new WaitForSeconds(0.1f);
        Debug.Log($"Try register {nameof(CmdRegisterGame)}");
        ServerNetworkBehaviour.Instance.RegisterGameManager(gameObject.scene, this);
        UpdateSceneOffset();

    }

    internal void OnGameStatusChanged(ServerGame game, ServerGame.Status status) {
        switch(status) {
            case ServerGame.Status.Action:
                //Start Game
                RPC_StartGame();
                break;
            case ServerGame.Status.Finish: 
                //Complete game
                RPC_StopGame();
                break;
        }
    }

    [ClientRpc]
    private void RPC_StartGame() {

    }
    [ClientRpc]
    private void RPC_StopGame() {

    }

    internal void OnPlayerJoin(ServerGame game, ServerGame.Player player) {
        _readyPlayers++;
    }

    private void UpdatePlayersCount(int oldvalue, int newvalue) {
        _connectionscreen.AddNextPlayer(newvalue); 
    }
}

using cyraxchel.network.server;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static Vector3 ClientWorldOffset { get; set; } = Vector3.zero;

    public Vector3 GlobalOffset { get; set; }

    

    public static GameManager Instance { get;
        set; }    //Используется только в клиентах!

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
    

    public Transform _levelTransform;
    [SerializeField]
    GameObject _startPlatform;

    public GMNetwork gm_network;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        if(ServerNetworkBehaviour.Instance != null) {
            ServerNetworkBehaviour.Instance.RegisterGameManager(gameObject.scene, this);
        }
    }

    void Update()
    {
        if (gm_network == null) return;     // Не выполнять, пока не инициализирована сеть

        if(!gm_network.isClientOnly) {
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
        if(gm_network.isServer) {
            _levelTransform.position = GlobalOffset;
        }
        if(gm_network.isClientOnly) {
            _levelTransform.position = ClientWorldOffset;
        }
    }

    

    public void OnStartClient() {
        Debug.Log($"{nameof(GameManager)}:{nameof(OnStartClient)} - Start Local client");
        CmdRegisterGame();
    }

    public void OnStartServer() {
        StartCoroutine(CmdRegisterGame());
        Instance = null;
    }

    private IEnumerator CmdRegisterGame() {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log($"Try register {nameof(CmdRegisterGame)}");
        //ServerNetworkBehaviour.Instance.RegisterGameManager(gameObject.scene, this);
        UpdateSceneOffset();

    }

    internal void OnGameStatusChanged(ServerGame game, ServerGame.Status status) {
        switch(status) {
            case ServerGame.Status.Action:
               // _startPlatform.SetActive(false);
                
                //Start Game
                //RPC_StartGame();
                ServerBotBalancer.Instance.Unpause(game);
                break;
            case ServerGame.Status.Finish: 
                //Complete game
                //gm_network.RPC_StopGame();
                break;
        }
    }


    internal void OnPlayerJoin(ServerGame game, ServerGame.Player player) {
        //_readyPlayers++;
        //Debug.Log($"Add player {player.name}. Index: {_readyPlayers}");
    }

    

    public void ShowConnectedUser() {
        _connectionscreen.Show(1);
    }
}

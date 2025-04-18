using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

namespace cyraxchel.network.server {

    //������������ ������� ����.
    [Serializable]
    public class ServerGame {

        public event Action<ServerGame, Status> GameStatusChanged = delegate { };
        public event Action<ServerGame, Player> JoinPlayer = delegate { };
        public event Action<ServerGame, Player> LeavePlayer = delegate { };

        List<Player> m_players = new List<Player>();
        public Vector3 WorldOffset { get; internal set; } = Vector3.zero;       //������ �������� ����

        [SerializeField]
        Status m_Status = Status.None;
        public Status GameStatus { 
            get { return m_Status; } 
            private set { m_Status = value; GameStatusChanged?.Invoke(this, value); } }

        /// <summary>
        /// ����� �������� �������, ���
        /// </summary>
        public float AwaitingTime = 10;
        /// <summary>
        /// ������������ ���������� ������� � ����
        /// </summary>
        public int MaxPlayerCount = 15;

        Coroutine countdownTimer;

        /// <summary>
        /// �������� �� �������� ������ � ����
        /// </summary>
        public bool CanAcceptPlayer { get {
                if(GameStatus == Status.Preparation) {
                    return PlayersCount < MaxPlayerCount;
                } else {
                    return false;
                }
            } }

        /// <summary>
        /// Get current players count
        /// </summary>
        public int PlayersCount { get => m_players.Count; }

        [SerializeField]
        Scene _gamescene;
        public Scene CurrentScene { get => _gamescene; internal set {
                _gamescene = value;
                //��������������� ���� 
                GMNetwork gMNetwork = MultisceneNoobNetworkManager.singleton.InstantiateGMNetwork(this);
                GameStatus = Status.None;
            } }

        GameManager _gamemanager;
        public GameManager CurrentGameManager { get=> _gamemanager; internal set { 
                _gamemanager= value;
                if(value != null) {
                    Debug.Log($"Set new GameManager. Offset is {WorldOffset}");
                    _gamemanager.GlobalOffset = WorldOffset;
                    GameStatusChanged += _gamemanager.OnGameStatusChanged;
                    JoinPlayer += _gamemanager.OnPlayerJoin;
                }
            } }

        public void Init() {
            Debug.Log($"Init without WorldOffset");
            awaitingPlayers = new Dictionary<int, NetworkConnectionToClient>();
            GameStatus = Status.Preparation;

            Debug.Log($"Current scene is {CurrentScene}");
            //��������� ������
            countdownTimer = ServerNetworkBehaviour.Instance.StartCoroutine(AwaitPlayers());
            
        }

        IEnumerator AwaitPlayers() {
            yield return new WaitForSeconds(AwaitingTime);

            if(PlayersCount > 0 ) {
                if(PlayersCount < MaxPlayerCount) {
                    //��������� ����� ��� ����
                    ServerBotBalancer.Instance.GetBotForGame(this, MaxPlayerCount - PlayersCount);
                } else {
                    //������ ����
                    GameStatus = Status.Action;
                }
            } else {
                GameStatus = Status.None;
            }
        }

        public void AddPlayer(Player player) {
            if (PlayersCount < MaxPlayerCount && GameStatus == Status.Preparation) {
                m_players.Add(player);
                JoinPlayer?.Invoke(this, player);
                if(m_players.Count == MaxPlayerCount) {
                    if(countdownTimer != null) ServerNetworkBehaviour.Instance.StopCoroutine(countdownTimer);
                }
            }
        }

        public void AddPlayer(string name, int id) {
            Player player = new Player(name, id);
            AddPlayer(player);
        }

        public void RemovePlayer(Player player) {
            //TODO
        }

        public void GameComplete() {
            GameStatus = Status.Finish;
            GameStatusChanged -= _gamemanager.OnGameStatusChanged;
            JoinPlayer -= _gamemanager.OnPlayerJoin;
        }

        Dictionary<int, NetworkConnectionToClient> awaitingPlayers = new Dictionary<int, NetworkConnectionToClient>();

        internal void ReservePlayerSlot(NetworkConnectionToClient conn) {
            if(awaitingPlayers.ContainsKey(conn.connectionId)) {
                //Update
                awaitingPlayers[conn.connectionId] = conn;
            } else {
                //Create new
                awaitingPlayers.Add(conn.connectionId, conn);
            }
        }

        internal bool AddReservedPlayer(string name, int netId) {
            if(awaitingPlayers.ContainsKey(netId)) {
                AddPlayer(name, netId);
                return true;
            } else {
                return false;
            }
        }

        internal void FreeSlot(NetworkConnectionToClient conn) {
            if (awaitingPlayers.ContainsKey(conn.connectionId)) {
                awaitingPlayers.Remove(conn.connectionId);
            }
        }

        
        /// <summary>
        /// ���������� � ���������� ���������� �����
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void BotComplete() {
            Debug.LogWarning("TODO: ������������ ������ ����");
            //GameStatus = Status.Action; //������ ����
        }

        public class Player {
            public string name { get; set; }

            int id { get; set; }

            public Player(string name, int id) {
                this.name = name;
                this.id = id;
            }
        }

        public enum Status {
            None,
            Preparation,
            Action,
            Finish
        }
    }

    
}
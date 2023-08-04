using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

namespace cyraxchel.network.server {

    //Обслуживание текущей игры.
    public class ServerGame {

        public event Action<ServerGame, Status> GameStatusChanged = delegate { };
        public event Action<ServerGame, Player> JoinPlayer = delegate { };
        public event Action<ServerGame, Player> LeavePlayer = delegate { };

        List<Player> m_players = new List<Player>();
        public Vector3 WorldOffset { get; private set; } = Vector3.zero;       //Храним смещение мира

        Status m_Status = Status.None;
        public Status GameStatus { 
            get { return m_Status; } 
            private set { m_Status = value; GameStatusChanged?.Invoke(this, value); } }

        /// <summary>
        /// Время ожидания игроков, сек
        /// </summary>
        public float AwaitingTime = 10;
        /// <summary>
        /// Максимальное количество игроков в игре
        /// </summary>
        public int MaxPlayerCount = 15;

        /// <summary>
        /// Возможно ли добавить игрока в игру
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
        public Scene CurrenScene { get; internal set; }

        public void Init(Vector3 worldOffset) {
            GameStatus = Status.Preparation;
            //Запустить таймер
            ServerNetworkBehaviour.Instance.StartCoroutine(AwaitPlayers());
            WorldOffset = worldOffset;
        }

        IEnumerator AwaitPlayers() {
            yield return new WaitForSeconds(AwaitingTime);

            if(PlayersCount > 0 ) {
                if(PlayersCount < MaxPlayerCount) {
                    //Запросить ботов для игры
                    ServerBotBalancer.Instance.GetBotForGame(this, MaxPlayerCount - PlayersCount);
                } else {
                    //Начать игру
                    GameStatus = Status.Action;
                }
            } else {
                GameStatus = Status.None;
            }
            

        }

        public void AddPlayer(Player player) {
            if (PlayersCount < MaxPlayerCount) {
                m_players.Add(player);
                JoinPlayer?.Invoke(this, player);
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyraxchel.network.server 
{
    public class ServerBotBalancer : MonoBehaviour 
    {
        public static ServerBotBalancer Instance { get; private set; }
        [SerializeField]
        private MultisceneNoobNetworkManager _multiscene;
        [SerializeField]
        private List<GameObject> _bots = new List<GameObject>();
        [SerializeField]
        private GameObject _playerPrefab;

        internal void GetBotForGame(ServerGame serverGame, int requiredNumberBots) 
        {
            //TODO Assign bots for this game
        }

        internal void ReleaseBotsFromGame(ServerGame serverGame) 
        {
            //TODO Remove bots from this game
        }

        void Awake() 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void Start() 
        {
            //TODO Add bots

        }

        [ContextMenu("PrepareBots")]
        public void PrepareBots()
        {
            foreach(var bot in _bots)
            {
                Destroy(bot);
            }

            int botsCount = 15 * _multiscene.instances;

            for(int i=0; i< botsCount; i++)
            {
                var goBot = Instantiate(_playerPrefab);

                //int indexBot = Random.Range(0, _freePositions.Count - 1);
                Vector3 botPosition = new Vector3(5000,0,5000);// _freePositions[indexBot];

                //_freePositions.RemoveAt(indexBot);

                var hat = goBot.GetComponent<DistributionHat>();
                hat.Init(CharType.bot);
                hat.Pause();

                goBot.SetActive(false);
                goBot.transform.position = botPosition;
                //goBot.SetActive(true);


                //PlayerCount.Instance.RegisterPlayer();
                _bots.Add(goBot);
            }

        }

        // Update is called once per frame
        void Update() 
        {

        }
    }
}
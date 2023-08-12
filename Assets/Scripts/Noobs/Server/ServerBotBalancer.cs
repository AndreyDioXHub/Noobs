using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cyraxchel.network.server 
{
    public class ServerBotBalancer : MonoBehaviour 
    {
        public static ServerBotBalancer Instance { get; private set; }

        [Scene]
        public string _mainSceneString;

        [SerializeField]
        private MultisceneNoobNetworkManager _multiscene;
        [SerializeField]
        private List<GameObject> _bots = new List<GameObject>();
        [SerializeField]
        private GameObject _playerPrefab;

        private Dictionary<ServerGame, List<GameObject>> _reservedBots = new Dictionary<ServerGame, List<GameObject>>();

        private Queue<BotForGameParameters> _forGameParameters = new Queue<BotForGameParameters>();
        private bool _botgamecouroutineProcecced = false;

        private Scene _mainScene;

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

            _mainScene = SceneManager.GetSceneByName(_mainSceneString);
        }

        internal void GetBotForGame(ServerGame serverGame, int requiredNumberBots) 
        {
            _forGameParameters.Enqueue(new BotForGameParameters(serverGame, requiredNumberBots));

            if (_botgamecouroutineProcecced)
            {

            }
            else
            {
                StartCoroutine(GetBotForGameCoroutine());
            }

            //Debug.Log($"GetBotForGame {_forGameParameters.Count}");
            //TODO Assign bots for this game
            //SceneManager.MoveGameObjectToScene(_bots[0], serverGame.CurrenScene);

        }

        IEnumerator GetBotForGameCoroutine()
        {
            _botgamecouroutineProcecced = true;

            while (_forGameParameters.Count > 0)
            {
                BotForGameParameters parameters = _forGameParameters.Dequeue();

                List<GameObject> bots = new List<GameObject>();

                //Генерирую с неучтенными ботами (для тестов)
                for(int i = 0; i < parameters.requiredNumberBots+5; i++)
                {
                    if (_bots.Count == 0)
                    {
                        var goBot = Instantiate(_playerPrefab);
                        Vector3 botPosition = new Vector3(5000, 0, 5000);
                        var hat = goBot.GetComponent<DistributionHat>();
                        hat.Init(CharType.bot);
                        hat.Pause();

                        goBot.SetActive(false);
                        goBot.transform.position = botPosition;
                        _bots.Add(goBot);
                        NetworkServer.Spawn(goBot);
                    }
                    if(i < parameters.requiredNumberBots) {
                        bots.Add(_bots[0]);
                        _bots.RemoveAt(0);
                    }

                    yield return new WaitForEndOfFrame();
                }

                foreach(var bot in bots)
                {
                    SceneManager.MoveGameObjectToScene(bot, parameters.serverGame.CurrenScene);
                    //parameters.serverGame.AddPlayer
                }

                _reservedBots.Add(parameters.serverGame, bots);
            }

            _botgamecouroutineProcecced = false;
        }

        internal void ReleaseBotsFromGame(ServerGame serverGame) 
        {
            //TODO Remove bots from this game
            StartCoroutine(ReleaseBotsFromGameCoroutine(serverGame));
        }

        IEnumerator ReleaseBotsFromGameCoroutine(ServerGame serverGame)
        {
            while (_botgamecouroutineProcecced)
            {
                yield return new WaitForEndOfFrame();
            }

            if (_reservedBots.TryGetValue(serverGame, out List<GameObject> bots))
            {
                foreach(var bot in bots)
                {
                    bot.SetActive(false);
                    bot.GetComponent<DistributionHat>().Pause();
                    SceneManager.MoveGameObjectToScene(bot, _mainScene);
                    bot.transform.position = new Vector3(5000, 0, 5000);
                    _bots.Add(bot);
                }

                _reservedBots.Remove(serverGame);
            }
            else
            {
                Debug.Log($"{serverGame.CurrenScene.name}: key empty");
            }
        }


        [ContextMenu("PrepareBots")]
        public void PrepareBots()
        {
            foreach(var bot in _bots)
            {
                Destroy(bot);
            }

            int botsCount = 1 * _multiscene.instances;

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
                NetworkServer.Spawn(goBot);
            }

        }

        [ContextMenu("Resume bots")]
        public void Unpause() {
            foreach (var bot in _bots) {
                bot.GetComponent<DistributionHat>().Play();
                Vector3 pos = UnityEngine.Random.insideUnitSphere * 10;
                pos.y = 79;
                bot.transform.position = pos;
                bot.SetActive(true);
            }
        }

        public void Unpause(ServerGame serverGame)
        {
            if (_reservedBots.TryGetValue(serverGame, out List<GameObject> bots))
            {
                foreach (var bot in bots)
                {
                    bot.GetComponent<DistributionHat>().Play();
                    Vector3 pos = UnityEngine.Random.insideUnitSphere * 10;
                    pos.y = 79;
                    bot.transform.position = pos;
                    bot.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"{serverGame.CurrenScene.name}: key empty");
            }
            
        }

        // Update is called once per frame
        void Update() 
        {

        }
    }

    [Serializable]
    public class BotForGameParameters
    {
        public ServerGame serverGame;
        public int requiredNumberBots;

        public BotForGameParameters(ServerGame serverGame, int requiredNumberBots)
        {
            this.serverGame = serverGame;
            this.requiredNumberBots = requiredNumberBots;
        }
    }
}


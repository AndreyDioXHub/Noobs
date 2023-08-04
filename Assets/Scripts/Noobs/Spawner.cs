using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _noobCharacter;
    [SerializeField]
    private int _botsCount = 15;
    [SerializeField]
    private List<Vector3> _freePositions = new List<Vector3>();

    public static Spawner Instance;

    void Awake() {
        Instance = this;
        //Init();
    }

    //[ContextMenu("Add bots")]
    public List<GameObject> Init() {
        return null;

        List<GameObject> bots = new List<GameObject>();

        for(int i =-3; i < 4; i++)
        {
            for (int j = -3; j < 4; j++)
            {
                _freePositions.Add(new Vector3(i, 77, j));
            }
        }
        /*
        var goPlayer = Instantiate(_noobCharacter);

        int index = Random.Range(0, _freePositions.Count - 1);
        Vector3 playerPosition = _freePositions[index];

        _freePositions.RemoveAt(index);

        goPlayer.GetComponent<DistributionHat>().Init(CharType.player);

        goPlayer.SetActive(false);
        goPlayer.transform.position = playerPosition;
        goPlayer.SetActive(true);

        PlayerCount.Instance.RegisterPlayer();/**/

        for (int i=0; i < _botsCount; i++)
        {

            
            var goBot = Instantiate(_noobCharacter);

            int indexBot = Random.Range(0, _freePositions.Count - 1);
            Vector3 botPosition = _freePositions[indexBot];

            _freePositions.RemoveAt(indexBot);

            goBot.GetComponent<DistributionHat>().Init(CharType.bot);

            goBot.SetActive(false);
            goBot.transform.position = botPosition;
            goBot.SetActive(true);

            PlayerCount.Instance.RegisterPlayer();
            bots.Add(goBot);
        }
        return bots;
    }

    

    void Update()
    {
        
    }

    internal void ReleaseBots(int gameIndex) {
        throw new System.NotImplementedException();
    }
}

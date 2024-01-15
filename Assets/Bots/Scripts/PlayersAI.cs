using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersAI : MonoBehaviour
{

    public static List<GameObject> Players;

    [SerializeField]
    List<GameObject> playes;
    // Start is called before the first frame update
    void Awake()
    {
        Players = playes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

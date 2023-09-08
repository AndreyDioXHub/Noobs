using cyraxchel.network.rooms;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPlayerCount : MonoBehaviour
{
    [SerializeField]
    TMP_Text ShowText;
    string prefix = "Player count = ";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MultiRoomsGameManager.Instance != null) {
            ShowText.text = prefix + MultiRoomsGameManager.Instance.playerscount.ToString();
        }
    }
}

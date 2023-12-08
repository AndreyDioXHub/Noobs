using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnlinePlayersView : MonoBehaviour
{
    [SerializeField]
    TMP_Text count_text;

    // Start is called before the first frame update
    void Start()
    {
        Chat.Instance.ChatUsersChanged.AddListener(UpdatePlayersView);
        UpdatePlayersView(Chat.Instance.CurrentPlayers);
    }

    private void UpdatePlayersView(int players) {
        count_text.text = players.ToString();
    }

    private void OnDestroy() {
        Chat.Instance.ChatUsersChanged.RemoveListener(UpdatePlayersView);

    }
}

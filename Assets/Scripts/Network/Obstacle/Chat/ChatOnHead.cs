using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ChatOnHead : MonoBehaviour
{
    [SerializeField]
    TMP_Text nameField;

    [SerializeField]
    float showTime = 3;
    [SerializeField]
    float fadeTime = 1;

    string userName;// = string.Empty;
    bool showName = false;

    // Start is called before the first frame update
    void Start()
    {
        nameField.text = showName ? userName : "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string playername, bool isLocalPlayer, bool requireRegisterListener = true) {
        userName = playername;
        showName = !isLocalPlayer;
        nameField.text = userName;
        Debug.Log(Chat.Instance);
        if(requireRegisterListener) Chat.Instance.OnChatMessageExtend.AddListener(OnChatMessage);
    }

    public void OnChatMessage(string sendername, string message) {
        Debug.Log($"{sendername}: {message}, username is<b>{userName}</b>, local player name is {Chat.localPlayerName}");
        if(userName.Equals(sendername)) {
            Debug.Log($"{sendername}: is Equals");
            StopAllCoroutines();
            nameField.CrossFadeAlpha(1, 0, true);
            message = SetFormatting(message);
            StartCoroutine(NewChatMessage(message));
            //TODO Send to network

        }
    }

    private string SetFormatting(string message) {
        return $"<size=8>{message}</size>";

    }

    private IEnumerator NewChatMessage(string message) {
        nameField.text = message;
        yield return new WaitForSeconds(showTime);
        nameField.CrossFadeAlpha(0, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);
        nameField.text = showName ? userName: "";
        nameField.CrossFadeAlpha(1, 0, true);
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
}

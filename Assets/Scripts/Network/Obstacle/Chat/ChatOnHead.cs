using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatOnHead : MonoBehaviour
{
    [SerializeField]
    TMP_Text nameField;
    [SerializeField]
    private RectTransform _root;
    [SerializeField]
    private Image _messageIMG;

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
        _messageIMG.CrossFadeAlpha(0, 0, true);
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
        Debug.Log($"<color=red>[ChatOnHead]</color> {sendername}: {message}, username is<b>{userName}</b>, local player name is {Chat.localPlayerName}");
        if (string.IsNullOrEmpty(sendername)) return;   //Dont show if user name null
        if(userName.Equals(sendername)) {
            Debug.Log($"{sendername}: is Equals");
            StopAllCoroutines();
            nameField.CrossFadeAlpha(1, 0, true);
            message = SetFormatting(message);
            StartCoroutine(NewChatMessage(message));
            //TODO Send to network

        }
    }

    private string SetFormatting(string message)
    {
        return $"<size=150>{message}</size>";
    }

    private IEnumerator NewChatMessage(string message) 
    {
        _messageIMG.CrossFadeAlpha(1, 0, true);
        nameField.text = message;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_messageIMG.rectTransform);

         yield return new WaitForSeconds(showTime);
        nameField.CrossFadeAlpha(0, fadeTime, true);
        _messageIMG.CrossFadeAlpha(0, fadeTime, true);

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

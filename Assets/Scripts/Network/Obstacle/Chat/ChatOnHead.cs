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
    uint playerid;
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

    public void Init(string playername,uint _pid, bool isLocalPlayer, bool requireRegisterListener = true) {
        userName = playername;
        playerid = _pid;
        showName = !isLocalPlayer;
        nameField.text = userName;
        Debug.Log(Chat.Instance);
        if(requireRegisterListener) Chat.Instance.OnChatMessageExtend.AddListener(OnChatMessage);
    }

    public void OnChatMessage(string sendername, string message, uint sender) {
        Debug.Log($"<color=red>[ChatOnHead]</color> {sendername}: {message}, username is <b>{userName}</b>, local player name is {Chat.localPlayerName}");
        if (string.IsNullOrEmpty(sendername)) return;   //Dont show if user name null
        if(sender ==playerid) {
            Debug.Log($"{sendername}: is Local");
            StopAllCoroutines();
            nameField.CrossFadeAlpha(1, 0, true);
            message = SetFormatting(message);
            StartCoroutine(NewChatMessage(message));
            
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

        showTime = (float)(message.Length) * 60/ 800;

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

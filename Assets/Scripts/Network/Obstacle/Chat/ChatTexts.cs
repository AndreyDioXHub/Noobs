using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTexts : MonoBehaviour
{
    public static bool IsActive
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            else
            {
                return Instance.ChatView.activeSelf;
            }
        }
    }

    public static ChatTexts Instance;

    [SerializeField]
    GameObject ChatView;

    [SerializeField]
    private Notification notificationView;

    [Header("Colors")]
    [SerializeField] Color localUserName = Color.black;
    [SerializeField] Color localUserText = Color.black;
    [SerializeField] Color remoteUserName = Color.black;
    [SerializeField] Color remoteUserText = Color.black;



    [SerializeField]
    NotificationOptions defaulOption;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        CloseChat();
    }
    public void OpenChat()
    {
        ChatView.SetActive(true);
    }
    public void CloseChat()
    {
        ChatView.SetActive(false);
    }

    public void ShowChatText(string user, string message) {
        //notificationView.AddItem();
        NotificationOptions opt = new NotificationOptions(defaulOption);
        opt.text = StylizeText(user, message);
        //opt.title = user;
        notificationView.AddItem(opt);
    }

    private string StylizeText(string username, string message) {
        bool islocal = username == Chat.localPlayerName;
        string ucolor = ColorUtility.ToHtmlStringRGB(islocal? localUserName : remoteUserName);
        string mcolor = ColorUtility.ToHtmlStringRGB(islocal? localUserText : remoteUserText);

        string prettyString = $"<color=#{ucolor}>[{username}]:</color> <color=#{mcolor}>{message}</color>";

        return prettyString;
    }
}

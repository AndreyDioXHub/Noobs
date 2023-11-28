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
        opt.text = message;
        opt.title = user;
        notificationView.AddItem(opt);
    }
}

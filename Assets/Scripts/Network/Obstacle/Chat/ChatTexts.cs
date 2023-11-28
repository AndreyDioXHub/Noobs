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
                return Instance.gameObject.activeSelf;
            }
        }
    }

    public static ChatTexts Instance;

    [SerializeField]
    private Notification notificationView;


    [SerializeField]
    NotificationOptions defaulOption;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }
    public void OpenChat()
    {
        gameObject.SetActive(true);
    }
    public void CloseChat()
    {
        gameObject.SetActive(false);
    }

    public void ShowChatText(string user, string message) {
        //notificationView.AddItem();
        NotificationOptions opt = new NotificationOptions(defaulOption);
        opt.text = message;
        opt.title = user;
        notificationView.AddItem(opt);
    }
}

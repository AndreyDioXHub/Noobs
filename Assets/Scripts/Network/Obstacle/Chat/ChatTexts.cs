using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTexts : MonoBehaviour
{

    [SerializeField]
    private Notification notificationView;


    [SerializeField]
    NotificationOptions defaulOption;

    // Start is called before the first frame update

    public void ShowChatText(string user, string message) {
        //notificationView.AddItem();
        NotificationOptions opt = new NotificationOptions(defaulOption);
        opt.text = message;
        opt.title = user;
        notificationView.AddItem(opt);
    }
}

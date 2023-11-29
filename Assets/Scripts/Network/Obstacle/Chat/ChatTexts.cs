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
                return Instance._chatIsOpen;// ChatView.activeSelf;
            }
        }
    }

    public static ChatTexts Instance;

    [SerializeField]
    private List<GameObject> _switchedes = new List<GameObject>();

    [SerializeField]
    private Notification notificationView;

    [Header("Colors")]
    [SerializeField] Color localUserName = Color.black;
    [SerializeField] Color localUserText = Color.black;
    [SerializeField] Color remoteUserName = Color.black;
    [SerializeField] Color remoteUserText = Color.black;

    [SerializeField]
    private bool _chatIsOpen;

    [SerializeField]
    private RectTransform _chatView;
    [SerializeField]
    private GameObject _openNotification;
    [SerializeField]
    private GameObject _closeNotification;
    [SerializeField]
    private GameObject _inputField;


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
        if (SettingScreen.IsActive)
        {
            foreach(var sw in _switchedes)
            {
                sw.SetActive(false);
            }
        }
        else
        {
            foreach (var sw in _switchedes)
            {
                sw.SetActive(true);
            }
            _chatIsOpen = true;
            _chatView.sizeDelta = new Vector2(660, 400);
            _openNotification.SetActive(false);
            _closeNotification.SetActive(true);
            _inputField.SetActive(true);
        }

        //ChatView.SetActive(true);
    }

    public void CloseChat()
    {
        if (SettingScreen.IsActive)
        {
            foreach (var sw in _switchedes)
            {
                sw.SetActive(false);
            }
        }
        else
        {
            foreach (var sw in _switchedes)
            {
                sw.SetActive(true);
            }
            _chatIsOpen = false;
            _chatView.sizeDelta = new Vector2(660, 60);
            _openNotification.SetActive(true);
            _closeNotification.SetActive(false);
            _inputField.SetActive(false);
        }
        //ChatView.SetActive(false);
    }

    public void ToggleChat() 
    {
        _chatIsOpen = !_chatIsOpen;

        if (_chatIsOpen)
        {
            OpenChat();
        }
        else
        {
            CloseChat();
        }

       // ChatView.SetActive(!ChatView.activeSelf);
    }

    public void ShowChatText(string user, string message) 
    {
        //notificationView.AddItem();
        NotificationOptions opt = new NotificationOptions(defaulOption);
        opt.text = StylizeText(user, message);
        //opt.title = user;
        notificationView.AddItem(opt);
    }

    private string StylizeText(string username, string message) 
    {
        bool islocal = username == Chat.localPlayerName;
        string ucolor = ColorUtility.ToHtmlStringRGB(islocal? localUserName : remoteUserName);
        string mcolor = ColorUtility.ToHtmlStringRGB(islocal? localUserText : remoteUserText);

        string prettyString = $"<color=#{ucolor}>[{username}]:</color> <color=#{mcolor}>{message}</color>";

        return prettyString;
        
    }
}

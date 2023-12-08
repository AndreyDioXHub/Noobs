using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private GameObject _openNotificationM;
    [SerializeField]
    private GameObject _closeNotificationM;
    [SerializeField]
    private GameObject _inputField;
    TMP_InputField _input;


    [SerializeField]
    NotificationOptions defaulOption;

    public bool IsMobile;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        _input = _inputField.GetComponent<TMP_InputField>();
        CloseChat();
    }

    public void SetMobile()
    {
        IsMobile = true;

        _openNotification.SetActive(false);
        _closeNotification.SetActive(false);

        _openNotificationM.SetActive(true);
        _closeNotificationM.SetActive(false);
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

            if (IsMobile)
            {
                _openNotificationM.SetActive(false);
                _closeNotificationM.SetActive(true);

                _openNotification.SetActive(false);
                _closeNotification.SetActive(false);
            }
            else
            {
                _openNotification.SetActive(false);
                _closeNotification.SetActive(true);

                _openNotificationM.SetActive(false);
                _closeNotificationM.SetActive(false);
            }

            _inputField.SetActive(true);
            _input.ActivateInputField();
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

            if (IsMobile)
            {
                _openNotificationM.SetActive(true);
                _closeNotificationM.SetActive(false);

                _openNotification.SetActive(false);
                _closeNotification.SetActive(false);
            }
            else
            {
                _openNotification.SetActive(true);
                _closeNotification.SetActive(false);

                _openNotificationM.SetActive(false);
                _closeNotificationM.SetActive(false);
            }

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

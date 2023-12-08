using cyraxchel.network.chat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class PlayerNameToPref : MonoBehaviour
{
    [SerializeField]
    TMP_Text viewText;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    private List<string> _names = new List<string>();


    string username = "";
    ChatAuth _chatauth;

    // Start is called before the first frame update
    void Start()
    {
        _chatauth = (ChatAuth)ObstacleNetworkManager.singleton.authenticator;
        Load();
    }

    public void Load()
    {
        PlayerSave.Instance.ExecuteMyDelegateInQueue(LoadSave);
    }

    private void LoadSave()
    {
        if (YandexGame.SDKEnabled)
        {
            username = YandexGame.savesData.USER_NAME_KEY;
        } else {
            //NO SDK, load from player pref :)
            username = PlayerPrefs.GetString(PlayerPrefsConsts.USER_NAME_KEY);
        }

        if (viewText != null) {
            viewText.text = username;
        }

        if (inputField != null) {
            inputField.text = username;
        }

        _chatauth?.SetPlayername(username);
    }

    private string GetDefaultName() {
        string result = _names[UnityEngine.Random.Range(0, _names.Count)];

        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.USER_NAME_KEY = result;
            PlayerSave.Instance.Save();
        } else {
            PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, result);
        }

        return result;
    }

    /// <summary>
    /// When change in menu
    /// </summary>
    /// <param name="newName"></param>
    public void SetNewName(string newName) 
    {
        if(string.IsNullOrWhiteSpace(newName)) 
        {
            inputField.text = username;
            return;
        } 
        else 
        {
            newName = CensoredList.ReplaceText(newName);
            inputField.text = newName;
        }

        if(viewText != null) 
        {
            viewText.text = newName;
        }

        _chatauth?.SetPlayername(newName);

        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.USER_NAME_KEY = newName;
            PlayerSave.Instance.Save();
        } else {
            PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, newName);
        }
    }

    private void OnDestroy() {
        bool resave = false;
        if (string.IsNullOrWhiteSpace(username)) {
            username = GetDefaultName();
            _chatauth?.SetPlayername(username);
            resave = true;
        }
        if(!YandexGame.SDKEnabled) {
            if (resave) PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, username);
            PlayerPrefs.Save();
        }
    }
}

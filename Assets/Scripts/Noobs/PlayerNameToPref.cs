
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameToPref : MonoBehaviour
{
    [SerializeField]
    TMP_Text viewText;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    private List<string> _names = new List<string>();


    string username = "";

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    public void Load()
    {
        PlayerSave.Instance.ExecuteMyDelegateInQueue(LoadSave);
    }

    private void LoadSave()
    {
        username = PlayerSave.Instance.progress.USER_NAME_KEY;
        /*
        if (YandexGame.SDKEnabled)
        {
            username = YandexGame.savesData.USER_NAME_KEY;
        } 
        else 
        {
            //NO SDK, load from player pref :)
            username = PlayerPrefs.GetString(PlayerPrefsConsts.USER_NAME_KEY);
        }*/

        if (viewText != null) {
            viewText.text = username;
        }

        if (inputField != null) {
            inputField.text = username;
        }

        PlayerNetworkResolver.LocalUserName = username;
    }

    private string GetDefaultName() {
        string result = _names[UnityEngine.Random.Range(0, _names.Count)];

        PlayerSave.Instance.progress.USER_NAME_KEY = result;
        PlayerSave.Instance.Save();
        /*
        if (YandexGame.SDKEnabled)
        {
        } else {
            PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, result);
        }*/

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

        PlayerNetworkResolver.LocalUserName = newName;

        PlayerSave.Instance.progress.USER_NAME_KEY = newName;
        PlayerSave.Instance.Save();
        /*
        if (YandexGame.SDKEnabled)
        {
        } else {
            PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, newName);
        }*/
    }

    private void OnDestroy() 
    {
        bool resave = false;

        if (string.IsNullOrWhiteSpace(username)) 
        {
            username = GetDefaultName();
            PlayerNetworkResolver.LocalUserName = username;
            resave = true;
        }

        if (resave)
        {
            PlayerSave.Instance.progress.USER_NAME_KEY = username;
            PlayerSave.Instance.Save();
            /*
            PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, username);
            PlayerPrefs.Save();*/
        }

        /*
        if(!YandexGame.SDKEnabled) 
        {
            if (resave) 
                PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, username);
            PlayerPrefs.Save();
        }*/
    }
}

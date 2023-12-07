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
        PlayerNetworkResolver.LocalUserName = username;
    }

    private string GetDefaultName() {
        //TODO
        string result = _names[UnityEngine.Random.Range(0, _names.Count)];

        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.USER_NAME_KEY = result;
            PlayerSave.Instance.Save();
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

        PlayerNetworkResolver.LocalUserName = newName;

        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.USER_NAME_KEY = newName;
            PlayerSave.Instance.Save();
        }
    }

    private void OnDestroy() {
        if (string.IsNullOrWhiteSpace(username)) {
            username = GetDefaultName();
            PlayerNetworkResolver.LocalUserName = username;
        }
    }
}

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
        LoadSave();
    }

    private void LoadSave() {
        username = PlayerPrefs.GetString(PlayerPrefsConsts.USER_NAME_KEY, GetDefaultName());
        if(viewText != null) {
            viewText.text = username;
        }
        if(inputField != null) {
            inputField.text = username;
        }
    }

    private string GetDefaultName() {
        //TODO
        string result = _names[UnityEngine.Random.Range(0, _names.Count)];
        return result;// string.Empty;
    }


    public void SetNewName(string newName) {
        if(string.IsNullOrWhiteSpace(newName)) {
            inputField.text = username;
            return;
        } else {
            newName = CensoredList.ReplaceText(newName);
            inputField.text = newName;
        }
        if(viewText != null) {
            viewText.text = newName;
        }
        PlayerPrefs.SetString(PlayerPrefsConsts.USER_NAME_KEY, newName);
        PlayerPrefs.Save();
    }
}

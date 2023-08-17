using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private List<LocalizationText> _texts = new List<LocalizationText>();

    [SerializeField]
    private string _local = "ru";
    [SerializeField]
    private TextMeshProUGUI _text0;
    [SerializeField]
    private TextMeshProUGUI _text1;
    [SerializeField]
    private TextMeshProUGUI _text2;
    [SerializeField]
    private TextMeshProUGUI _text3;

    void Start()
    {
        _local = PlayerPrefs.GetString(PlayerPrefsConsts.LOCAL, "ru");

        _text0.text = _local;
        _text1.text = _local;
        _text2.text = _local;
        _text3.text = _local;

        LocalizationStrings.SetLanguage(_local);

        foreach (var t in _texts)
        {
            t.UpdateText();
        }

        int a = PlayerPrefs.GetInt(PlayerPrefsConsts.AUDIO, 1);

        if (a == 1)
        {
            StaticConsts.AUDIO_ON = true;
        }
        else
        {
            StaticConsts.AUDIO_ON = false;
        }
    }


    void Update()
    {
        
    }

    public void ChangeLanguage()
    {
        switch (_local)
        {
            case "ru":
                _local = "en";
                break;
            case "en":
                _local = "ru";
                break;
            default:
                _local = "ru";
                break;
        }

        PlayerPrefs.SetString(PlayerPrefsConsts.LOCAL, _local);

        _text0.text = _local;
        _text1.text = _local;
        _text2.text = _local;
        _text3.text = _local;
        LocalizationStrings.SetLanguage(_local);

        foreach(var t in _texts)
        {
            t.UpdateText();
        }
    }
}

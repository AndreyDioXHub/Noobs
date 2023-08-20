using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private List<LocalizationText> _texts = new List<LocalizationText>();

    [SerializeField]
    private YandexGame _sdk;

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

    /*
    [SerializeField]
    private TextMeshProUGUI _textlanguage;
    [SerializeField]
    private TextMeshProUGUI _textdomain;
    [SerializeField]
    private TextMeshProUGUI _textdeviceType;*/

    void Start()
    {
        _local = PlayerPrefs.GetString(PlayerPrefsConsts.LOCAL, "ru");

        if (LocalizationStrings.first_start)
        {
            switch (_local)
            {
                case "ru":
                    _text0.text = "русский";
                    _text1.text = "русский";
                    _text2.text = "русский";
                    _text3.text = "русский";
                    break;
                case "en":
                    _text0.text = "english";
                    _text1.text = "english";
                    _text2.text = "english";
                    _text3.text = "english";
                    break;
                default:
                    _text0.text = "русский";
                    _text1.text = "русский";
                    _text2.text = "русский";
                    _text3.text = "русский";
                    break;
            }
        }
        else
        {
            _sdk._RequestingEnvironmentData();
            InitLanguage();
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

    public void InitLanguage()
    {
        string lang = YandexGame.EnvironmentData.language;

        if(lang.Equals("ru") || lang.Equals("be") || lang.Equals("kk") || lang.Equals("uk")|| lang.Equals("uz"))
        {
            _local = "ru";

            switch (_local)
            {
                case "ru":
                    _text0.text = "русский";
                    _text1.text = "русский";
                    _text2.text = "русский";
                    _text3.text = "русский";
                    break;
                case "en":
                    _text0.text = "english";
                    _text1.text = "english";
                    _text2.text = "english";
                    _text3.text = "english";
                    break;
                default:
                    _text0.text = "русский";
                    _text1.text = "русский";
                    _text2.text = "русский";
                    _text3.text = "русский";
                    break;
            }

            LocalizationStrings.SetLanguage(_local);

            foreach (var t in _texts)
            {
                t.UpdateText();
            }
        }
        else
        {
            _local = "en";

            switch (_local)
            {
                case "ru":
                    _text0.text = "русский";
                    _text1.text = "русский";
                    _text2.text = "русский";
                    _text3.text = "русский";
                    break;
                case "en":
                    _text0.text = "english";
                    _text1.text = "english";
                    _text2.text = "english";
                    _text3.text = "english";
                    break;
                default:
                    _text0.text = "русский";
                    _text1.text = "русский";
                    _text2.text = "русский";
                    _text3.text = "русский";
                    break;
            }

            LocalizationStrings.SetLanguage(_local);

            foreach (var t in _texts)
            {
                t.UpdateText();
            }
        }

        PlayerPrefs.SetString(PlayerPrefsConsts.LOCAL, _local);
        LocalizationStrings.first_start = true;
    }


    void Update()
    {/*
        _textlanguage.text = YandexGame.EnvironmentData.language;
        _textdomain.text = YandexGame.EnvironmentData.domain;
        _textdeviceType.text = YandexGame.EnvironmentData.deviceType;*/

    }

    public void ChangeLanguage()
    {
        switch (_local)
        {
            case "ru":
                _local = "en";
                _text0.text = "english";
                _text1.text = "english";
                _text2.text = "english";
                _text3.text = "english";
                break;
            case "en":
                _local = "ru";
                _text0.text = "русский";
                _text1.text = "русский";
                _text2.text = "русский";
                _text3.text = "русский";
                break;
            default:
                _local = "ru";
                _text0.text = "русский";
                _text1.text = "русский";
                _text2.text = "русский";
                _text3.text = "русский";
                break;
        }

        PlayerPrefs.SetString(PlayerPrefsConsts.LOCAL, _local);

        LocalizationStrings.SetLanguage(_local);

        foreach(var t in _texts)
        {
            t.UpdateText();
        }
    }
}

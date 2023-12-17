using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LocalizationManager : MonoBehaviour
{
    public UnityEvent OnRequestingEnvironmentData = new UnityEvent();
    /*
    [SerializeField]
    private YandexGame _sdk;*/
    [SerializeField]
    private string _local = "ru";

    public string Locale => _local;

    [SerializeField]
    private TextLocalizer[] _texts;

    void Start()
    {
        _texts = Resources.FindObjectsOfTypeAll<TextLocalizer>();
        //_sdk._RequestingEnvironmentData();

        if (LocalizationStrings.first_start)
        {
            InitLanguage();
        }
        else
        {
            _local = PlayerPrefs.GetString(PlayerPrefsConsts.local, "ru");
        }
        OnRequestingEnvironmentData?.Invoke();
    }

    void Update()
    {

    }

    public void InitLanguage()
    {
        string lang = PlayerSave.Instance.progress.language;

        if (lang.Equals("ru") || lang.Equals("be") || lang.Equals("kk") || lang.Equals("uk") || lang.Equals("uz"))
        {
            _local = "ru";
        }
        else
        {
            _local = "en";
        }
        /*
        switch (_local)
        {
            case "ru":
                break;
            case "en":
                break;
            default:
                break;
        }*/

        LocalizationStrings.SetLanguage(_local);

        foreach (var t in _texts)
        {
            t.UpdateText();
        }

        PlayerPrefs.SetString(PlayerPrefsConsts.local, _local);
        PlayerPrefs.Save();

        LocalizationStrings.first_start = false;

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

        PlayerPrefs.SetString(PlayerPrefsConsts.local, _local);

        LocalizationStrings.SetLanguage(_local);

        foreach (var t in _texts)
        {
            t.UpdateText();
        }
    }
}

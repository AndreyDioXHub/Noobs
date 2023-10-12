using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class LocalizationManager : MonoBehaviour
{
    public UnityEvent OnRequestingEnvironmentData = new UnityEvent();

    [SerializeField]
    private YandexGame _sdk;
    [SerializeField]
    private string _local = "ru";

    [SerializeField]
    private TextLocalizer[] _texts;

    void Start()
    {
        if (YandexGame.Instance != null)
        {
            _sdk = YandexGame.Instance;

            _texts = Resources.FindObjectsOfTypeAll<TextLocalizer>();
            _sdk._RequestingEnvironmentData();
            OnRequestingEnvironmentData?.Invoke();

            if (LocalizationStrings.first_start)
            {
                InitLanguage();
            }
            else
            {
                _local = PlayerPrefs.GetString(PlayerPrefsConsts.local, "ru");
            }
        }

    }

    void Update()
    {

    }

    public void InitLanguage()
    {
        string lang = YandexGame.EnvironmentData.language;

        if (lang.Equals("ru") || lang.Equals("be") || lang.Equals("kk") || lang.Equals("uk") || lang.Equals("uz"))
        {
            _local = "ru";
        }
        else
        {
            _local = "en";
        }

        switch (_local)
        {
            case "ru":
                break;
            case "en":
                break;
            default:
                break;
        }

        LocalizationStrings.SetLanguage(_local);

        foreach (var t in _texts)
        {
            t.UpdateText();
        }

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

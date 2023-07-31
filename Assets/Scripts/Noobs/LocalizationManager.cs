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

    void Start()
    {
        _local = PlayerPrefs.GetString("PlatformLocal", "ru");

        _text0.text = _local;
        _text1.text = _local;

        LocalizationStrings.SetLanguage(_local);

        foreach (var t in _texts)
        {
            t.UpdateText();
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

        PlayerPrefs.SetString("PlatformLocal", _local);

        _text0.text = _local;
        _text1.text = _local;
        LocalizationStrings.SetLanguage(_local);

        foreach(var t in _texts)
        {
            t.UpdateText();
        }
    }
}

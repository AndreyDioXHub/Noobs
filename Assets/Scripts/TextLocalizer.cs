using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLocalizer : MonoBehaviour
{
    [SerializeField]
    private string _key;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    void Update()
    {

    }

    public void UpdateText()
    {
        if (_text != null)
        {
            try
            {
                _text.text = LocalizationStrings.Strings[_key];

            }
            catch (Exception e)
            {
                Debug.Log($"TextLocalizer {_key}");
            }
        }
    }
}

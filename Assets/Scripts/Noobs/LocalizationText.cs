using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    [SerializeField]
    private string _key;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    public void UpdateText()
    {
        if(_text!= null)
        {
            string text = "";

            try
            {
                _text.text = text;
            }
            catch (NullReferenceException e)
            {
                Debug.Log(name);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

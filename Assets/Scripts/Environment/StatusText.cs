using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusText : MonoBehaviour
{
    public static StatusText Instance;

    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private string _defaultText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        _text.text = _defaultText;
    }

    void Update()
    {

    }

    public void AddText(string text)
    {
        _text.text += $"\n{text}";
    }

    public void RemoveText(string text)
    {
        _text.text = _text.text.Replace($"\n{text}", "");
    }



}

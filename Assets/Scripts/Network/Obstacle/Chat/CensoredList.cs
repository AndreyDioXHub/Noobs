using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Censored;

public class CensoredList : MonoBehaviour
{
    Censor censor;

    [SerializeField]
    List<string> filterList;
    static CensoredList instance;

    [SerializeField]
    string locale = "ru";

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            if(instance.locale != locale) {
                Destroy(instance);
                instance = this;
            } else {
                Destroy(this);
                return;
            }
        }
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        if(filterList == null || filterList.Count ==0) {
            //TODO Try load from resources
            
        }
        Init();
    }

    private void Init() {
        censor = new Censor(filterList);
    }

    public static string ReplaceText(string text) {
        if(instance == null) {
            return text;
        }
        return instance.censor.CensorText(text);
    }

    [ContextMenu("Load list from file")]
    private void LoadFromFile() {
        //TODO
    }
}

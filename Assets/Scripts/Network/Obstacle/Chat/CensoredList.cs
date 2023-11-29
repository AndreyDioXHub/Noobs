using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Censored;
using System.IO;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class CensoredList : MonoBehaviour
{
    Censor censor;

    [SerializeField]
    List<string> filterList;
    static CensoredList instance;

    [SerializeField]
    string locale = "ru";
    
    [SerializeField]
    bool replace = false;

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
        #region Unity editor
#if UNITY_EDITOR
        string tfile = EditorUtility.OpenFilePanel("Select file",Application.dataPath, "txt");
        string[] listItems = File.ReadAllLines(tfile);
        if(replace) {
            filterList = listItems.ToList<string>();
        } else {
            filterList.AddRange(listItems);
        }
#endif
#endregion
    }
}

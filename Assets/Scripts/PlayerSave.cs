using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEditor;

public class PlayerSave : MonoBehaviour
{
    public static PlayerSave Instance;

    public delegate void OkCallbackDelegate();
    public Queue<OkCallbackDelegate> OkCallbacks = new Queue<OkCallbackDelegate>();
    public bool DataIsload => _dataIsload;
    public PlayerProgress progress;
    [SerializeField]
    private bool _dataIsload;
    [SerializeField]
    private bool _coroutineIsRunning;
    [SerializeField]
    private float _timeOut;


    private void OnEnable()
    { 
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Instance = null;
        _dataIsload = false;
        _coroutineIsRunning = false;
        OkCallbacks.Clear();
        OkCallbacks = new Queue<OkCallbackDelegate>();
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        string json = PlayerPrefs.GetString(PlayerPrefsConsts.save, "");
        progress = new PlayerProgress();
        progress = JsonConvert.DeserializeObject<PlayerProgress>(json);
        progress.platform = "isMobile";
        _dataIsload = true;// YandexGame.DataIsLoaded;// SceneManager.GetActiveScene().name.Equals("NoobLevelObstacleCourseNetwork") || SceneManager.GetActiveScene().name.Equals("NoobLevelObstacleCourseOffline");
        //Debug.Log($"YandexGame.SDKEnabled {YandexGame.SDKEnabled}");
    }


    void Update()
    {

    }

    public void GetLoad()
    {
        _dataIsload = true;// YandexGame. true;
    }

    public void ExecuteMyDelegateInQueue(OkCallbackDelegate mydelegate)
    {
        //Debug.Log($"delegates Added delegate {mydelegate.ToString()}");
        OkCallbacks.Enqueue(mydelegate);

        if (_coroutineIsRunning)
        {

        }
        else
        {
            StartCoroutine(ExecuteMyDelegateInQueueCoroutine());
        }

        /*
        var ok = OkCallbacks.Peek();
        ok();
        OkCallbacks.Dequeue();*/
    }

    IEnumerator ExecuteMyDelegateInQueueCoroutine()
    {


        _coroutineIsRunning = true;

        while (!_dataIsload)
        {
            //Debug.Log($"delegates cooroutine is running");
            yield return new WaitForEndOfFrame();
        }

        while (OkCallbacks.Count > 0)
        {
            //Debug.Log($"delegates count {OkCallbacks.Count}");
            var ok = OkCallbacks.Dequeue();
            ok();
            yield return new WaitForEndOfFrame();
        }

        _coroutineIsRunning = false;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        _coroutineIsRunning = false;
        Load();
    }

    public void Load()
    {
        StartCoroutine(LoadCoroutine());
    }
    IEnumerator LoadCoroutine()
    {
        Debug.Log($"delegates load start");
        yield return new WaitForSeconds(_timeOut);
        Debug.Log($"delegates load end");
        //YandexGame.LoadProgress();
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(progress);

        PlayerPrefs.SetString(PlayerPrefsConsts.save, json);
        PlayerPrefs.Save();
        //YandexGame.SaveProgress();
    }
}

[Serializable]
public class PlayerProgress
{
    public int askAuthorize = 0;
    public int askReject = 0;
    public string checkpoints = "";
    public int coins = 0;
    public int freeskin = 0;
    public string language = "ru";
    public string USER_NAME_KEY = "";
    public string USER_SKIN_KEY = "";
    public string platform = "";
}

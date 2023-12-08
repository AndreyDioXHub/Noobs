using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class PlayerSave : MonoBehaviour
{
    public static PlayerSave Instance;

    public delegate void OkCallbackDelegate();
    public Queue<OkCallbackDelegate> OkCallbacks = new Queue<OkCallbackDelegate>();
    public bool DataIsload => _dataIsload;
    [SerializeField]
    private bool _dataIsload;
    [SerializeField]
    private bool _coroutineIsRunning;
    [SerializeField]
    private float _timeOut;

    private void OnEnable()
    { 
        YandexGame.GetDataEvent += GetLoad;
    }

    private void OnDisable() 
    { 
        YandexGame.GetDataEvent -= GetLoad; 
    }

    private void OnDestroy()
    {
        YandexGame.GetDataEvent -= GetLoad;
        StopAllCoroutines();
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _dataIsload = SceneManager.GetActiveScene().name.Equals("NoobLevelObstacleCourseNetwork");
        Debug.Log($"YandexGame.SDKEnabled {YandexGame.SDKEnabled}");
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

        OkCallbackDelegate ok = () => Debug.Log("ghjghjghj");
        ExecuteMyDelegateInQueue(ok);
        /*
        OkCallbacks.Enqueue(delegate { });
        var ok = OkCallbacks.Peek();
        ok = () => Debug.Log("ghjghjghj");
        ok();
        OkCallbacks.Dequeue();*/
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
        YandexGame.LoadProgress();
    }

    public void Save()
    {
        YandexGame.SaveProgress();
    }
}

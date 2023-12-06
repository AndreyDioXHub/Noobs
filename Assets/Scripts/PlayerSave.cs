using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField]
    private InfoYG _info;

    private void OnEnable()
    { 
        YandexGame.GetDataEvent += GetLoad;
    }

    private void OnDisable() 
    { 
        YandexGame.GetDataEvent -= GetLoad; 
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchFlush()
    {
        _info.flush = !_info.flush;

    }

    void Start()
    {
        //Load();
    }


    void Update()
    {

    }

    public void GetLoad()
    {
        _dataIsload = true;
    }

    public void ExecuteMyDelegateInQueue(OkCallbackDelegate mydelegate)
    {
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
            yield return new WaitForEndOfFrame();
        }

        while (OkCallbacks.Count > 0)
        {
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
        yield return new WaitForSeconds(_timeOut);
        YandexGame.LoadProgress();
    }

    public void Save()
    {
        YandexGame.SaveProgress();
    }
}

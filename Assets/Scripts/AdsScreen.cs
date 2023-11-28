using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdsScreen : MonoBehaviour
{

    public static bool IsActive
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            else
            {
                return Instance.gameObject.activeSelf;
            }
        }
    }

    public static AdsScreen Instance;

    public UnityEvent OnTimeIsUp = new UnityEvent();

    [SerializeField]
    private float _time=3, timeCur;
    [SerializeField]
    private bool _isRelease;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
    }



    void Update()
    {
        timeCur += Time.deltaTime;

        if(timeCur > _time && !_isRelease)
        {
            _isRelease = true;
            OnTimeIsUp?.Invoke();
            timeCur = 0;
        }
    }

    private void OnDisable()
    {
        _isRelease = false;
        timeCur = 0;

    }
}

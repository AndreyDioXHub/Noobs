using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdsScreen : MonoBehaviour
{
    public static AdsScreen Instance;

    //public UnityEvent OnTimeIsUp = new UnityEvent();

    [SerializeField]
    private float _time = 3, timeCur;
    [SerializeField]
    private bool _isRelease;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<GameObject> _texts = new List<GameObject>();
    [SerializeField]
    private GameObject _button;
    [SerializeField]
    private GameObject _buttonStart;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //ShowImmidiatly();
        StartCoroutine(LateStart());
        //gameObject.SetActive(false);
    }

    IEnumerator LateStart()
    {
        gameObject.SetActive(true);
        _button.SetActive(false);
        _buttonStart.SetActive(true);

        foreach (var t in _texts)
        {
            t.SetActive(false);
        }

        _animator.enabled = false;
        yield return null;// new WaitForSeconds(0.5f);
        /*Time.timeScale = 0;
        AudioListener.volume = 0;*/

        AdsManager.Instance.ShowFullscreen();

        _isRelease = true;
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
            AdsManager.Instance.ShowFullscreen();
            //OnTimeIsUp?.Invoke();
            timeCur = 0;
        }
    }

    [ContextMenu("ShowImmidiatly")]
    public void ShowImmidiatly()
    {
        gameObject.SetActive(true);
        timeCur = _time;
        _button.SetActive(true);
        _buttonStart.SetActive(false);

        foreach (var t in _texts)
        {
            t.SetActive(false);
        }

        _animator.enabled = false;

        Time.timeScale = 0;
        AudioListener.volume = 0;
        AdsManager.Instance.ShowFullscreen();
    }



    private void OnDisable()
    {
        _isRelease = false;
        timeCur = 0;

        _animator.enabled = true;

        _button.SetActive(false);

        foreach (var t in _texts)
        {
            t.SetActive(false);
        }
        Time.timeScale = 1;

        float volume = PlayerPrefs.GetFloat(PlayerPrefsConsts.audio, 1);

        AudioListener.volume = volume;
    }
}

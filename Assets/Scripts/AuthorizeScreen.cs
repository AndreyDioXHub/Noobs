using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthorizeScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private GameObject _authorizeScreenReject;
    [SerializeField]
    private GameObject _authorizeButton;
    [SerializeField]
    private bool _authorized;
    [SerializeField]
    private bool _askReject;
    [SerializeField]
    private bool _askAuthorization;

    private void Awake()
    {
        _text.CrossFadeAlpha(0, 0, true);
        _text.gameObject.SetActive(false);
    }

    void Start()
    {
        /*if (YandexGame.DataIsLoaded)
        {
            _authorized = YandexGame.savesData.askAuthorize == 1;

            if (_authorized)
            {
                AuthorizeSuxess();
            }
        }*/
    }

    public void Authorize()
    {
        _text.CrossFadeAlpha(0, 0, true);
        _askAuthorization = true;
        //YandexGame.AuthDialog();
    }

    public void AuthorizeSuxess()
    {
        Debug.Log("Authorize Suxess");
        _text.CrossFadeAlpha(0, 0, true);

        PlayerSave.Instance.ExecuteMyDelegateInQueue(GetAuthorize);

        if (_askAuthorization)
        {
            _askAuthorization = false;
            //YandexGame.LoadProgress();
            SceneManager.LoadScene(0);
        }
    }

    public void GetAuthorize()
    {
        _authorized = PlayerSave.Instance.progress.askAuthorize == 1;

        _authorizeScreenReject.SetActive(false);

        if (_authorized)
        {
            _authorizeButton.SetActive(false);
        }
        else
        {

            PlayerSave.Instance.progress.askAuthorize = 1;
            PlayerSave.Instance.Save();
            _authorizeButton.SetActive(false);
            StartCoroutine(AuthorizeSuxessCoroutine());
        }
    }

    IEnumerator AuthorizeSuxessCoroutine()
    {
        _text.CrossFadeAlpha(0, 0, true);
        yield return new WaitForSeconds(0.02f);
        /*
        _text.gameObject.SetActive(true);
        _text.CrossFadeAlpha(1, _duration, true);
        yield return new WaitForSeconds(_duration);
        _text.CrossFadeAlpha(0, _duration, true);*/
    }

    public void AuthorizeReject()
    {
        Debug.Log("Authorize Reject");
        PlayerSave.Instance.ExecuteMyDelegateInQueue(GetAuthorizeReject);
    }

    public void GetAuthorizeReject()
    {
        _askReject = PlayerSave.Instance.progress.askReject == 1;

        if (!_askReject)
        {
            //_authorizeScreenReject.SetActive(true);
            PlayerSave.Instance.progress.askReject = 1;
            PlayerSave.Instance.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

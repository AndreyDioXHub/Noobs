using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

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

    private void Awake()
    {
        _text.CrossFadeAlpha(0, 0, true);
        _text.gameObject.SetActive(false);
    }

    void Start()
    {
        /*
        if (YandexGame.auth)
        {
            _authorizeScreen.SetActive(false);
            _authorizeButton.SetActive(false);
        }
        else
        {
            _authorized = PlayerPrefs.GetInt(PlayerPrefsConsts.askAuthorize, 0) == 1;

            if (!_authorized)
            {
                _authorizeScreen.SetActive(true);
                PlayerPrefs.SetInt(PlayerPrefsConsts.askAuthorize, 1);
            }
        }*/

    }

    public void Authorize()
    {
        _text.CrossFadeAlpha(0, 0, true);
        YandexGame.AuthDialog();
    }

    public void AuthorizeSuxess()
    {
        _text.CrossFadeAlpha(0, 0, true);
        _authorized = PlayerPrefs.GetInt(PlayerPrefsConsts.askAuthorize, 0) == 1;
        _authorizeScreenReject.SetActive(false);

        if (_authorized)
        {
            _authorizeButton.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsConsts.askAuthorize, 1);
            _authorizeButton.SetActive(false);
            StartCoroutine(AuthorizeSuxessCoroutine());
        }
    }

    IEnumerator AuthorizeSuxessCoroutine()
    {
        _text.CrossFadeAlpha(0, 0, true);
        yield return new WaitForSeconds(0.02f);
        _text.gameObject.SetActive(true);
        _text.CrossFadeAlpha(1, _duration, true);
        yield return new WaitForSeconds(_duration);
        _text.CrossFadeAlpha(0, _duration, true);
    }

    public void AuthorizeReject()
    {
        _askReject = PlayerPrefs.GetInt(PlayerPrefsConsts.askReject, 0) == 1;

        if (!_askReject)
        {
            _authorizeScreenReject.SetActive(true);
            PlayerPrefs.SetInt(PlayerPrefsConsts.askReject, 1);
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

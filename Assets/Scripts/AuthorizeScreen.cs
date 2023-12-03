using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class AuthorizeScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _authorizeScreen;
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

    void Start()
    {
        _text.CrossFadeAlpha(0, 0, true);

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
        }

    }

    public void Authorize() 
    {
        YandexGame.AuthDialog();
    }

    public void AuthorizeSuxess()
    {
        _text.CrossFadeAlpha(0, 0, true);
        _authorized = PlayerPrefs.GetInt(PlayerPrefsConsts.askAuthorize, 0) == 1;
        _authorizeScreen.SetActive(false);
        _authorizeButton.SetActive(false);
        StartCoroutine(AuthorizeSuxessCoroutine());
    }

    IEnumerator AuthorizeSuxessCoroutine()
    {
        if (_authorized)
        {

        }
        else
        {
            _text.CrossFadeAlpha(1, _duration, true);
            yield return new WaitForSeconds(_duration);
            _text.CrossFadeAlpha(0, _duration, true);
        }
    }

    public void AuthorizeReject()
    {
        if (_authorized)
        {
            _authorizeScreenReject.SetActive(true);
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

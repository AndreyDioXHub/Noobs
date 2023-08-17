using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioView : MonoBehaviour
{
    [SerializeField]
    private GameObject _audioOn, _audioOff;

    void Start()
    {
        int a = PlayerPrefs.GetInt(PlayerPrefsConsts.AUDIO, 0);

        if (a == 1)
        {
            _audioOn.SetActive(true);
            _audioOff.SetActive(false);
            StaticConsts.AUDIO_ON = true;
        }
        else
        {
            _audioOn.SetActive(false);
            _audioOff.SetActive(true);
            StaticConsts.AUDIO_ON = false;
        }
    }

    void Update()
    {
        if (StaticConsts.AUDIO_ON)
        {
            _audioOn.SetActive(true);
            _audioOff.SetActive(false);
        }
        else
        {
            _audioOn.SetActive(false);
            _audioOff.SetActive(true);

        }
    }

    public void AudioOn()
    {
        PlayerPrefs.SetInt(PlayerPrefsConsts.AUDIO, 1);
        _audioOn.SetActive(true);
        _audioOff.SetActive(false);
        StaticConsts.AUDIO_ON = true;
    }

    public void AudioOff()
    {
        PlayerPrefs.SetInt(PlayerPrefsConsts.AUDIO, 0);
        _audioOn.SetActive(false);
        _audioOff.SetActive(true);
        StaticConsts.AUDIO_ON = false;
    }
}

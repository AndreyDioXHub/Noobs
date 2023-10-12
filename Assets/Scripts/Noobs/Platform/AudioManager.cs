using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _audioOn;
    [SerializeField]
    private GameObject _audioOff;
    [SerializeField]
    private Slider _audioSlider;
    [SerializeField]
    private float _volume = 1;

    void Start()
    {
        _volume = PlayerPrefs.GetFloat(PlayerPrefsConsts.audio, 1);
        _volume = _volume > 1 ? 1 : _volume;

        AudioListener.volume = _volume;

        if (_audioOn != null && _audioOff != null && _audioSlider != null)
        {
            _audioSlider.value = _volume;

            if (_volume == 0)
            {
                _audioOn.SetActive(false);
                _audioOff.SetActive(true);
            }
            else
            {
                _audioOn.SetActive(true);
                _audioOff.SetActive(false);
            }
        }
    }

    public void UpdateVolume(float value)
    {
        _volume = value;
        PlayerPrefs.SetFloat(PlayerPrefsConsts.audio, _volume);

        AudioListener.volume = _volume;

        if (_audioOn != null && _audioOff != null && _audioSlider != null)
        {
            _audioSlider.value = _volume;

            if (_volume == 0)
            {
                _audioOn.SetActive(false);
                _audioOff.SetActive(true);
            }
            else
            {
                _audioOn.SetActive(true);
                _audioOff.SetActive(false);
            }
        }
    }

    public void SwitchVolume()
    {
        if (_volume == 0)
        {
            _volume = 1;
        }
        else
        {
            _volume = 0;
        }

        UpdateVolume(_volume);
    }

    public void MuteOnLoad()
    {
        AudioListener.volume = 0;
    }
}

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

    [SerializeField]
    private AudioSource _music;
    [SerializeField]
    private GameObject _audioMusicOn;
    [SerializeField]
    private GameObject _audioMusicOff;
    [SerializeField]
    private Slider _audioMusicSlider;
    [SerializeField]
    private float _volumeMusic = 1;


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

        _volumeMusic = PlayerPrefs.GetFloat(PlayerPrefsConsts.music, 1);
        _volumeMusic = _volumeMusic > 1 ? 1 : _volumeMusic;

        _music.volume = _volumeMusic * 0.5f;

        if (_audioMusicOn != null && _audioMusicOff != null && _audioMusicSlider != null)
        {
            _audioMusicSlider.value = _volumeMusic;

            if (_volumeMusic == 0)
            {
                _audioMusicOn.SetActive(false);
                _audioMusicOff.SetActive(true);
            }
            else
            {
                _audioMusicOn.SetActive(true);
                _audioMusicOff.SetActive(false);
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

    public void UpdateMusicVolume(float value)
    {
        _volumeMusic = value;
        PlayerPrefs.SetFloat(PlayerPrefsConsts.music, _volumeMusic);

        _music.volume = _volumeMusic * 0.5f;

        if (_audioMusicOn != null && _audioMusicOff != null && _audioMusicSlider != null)
        {
            _audioMusicSlider.value = _volumeMusic;

            if (_volumeMusic == 0)
            {
                _audioMusicOn.SetActive(false);
                _audioMusicOff.SetActive(true);
            }
            else
            {
                _audioMusicOn.SetActive(true);
                _audioMusicOff.SetActive(false);
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

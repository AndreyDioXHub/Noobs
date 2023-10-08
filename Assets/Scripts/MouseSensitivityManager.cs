using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    [SerializeField]
    private CameraView _cameraView;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private float _sliderValue;

    void Start()
    {
        _sliderValue = PlayerPrefs.GetFloat(PlayerPrefsConsts.sensitivity, 0.25f);
        _slider.value = _sliderValue;
    }

    public void UpdateValue(float value)
    {
        _sliderValue = value;
        PlayerPrefs.SetFloat(PlayerPrefsConsts.sensitivity, _sliderValue);
        _slider.value = _sliderValue;
        _cameraView.SetSensitivity(_sliderValue);
    }

}

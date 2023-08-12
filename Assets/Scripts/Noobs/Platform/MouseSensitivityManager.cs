using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    void Start()
    {

        StaticConsts.MOUSE_SENSITIVITY = PlayerPrefs.GetInt(PlayerPrefsConsts.SENSITIVITY, 300);
        _slider.value = (float)(StaticConsts.MOUSE_SENSITIVITY)/600;
    }

    void Update()
    {
        
    }

    public void ChangeSensitivity(float value)
    {
        int sensitivity = (int)(value * 600);
        StaticConsts.MOUSE_SENSITIVITY = sensitivity;
        PlayerPrefs.SetInt(PlayerPrefsConsts.SENSITIVITY, sensitivity);

    }
}

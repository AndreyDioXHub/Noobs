using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdsButtonView : MonoBehaviour
{
    public static AdsButtonView Instance;
    public GameObject Parent;

    [SerializeField]
    private GameObject _timer;
    [SerializeField]
    private GameObject _button;

    [SerializeField]
    private TextMeshProUGUI _textTime;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {

        if (AdsManager.Instance.IsPaused)
        {
            _timer.SetActive(true);
            _button.SetActive(false);

            _textTime.text = AdsManager.Instance.TimeString;
        }
        else
        {
            _timer.SetActive(false);
            _button.SetActive(true);
        }
    }
}

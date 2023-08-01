using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _screen;
    [SerializeField]
    private float _showTime = 2;
    [SerializeField]
    private float _showTimeCur = 2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _showTimeCur += Time.deltaTime;

        if (_showTimeCur > _showTime)
        {
            _screen.SetActive(false);
        }
        else
        {
            _screen.SetActive(true);
        }
    }

    public void Show()
    {
        _showTimeCur = 0;
    }
}

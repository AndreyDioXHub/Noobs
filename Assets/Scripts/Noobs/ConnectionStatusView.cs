using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionStatusView : MonoBehaviour
{
    [SerializeField]
    private GameObject _ready;
    [SerializeField]
    private GameObject _failed;


    public void ResetView()
    {
        _ready.SetActive(false);
        _failed.SetActive(false);
    }

    public void Show(int status)
    {
        switch (status)
        {
            case 0:
                _failed.SetActive(true);
                break;
            case 1:
                _ready.SetActive(true);
                break;
            default:
                break;
        }
    }
}

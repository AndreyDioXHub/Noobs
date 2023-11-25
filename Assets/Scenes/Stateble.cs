using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stateble : MonoBehaviour
{
    public UnityEvent<int, bool> OnStateSwitched = new UnityEvent<int, bool>();
    public int ID => _id;
    [SerializeField]
    private GameObject _active;
    [SerializeField]
    private GameObject _default;
    [SerializeField]
    private GameObject _screen;
    [SerializeField]
    private int _id;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void MakeActive()
    {
        _active.SetActive(true);
        _default.SetActive(false);
        if (_screen != null)
        {
            _screen.SetActive(true);
        }
        OnStateSwitched?.Invoke(_id, true);
    }

    public void MakePassive()
    {
        _active.SetActive(false);
        _default.SetActive(true);
        if (_screen != null)
        {
            _screen.SetActive(false);
        }
        OnStateSwitched?.Invoke(_id, false);
    }
}

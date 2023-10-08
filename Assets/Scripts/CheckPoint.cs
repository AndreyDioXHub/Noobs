using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public CheckPointState State = CheckPointState.passive;
    public bool IsFinish;
    public BoxCollider _box;
    private bool _itIsWasActive;

    void Start()
    {
        _box = GetComponent<BoxCollider>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckPointManager.Instance.SetActiveCheckPoint(this);
        _box.enabled = false;
    }
    /*
    public bool TrySetActive()
    {
        bool result = false;

        if (_itIsWasActive)
        {

        }
        else
        {
            State = CheckPointState.active;
            _itIsWasActive = true;
        }

        return result;
    }*/
}

public enum CheckPointState
{
    passive,
    active
}

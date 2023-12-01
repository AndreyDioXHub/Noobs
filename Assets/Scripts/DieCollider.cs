using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCollider : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        LifeManager.Instance.PlayerDead();
        other.transform.SetParent(null);
        //CheckPointManager.Instance.SetNewRecord();
    }
}

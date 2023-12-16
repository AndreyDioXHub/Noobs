using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        if (other.tag.Equals("Player"))
        {
            LifeManager.Instance.PlayerDead();
            other.transform.SetParent(null);
            other.GetComponent<RobloxController>().OnDie?.Invoke(true);
        }
    }
}

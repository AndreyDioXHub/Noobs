using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject _heart;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<LifeManager>(out LifeManager life))
        {
            life.MinusLife();
        }

        if (other.tag.Equals("Player"))
        {
            Destroy(_heart);
            Destroy(gameObject);
        }

    }
}

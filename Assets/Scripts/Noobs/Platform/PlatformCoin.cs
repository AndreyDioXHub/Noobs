using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCoin : MonoBehaviour
{
    [SerializeField]
    private GameObject _father;

    void Start()
    {
        GameObject eatEffect = Instantiate(Resources.Load<GameObject>("WorldSphereEatEffect"));
        eatEffect.transform.position = transform.position;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlatformCoinManager.Instance.AddCoin();

        Destroy(gameObject);
        Destroy(_father);
    }

    private void OnDestroy()
    {
        GameObject eatEffect = Instantiate(Resources.Load<GameObject>("WorldSphereEatEffect"));
        eatEffect.transform.position = transform.position;
    }
}

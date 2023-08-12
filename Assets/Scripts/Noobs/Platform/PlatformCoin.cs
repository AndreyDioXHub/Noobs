using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCoin : MonoBehaviour
{
    [SerializeField]
    private GameObject _father;

    private bool _isEffected;

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

        GameObject eatEffect = Instantiate(Resources.Load<GameObject>("WorldSphereEatEffectCoinSound"));
        eatEffect.transform.position = transform.position;

        _isEffected = true;

        Destroy(gameObject);
        Destroy(_father);
    }

    private void OnDestroy()
    {
        if (!_isEffected)
        {
            GameObject eatEffect = Instantiate(Resources.Load<GameObject>("WorldSphereEatEffect"));
            eatEffect.transform.position = transform.position;

        }
    }
}

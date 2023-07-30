using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _platform;

    [SerializeField]
    private float _timeToDestroy = 3;
    [SerializeField]
    private float _timeToDestroyCur;
    [SerializeField]
    private bool _isNeedDestroy;


    void Start()
    {
        
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyPlatform();
    }

    public void DestroyPlatform()
    {
        Destroy(_platform, _timeToDestroy);
        _isNeedDestroy = true;
        _animator.SetTrigger("Step");
    }

}

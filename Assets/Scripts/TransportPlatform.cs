using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform _p0;
    [SerializeField]
    private Transform _p1;
    [SerializeField]
    private float _time = 3, _timeCur;
    [SerializeField]
    private bool _isForward;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(_isForward)
        {
            _timeCur += Time.fixedDeltaTime;
        }
        else
        {
            _timeCur -= Time.fixedDeltaTime;
        }

        _timeCur = _timeCur > _time ? _time : _timeCur;
        _timeCur = _timeCur < 0 ? 0 : _timeCur;

        transform.position = Vector3.Lerp(_p0.position, _p1.position, _timeCur / _time);

        if(_timeCur == _time)
        {
            _isForward = false;
        }

        if(_timeCur == 0)
        {
            _isForward = true;
        }
    }
}

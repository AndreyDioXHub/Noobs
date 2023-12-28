using cyraxchel.network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkTransformReliableFixed))]
public class TransportPlatform : NetworkBehaviour
{
    [SerializeField]
    private Transform _p0;
    [SerializeField]
    private Transform _p1;
    [SerializeField]
    private float _time = 3, _timeCur;
    [SerializeField]
    private float _timePause = 0, _timePauseCur;
    [SerializeField]
    private bool _isPause;
    [SerializeField]
    private bool _isForward;


    void Start()
    {
        
    }

    [ContextMenu("SpawnPoints")]
    public void SpawnPoints()
    {
        GameObject p0 = new GameObject();
        p0.transform.position = transform.position;
        p0.name = $"{transform.name} (0)";
        _p0 = p0.transform;

        GameObject p1 = new GameObject();
        p1.transform.position = transform.position;
        p1.name = $"{transform.name} (1)";
        _p1 = p1.transform;
    }

    void FixedUpdate()
    {
        
        if (_isPause)
        {
            _timePauseCur += Time.fixedDeltaTime;

            if (_timePauseCur > _timePause)
            {
                _timePauseCur = 0;
                _isPause = false;
            }
        }
        else
        {
            if (_isForward)
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

            if (_timeCur == _time)
            {
                _isForward = false;
                _isPause = true;
            }

            if (_timeCur == 0)
            {
                _isForward = true;
                _isPause = true;
            }
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        if(isClientOnly) {
            this.enabled = false;
        }
    }
}

using cyraxchel.network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkTransformReliableFixed))]
public class TransportRotate : NetworkBehaviour
{
    [SerializeField]
    private Vector3 _angle0, _angle1; 
    [SerializeField]
    private float _time = 5, _timeCur;
    [SerializeField]
    private float _timePause = 5, _timePauseCur;
    [SerializeField]
    private bool _isPause;
    [SerializeField]
    private bool _isForward;
    [SerializeField]
    private bool _needReturn = true;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (_needReturn)
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

                transform.eulerAngles = Vector3.Lerp(_angle0, _angle1, _timeCur / _time);

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
        else
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

                _timeCur += Time.fixedDeltaTime;

                _timeCur = _timeCur > _time ? _time : _timeCur;

                transform.eulerAngles = Vector3.Lerp(_angle0, _angle1, _timeCur / _time);

                if (_timeCur == _time)
                {
                    _isPause = true;
                    _timeCur = 0;
                }
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

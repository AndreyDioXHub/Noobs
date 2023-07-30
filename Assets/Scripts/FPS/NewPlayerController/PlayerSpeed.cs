using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    public float Speed { get => _speed; }

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedDestenation;
    [SerializeField]
    private float _speedCurent;

    [SerializeField]
    private float _timeChange = 0.5f;
    [SerializeField]
    private float _timeChangeCur;


    void Start()
    {
        
    }


    void FixedUpdate()
    {
        if(_timeChangeCur > _timeChange)
        {
            _speed = _speedDestenation;
        }
        else
        {
            _timeChangeCur += Time.fixedDeltaTime;
            _speed = _speedCurent + (_speedDestenation - _speedCurent) * _timeChangeCur / _timeChange;
        }
    }

    public void SetSpeed(float speed)
    {
        //Debug.Log(speed);
        _speedCurent = _speed;
        _speedDestenation = speed;
        _timeChangeCur = 0;
    }
}

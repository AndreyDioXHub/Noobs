using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField]
    private Transform _anchor;
    [SerializeField]
    private Vector3 _positionNext;
    [SerializeField]
    private Vector3 _positionCurent;
    [SerializeField]
    private float _timeBetweenNewCall = 0.5f;
    [SerializeField]
    private float _timeCurent;


    void Start()
    {
        _positionNext = Ocean.Instance.WaterPosition(_anchor.position);
        _positionCurent = Ocean.Instance.WaterPosition(_anchor.position);
        transform.position = _positionNext;     
    }


    void FixedUpdate()
    {
        _timeCurent += Time.fixedDeltaTime;

        if (_timeCurent > _timeBetweenNewCall)
        {
            _timeCurent = 0;
            _positionCurent = _positionNext;
            _positionNext = Ocean.Instance.WaterPosition(_anchor.position);
        }

        transform.position = Vector3.Lerp(_positionCurent, _positionNext, _timeCurent / _timeBetweenNewCall);
    }
}

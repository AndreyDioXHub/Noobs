using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float _f = 1;


   void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetKey("w"))
        {
            _f += _speed * Time.deltaTime;
            
        }

        if (Input.GetKey("s"))
        {

            _f -= _speed * Time.deltaTime;
        }

        if (_f < 1)
        {
            _f = 1;
        }
        _camera.nearClipPlane = _f;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimeController : MonoBehaviour
{
    public float TimeG { get => _time; }
    [SerializeField]
    private List<Material> _waveMtls = new List<Material>();
    private float _time;
    [SerializeField]
    private int _missFrame = 60;
    [SerializeField]
    private int _missFrameCur = 0;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        _missFrameCur++;

        _time += Time.fixedDeltaTime;

        if(_missFrameCur > _missFrame)
        {
            _missFrameCur = 0;

            for (int i = 0; i < _waveMtls.Count; i++)
            {
                _waveMtls[i].SetFloat("_WavesTime", _time);

            }

        }
    }

    internal void SetNewTime(float newval) {
        Debug.Log($"Update wave time to {newval}");
        _time = newval;
    }

    internal float GetTime() {
        return _time;
    }
}

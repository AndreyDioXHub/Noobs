using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDirection : MonoBehaviour
{
    [SerializeField]
    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _material.SetVector("_CutPosition", transform.position);
        _material.SetVector("_WaterNormal", transform.up);
    }
}

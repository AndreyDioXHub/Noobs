using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform conten;

    void Start()
    {
        for(int i=0; i<4; i++)
        {
            var go = Instantiate(prefab, conten);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

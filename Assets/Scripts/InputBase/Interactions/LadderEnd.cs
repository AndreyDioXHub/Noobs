using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderEnd : MonoBehaviour
{
    [SerializeField]
    private LadderInteraction _ladderInteraction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
        }
    }
}

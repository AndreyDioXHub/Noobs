using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairColorModel : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _hairs = new List<GameObject>();
    [SerializeField]
    private int _index;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Select(int index)
    {
        if (_index == index)
        {
            foreach(var hair in _hairs)
            {
                hair.SetActive(true);
            }
        }
        else
        {
            foreach (var hair in _hairs)
            {
                hair.SetActive(false);
            }
        }
    }

}

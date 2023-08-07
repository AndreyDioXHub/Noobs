using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renamer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Rename")]
    public void Rename()
    {
        var transforms = GetComponentsInChildren<Transform>();

        for (int i=0; i< transforms.Length; i++)
        {
            transforms[i].gameObject.name = $"object {i}";
        }
    }
}

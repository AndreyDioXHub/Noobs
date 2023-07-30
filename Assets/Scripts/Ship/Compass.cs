using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = transform.localEulerAngles;
        rot.y = target.eulerAngles.y * -1;
        transform.localEulerAngles = rot;
    }
}

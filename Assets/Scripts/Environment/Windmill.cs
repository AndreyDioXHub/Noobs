using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + _rotationSpeed * Time.deltaTime, 0);
    }
}

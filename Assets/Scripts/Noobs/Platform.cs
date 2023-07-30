using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Collider _ground;

   void Start()
    {

        //_details[Random.Range(0, _details.Count - 1)].SetActive(true);
        /*_details[Random.Range(0, _details.Count - 1)].SetActive(true);
        _details[Random.Range(0, _details.Count - 1)].SetActive(true);*/

    }


    void Update()
    {
        
    }

    public void DisableGround()
    {
        _ground.enabled = false;
    }
}

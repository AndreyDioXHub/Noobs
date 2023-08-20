using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOffcetBlender : MonoBehaviour
{

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private List<Vector3> _offcets = new List<Vector3>();

    float lastTime = 0;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 offcetSumm = Vector3.zero;

        foreach(var offcet in _offcets)
        {
            offcetSumm += offcet;
        }

        _characterController.Move(offcetSumm);

        _offcets.Clear();
        lastTime = Time.realtimeSinceStartup;
        _offcets = new List<Vector3>();
    }

    public void AddOffcet(Vector3 offcet)
    {
        _offcets.Add(offcet);
        //Reclean after 2 seconds
        if(Time.realtimeSinceStartup - lastTime > 2) {
            _offcets.Clear();
        }
    }
}

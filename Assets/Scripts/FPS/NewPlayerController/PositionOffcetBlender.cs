using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOffcetBlender : MonoBehaviour
{

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private List<Vector3> _offcets = new List<Vector3>();

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
        _offcets = new List<Vector3>();
    }

    public void AddOffcet(Vector3 offcet)
    {
        if(this.enabled) {
            _offcets.Add(offcet);
        }
    }

    public void Move(Vector3 newPosition) {
        _offcets.Clear();
        _offcets = new List<Vector3>();

        _characterController.enabled = false;
        transform.position = newPosition;
        _characterController.enabled = true;
    }
}

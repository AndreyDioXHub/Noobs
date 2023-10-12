using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOffcetBlender : MonoBehaviour
{

    public static PositionOffcetBlender Instance;

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private List<Vector3> _offcets = new List<Vector3>();

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 offcetSumm = Vector3.zero;

        if (transform.position.y >= 0)
        {
            foreach (var offcet in _offcets)
            {
                offcetSumm += offcet;
            }

            _characterController.Move(offcetSumm);

            _offcets.Clear();
            _offcets = new List<Vector3>();
        }
        else
        {
            _offcets.Clear();
            _offcets = new List<Vector3>();
        }

        /*
        Vector3 offcetSumm = Vector3.zero;

        foreach(var offcet in _offcets)
        {
            offcetSumm += offcet;
        }

        _characterController.Move(offcetSumm);

        _offcets.Clear();
        _offcets = new List<Vector3>();*/
    }

    public void AddOffcet(Vector3 offcet)
    {
        if (this.enabled)
            _offcets.Add(offcet);
    }
}

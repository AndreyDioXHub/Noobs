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
    [SerializeField]
    private bool _onTranspor;
    private Transform _transportPosition;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    public void ReturnToCheckPoint(Vector3 position)
    {
        _offcets.Clear();
        _offcets = new List<Vector3>();

        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
    }

    public void OnTransport(Transform transportPosition)
    {
        _onTranspor = true;
        _transportPosition = transportPosition;
    } 

    public void TransportExit()
    {
        _onTranspor = false;
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

            if (_onTranspor)
            {
                _characterController.enabled = false;
                transform.position = new Vector3( _transportPosition.position.x, transform.position.y, _transportPosition.position.z);
                _characterController.enabled = true;
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
    }

    public void AddOffcet(Vector3 offcet)
    {/*
        if (SettingScreen.Instance.gameObject.activeSelf)
        {
            return;
        }*/

        if (this.enabled)
            _offcets.Add(offcet);
    }
}

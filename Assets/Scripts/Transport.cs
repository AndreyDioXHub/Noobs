using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private Vector3 _firstDelta;
    [SerializeField]
    private PositionOffcetBlender _blender;
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(_parent);
        /*_groundCheck = other.GetComponent<GroundCheck>();
        _groundCheck.IsGroundedBB(true);
        _firstDelta = _parent.position - other.transform.position;*/
        _blender = other.GetComponent<PositionOffcetBlender>();//.AddOffcet(_firstDelta);
        _blender.OnTransport(_parent);
    }

    private void OnTriggerStay(Collider other)
    {
        //Vector3 delta = _parent.position - other.transform.position;
        //Debug.Log(delta);
        // other.GetComponent<PositionOffcetBlender>().AddOffcet(delta - _firstDelta);// transform.position += delta;
        if (other.transform.localPosition.y > 2)
        {
            if(_blender!= null)
            {
                _blender.TransportExit();
                _blender = null;
                other.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("fgjg hgh gjg h");
        // other.GetComponent<GroundCheck>().IsGroundedBB(false);

    }
}

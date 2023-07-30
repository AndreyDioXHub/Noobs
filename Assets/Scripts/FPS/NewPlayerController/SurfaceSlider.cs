using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    public bool IsGrounded;

    [SerializeField]
    private Rigidbody _rigidbody;

    private Vector3 _normal;


    public Vector3 Project(Vector3 forward)
    {
        Vector3 result = Vector3.zero;
        if(_normal.y == -1)
        {
            result = _normal;
        }
        else
        {
            result = forward - Vector3.Dot(forward, _normal) * _normal;
        }
        return result;
    }

    public void UseGravity(bool state)
    {
        _rigidbody.useGravity = state;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = Vector3.zero;

        foreach(var contact in collision.contacts)
        {
            if (contact.thisCollider.tag.Equals("Terrain"))
            {
                normal = Vector3.Cross(normal,collision.contacts[0].normal).normalized;
            }
        }

        _normal = normal;

        _rigidbody.useGravity = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
        //_normal = Vector3.down;
    }


    private void OnCollisionStay(Collision collision)
    {
        IsGrounded = true;
        //_normal = Vector3.zero;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _normal*3);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward));
    }*/
}

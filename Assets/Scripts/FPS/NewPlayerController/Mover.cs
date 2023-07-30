using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Vector3 forward;
    public Vector3 offcet;
    public Rigidbody _rigidbody;
    public float speed;

    [SerializeField]
    private float _mouseSensitivity = 100f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetKey("w"))
        {
            offcet = forward * speed * Time.deltaTime;
            _rigidbody.MovePosition(transform.position + offcet);
            //Debug.Log("w");
        }


    }

    void OnCollisionStay(Collision collisionInfo)
    {

        //Debug.DrawRay(collisionInfo.contacts[0].point, collisionInfo.contacts[0].normal * 10, Color.red);

        forward = transform.forward - Vector3.Dot(transform.forward, collisionInfo.contacts[0].normal) * collisionInfo.contacts[0].normal;
        Debug.Log(Vector3.Dot(transform.forward, collisionInfo.contacts[0].normal));
        Debug.DrawRay(transform.position, forward, Color.red);
        //Debug.DrawRay(transform.position, transform.forward.normalized, Color.red);
        //Debug.Log(Vector3.Cross( transform.forward.normalized, collisionInfo.contacts[0].normal.normalized).magnitude);
        /*
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        forward = Vector3.down;
    }

    /* public Vector3 Project(Vector3 forward)
     {

     }*/
}

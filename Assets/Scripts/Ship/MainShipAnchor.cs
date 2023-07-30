using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShipAnchor : MonoBehaviour
{
    [SerializeField]
    private float _anchorSpace = 4;

    [SerializeField]
    private Transform _anchor0;
    [SerializeField]
    private Transform _anchor1;
    [SerializeField]
    private Transform _anchor2;
    [SerializeField]
    private Transform _anchor3;

    [SerializeField]
    private Transform _floating0;
    [SerializeField]
    private Transform _floating1;
    [SerializeField]
    private Transform _floating2;
    [SerializeField]
    private Transform _floating3;

    [SerializeField]
    private Transform _shipAnchor;

    //[SerializeField]
    //Transform ProjectPoint;

    void Start()
    {
        //AplyConfig();
    }

    [ContextMenu("Aply Config")]
    public void AplyConfig()
    {
        _anchor0.localPosition = new Vector3(_anchorSpace, 0, _anchorSpace);
        _anchor1.localPosition = new Vector3(-_anchorSpace, 0, _anchorSpace);
        _anchor2.localPosition = new Vector3(_anchorSpace, 0, -_anchorSpace);
        _anchor3.localPosition = new Vector3(-_anchorSpace, 0, -_anchorSpace);
    }

    void Update()
    {
        _shipAnchor.position = (_floating0.position + _floating1.position + _floating2.position + _floating3.position) / 4;

        Vector3 sideA = _floating0.position - _floating1.position;
        Vector3 sideB = _floating0.position - _floating2.position;

        Vector3 normalAB = Vector3.Cross(sideB, sideA);

        Vector3 sideC = _floating3.position - _floating1.position;
        Vector3 sideD= _floating3.position - _floating2.position;

        Vector3 normalCD = Vector3.Cross(sideC, sideD);

        Debug.DrawRay(_shipAnchor.position, normalAB.normalized *2, Color.green);
        Debug.DrawRay(_shipAnchor.position, normalCD.normalized, Color.green);
       
        Vector3 normal = normalAB + normalCD;// Vector3.Cross(normalAB, normalCD);
        Vector3 planeprojection = Vector3.ProjectOnPlane(transform.forward, normal).normalized;

        Debug.DrawRay(_shipAnchor.position, planeprojection, Color.red);
        Debug.DrawRay(_shipAnchor.position, normal.normalized*5, Color.blue);

        _shipAnchor.LookAt(planeprojection * -1 + _shipAnchor.position, normal);
    }
}

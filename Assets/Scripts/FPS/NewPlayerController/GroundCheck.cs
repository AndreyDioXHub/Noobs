using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded{ get 
        {
            if (_pause)
            {
                return false;
            }

            return _isGrounded;// _controller.isGrounded;
        }
    }

    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private bool _pause;
    [SerializeField]
    private bool _isGrounded;

    private void Awake()
    {
        this.enabled = false;
    }

    public void Pause()
    {
        StartCoroutine(CoroutinePause());
    }

    IEnumerator CoroutinePause()
    {
        _pause = true;
        yield return new WaitForSeconds(0.5f);
        _pause = false;
    }
    void FixedUpdate()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.92f))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        _isGrounded = _isGrounded || _controller.isGrounded;
    }

    /*
    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private bool _isGroundedCur;

    [SerializeField]
    private float _lagTime = 0.05f;
    [SerializeField]
    private float _lagTimeCur;


   private void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.tag.Equals("Player"))
        {
            //_lagTimeCur = 0;
            _isGroundedCur = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.tag.Equals("Player"))
        {
           //_lagTimeCur = 0;
            _isGroundedCur = false;
            
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        _lagTimeCur += Time.deltaTime;

        if (_lagTimeCur > _lagTime)
        {
            //Debug.Log("+"); 
            _lagTimeCur = 0;
            _isGrounded = _isGroundedCur;
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { get 
        {
            if (_pause)
            {
                return false;
            }

            return _controller.isGrounded;
        }
    }

    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private bool _pause;
    [SerializeField]
    private bool _isGrounded;

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

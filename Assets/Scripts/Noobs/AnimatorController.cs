using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GroundCheck _groundCheck;
    private float _horizontal, _vertical, _blend;

    void Start()
    {
        
    }

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    void Update()
    {
        if(_animator != null)
        {
            if (_groundCheck.IsGrounded)
            {
                _animator.SetBool("Fall", false);

                if (_horizontal == 0 && _vertical == 0)
                {
                    _animator.SetBool("Run", false);
                }
                else
                {
                    _animator.SetBool("Run", true);
                }
            }
            else
            {
                _animator.SetBool("Fall", true);
            }
        }

        _animator.SetFloat("Blend", _blend);

        if(_blend > 0.05f)
        {
            _blend -= 3 * Time.deltaTime;
        }

        if(_blend < -0.05f)
        {
            _blend += 3 * Time.deltaTime;
        }


        /*_blend = _blend > 0.1 ? _blend - Time.deltaTime : 0;
        _blend = _blend < -0.1 ? _blend + Time.deltaTime : 0;*/
    }

    public void SetBlend(float blend)
    {

        if (blend != 0)
        {
            if (blend > 0)
            {
                _blend += 10 * Time.deltaTime;

                if (_blend > 1)
                {
                    _blend = 1;
                }
            }
            else
            {
                _blend -= 10 * Time.deltaTime;

                if (_blend < -1)
                {
                    _blend = -1;
                }
            }
        }
    }

    public void SetRun(float horizontal, float vertical)
    {
        if (_animator != null)
        {
            _horizontal = horizontal;
            _vertical = vertical;
        }
    }

    public void SetFall()
    {

    }
    /*
    public void SetJump()
    {
        _animator.SetTrigger("Jump");
        _animator.SetBool("Fall", true);
    }
    */

}

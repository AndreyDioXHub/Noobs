using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected GroundCheck _groundCheck;
    protected float _horizontal, _vertical;

    public virtual void Start()
    {
        
    }

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    public virtual void Update()
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
    }

    public virtual void SetRun(float horizontal, float vertical)
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
    public virtual void SetWeapon()
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

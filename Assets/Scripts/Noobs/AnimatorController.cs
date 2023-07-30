using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GroundCheck _groundCheck;
    private float _horizontal, _vertical;

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

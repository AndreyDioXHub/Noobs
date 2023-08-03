using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoAnimationController : AnimatorController
{
    [SerializeField]
    private bool _weaponActivated;

    public override void Start()
    {
        
    }


    public override void Update()
    {
        if (_animator != null)
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


    public override void SetRun(float horizontal, float vertical)
    {
        base.SetRun(horizontal, vertical);

        _animator.SetFloat("RunX", horizontal);
        _animator.SetFloat("RunY", vertical);
    }

    public override void SetWeapon()
    {
        _weaponActivated = !_weaponActivated;

        float gun = _weaponActivated ? 1 : 0;

        _animator.SetFloat("Gun", gun);
    }
}

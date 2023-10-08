using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Transform _model;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GroundCheck _groundCheck;
    [SerializeField]
    private Transform _direction;
    [SerializeField]
    private Vector2 _axisLook;
    [SerializeField]
    private Vector2 _axisMove;
    [SerializeField]
    private Vector3 _prevEuler;
    [SerializeField]
    private float _blend, sencity = 0.1f;

    void Start()
    {

    }

    void Update()
    {
        if (_groundCheck.IsGrounded)
        {
            _animator.SetBool("Jump", false);
            if (_axisMove.magnitude > 0)
            {
                _animator.SetBool("Run", true);
            }
            else
            {
                _animator.SetBool("Run", false);
            }
        }
        else
        {
            _animator.SetBool("Jump", true);
        }
        Vector3 dir = _direction.position - transform.position;

        if (dir.magnitude > 0.2f)
        {
            _model.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        else
        {
            // _model.rotation = transform.rotation;
        }

        _animator.SetFloat("Blend", _blend);

        if (_blend > 0.05f)
        {
            _blend -= 5 * Time.deltaTime;
        }

        if (_blend < -0.05f)
        {
            _blend += 5 * Time.deltaTime;
        }

    }

    public void OnJump(bool jump)
    {
        _animator.SetBool("Jump", jump);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        //Read.
        _axisMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        float blend = context.ReadValue<Vector2>().x;
        //Debug.Log(blend);


        if (blend != 0)
        {
            if (blend > sencity)
            {
                _blend += blend * 10 * Time.deltaTime;

                if (_blend > 1)
                {
                    _blend = 1;
                }
            }

            if (blend < -sencity)
            {
                _blend -= -blend * 10 * Time.deltaTime;

                if (_blend < -1)
                {
                    _blend = -1;
                }
            }
        }

        /*
        //Read.
        _axisLook = context.ReadValue<Vector2>();
        float axis = _axisLook.x > 0 ? 1 : _axisLook.x;
        axis = _axisLook.x < 0 ? -1 : _axisLook.x;
        axis = _axisLook.x == 0 ? -1 : _axisLook.x;

        _animator.SetFloat("Blend", axis);
        //*/
    }
}

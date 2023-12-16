using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _models = new List<Transform>();
    [SerializeField]
    private List<Animator> _animators = new List<Animator>();
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

    public bool IsGrounded = true;

    public bool IsLocalPlayer = false;
    public float Blend { get => _blend; set { _blend = value; } }
    public Vector3 AxisMove = Vector3.zero;

    public event Action<Boolean> MovingStateChange = delegate { };
    void Start() 
    {

    }

    void Update()
    {
        #region OnlyLocalPlayer

        bool moving = false;

        if(IsLocalPlayer) 
        {
            bool prevstate = IsGrounded;
            IsGrounded = _groundCheck.IsGrounded;

            if(prevstate != IsGrounded) 
            {
                MovingStateChange?.Invoke(IsGrounded);
            }
            moving = _axisMove.sqrMagnitude > 0;
            AxisMove = _axisMove;
        } 
        else 
        {
            //Apply values from network data
            moving = AxisMove.sqrMagnitude > 0;
        }

        var activeAnimator = _animators.First(anim => anim.enabled && anim.gameObject.activeInHierarchy);

        if (IsGrounded)
        {
            activeAnimator.SetBool("Jump", false);
            activeAnimator.SetBool("Run", moving);
        }
        else
        {
            activeAnimator.SetBool("Jump", true);
        }

        activeAnimator.SetFloat("Blend", _blend);

        if (_blend > 0.05f)
        {
            _blend -= 5 * Time.deltaTime;
        }

        if (_blend < -0.05f)
        {
            _blend += 5 * Time.deltaTime;
        }

        #endregion

        #region Union

        Vector3 dir = _direction.position - transform.position;

        if(ChatTexts.IsActive && IsLocalPlayer)
        {

        }
        else
        {
            if (dir.magnitude > 0.2f)
            {
                foreach (var model in _models)
                {
                    model.rotation = Quaternion.LookRotation(dir, Vector3.up);
                }
            }
            else
            {
                // _model.rotation = transform.rotation;
            }
        }

        #endregion
    }

    public void OnJump(bool jump)
    {
        if (ChatTexts.IsActive && IsLocalPlayer)
        {
            return;
        }

        var activeAnimator = _animators.First(anim => anim.enabled && anim.gameObject.activeInHierarchy);
        activeAnimator.SetBool("Jump", jump);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (ChatTexts.IsActive && IsLocalPlayer)
        {
            _axisMove = Vector2.zero;
            return;
        }

        _axisMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (ChatTexts.IsActive && IsLocalPlayer)
        {
            _blend = 0;
            return;
        }

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

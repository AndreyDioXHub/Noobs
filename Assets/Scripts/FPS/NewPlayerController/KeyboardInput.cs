using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField]
    protected Movement _movement;
    [SerializeField]
    protected Transform _lookDirection;
    [SerializeField]
    protected Transform _model;
    [SerializeField]
    protected AnimatorController _animatorController;

    [SerializeField]
    protected AnimationCurve _accelerationCurve;
    [SerializeField]
    protected float _accelerationTime;
    [SerializeField]
    protected List<InteractableObject> _interactables = new List<InteractableObject>();

    protected float _accelerationVerticalForwardTimeCur;
    protected float _accelerationVerticalBackwardTimeCur;
    protected float _accelerationVerticalForwardDuration;
    protected float _accelerationVerticalBackwardDuration;

    protected float _accelerationHorizontalForwardTimeCur;
    protected float _accelerationHorizontalBackwardTimeCur;
    protected float _accelerationHorizontalForwardDuration;
    protected float _accelerationHorizontalBackwardDuration;

    public void SetMovement(Movement movement)
    {
        _movement = movement;
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _movement.Jump(true);
            //_animatorController.SetJump();
        }
        if (Input.GetKeyUp("space"))
        {
            _movement.Jump(false);
        }

        if (Input.GetKeyDown("left shift"))
        {
            _movement.Run(true);
        }

        if (Input.GetKeyUp("left shift"))
        {
            _movement.Run(false);
        }

        if (Input.GetKeyDown("left ctrl"))
        {
            _movement.Crouch(true);
        }

        if (Input.GetKeyUp("left ctrl"))
        {
            _movement.Crouch(false);
        }
    }

    public virtual void FixedUpdate()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey("w"))
        {
            if(_accelerationVerticalBackwardTimeCur == 0)
            {
                _accelerationVerticalForwardTimeCur += Time.deltaTime;
                _accelerationVerticalForwardTimeCur = _accelerationVerticalForwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationVerticalForwardTimeCur;
            }
        }
        else
        {
            _accelerationVerticalForwardTimeCur = _accelerationVerticalForwardTimeCur < 0 ? 0 : _accelerationVerticalForwardTimeCur - Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            if (_accelerationVerticalForwardTimeCur == 0)
            {
                _accelerationVerticalBackwardTimeCur = _accelerationVerticalBackwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationVerticalBackwardTimeCur + Time.deltaTime;
            }
        }
        else
        {
            _accelerationVerticalBackwardTimeCur = _accelerationVerticalBackwardTimeCur < 0 ? 0 : _accelerationVerticalBackwardTimeCur - Time.deltaTime;
        }
        _accelerationVerticalForwardDuration = _accelerationVerticalForwardTimeCur / _accelerationTime;
        _accelerationVerticalBackwardDuration = _accelerationVerticalBackwardTimeCur / _accelerationTime;

        vertical = _accelerationCurve.Evaluate(_accelerationVerticalForwardDuration) - _accelerationCurve.Evaluate(_accelerationVerticalBackwardDuration);
         

        if (Input.GetKey("d"))
        {
            if (_accelerationHorizontalBackwardTimeCur == 0)
            {
                _accelerationHorizontalForwardTimeCur += Time.deltaTime;
                _accelerationHorizontalForwardTimeCur = _accelerationHorizontalForwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationHorizontalForwardTimeCur;
            }
        }
        else
        {
            _accelerationHorizontalForwardTimeCur = _accelerationHorizontalForwardTimeCur < 0 ? 0 : _accelerationHorizontalForwardTimeCur - Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            if (_accelerationHorizontalForwardTimeCur == 0)
            {
                _accelerationHorizontalBackwardTimeCur = _accelerationHorizontalBackwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationHorizontalBackwardTimeCur + Time.deltaTime;
            }
        }
        else
        {
            _accelerationHorizontalBackwardTimeCur = _accelerationHorizontalBackwardTimeCur < 0 ? 0 : _accelerationHorizontalBackwardTimeCur - Time.deltaTime;
        }

        _accelerationHorizontalForwardDuration = _accelerationHorizontalForwardTimeCur / _accelerationTime;
        _accelerationHorizontalBackwardDuration = _accelerationHorizontalBackwardTimeCur / _accelerationTime;

        horizontal = _accelerationCurve.Evaluate(_accelerationHorizontalForwardDuration) - _accelerationCurve.Evaluate(_accelerationHorizontalBackwardDuration);

        if(horizontal !=0 || vertical != 0)
        {
            //Vector3 dir = new Vector3(horizontal, 0, vertical);
            _lookDirection.position = transform.position + transform.forward * vertical + transform.right * horizontal;
            _model.LookAt(_lookDirection, _model.up);//.rotation = Quaternion.LookRotation(_lookDirection.position - _model.position, _model.up);
        }

        _animatorController.SetRun(horizontal, vertical);

        _movement.Move(horizontal, vertical);
    }

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && e.type == EventType.KeyDown)
        {
            //Debug.Log("Detected key code: " + e.type);
            //Debug.Log("Detected key code: " + e.clickCount);
            //Debug.Log("Detected key code: " + e.keyCode);
            foreach (var interactable in _interactables)
            {
                interactable.Interact(e.keyCode.ToString());
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractableObject>(out InteractableObject interaction))
        {
            if (!_interactables.Contains(interaction))
            {
                _interactables.Add(interaction);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractableObject>(out InteractableObject interaction))
        {
            if (_interactables.Contains(interaction))
            {
                _interactables.Remove(interaction);
            }
        }
    }
}

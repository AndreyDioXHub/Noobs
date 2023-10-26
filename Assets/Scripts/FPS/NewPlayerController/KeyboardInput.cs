using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyboardInput : MonoBehaviour
{
    public UnityEvent OnEscDown = new UnityEvent();

    [SerializeField]
    protected Movement _movement;

    [SerializeField]
    protected AnimationCurve _accelerationCurve;
    [SerializeField]
    protected float _accelerationTime;
    private Vector2 _axisMove;
    /*[SerializeField]
    protected List<InteractableObject> _interactables = new List<InteractableObject>();*/

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

        if (Input.GetKeyDown("left shift"))
        {
            //_movement.Run(true);
        }

        if (Input.GetKeyUp("left shift"))
        {
            //_movement.Run(false);
        }

        if (Input.GetKeyDown("left ctrl"))
        {
            //_movement.Crouch(true);
        }

        if (Input.GetKeyUp("left ctrl"))
        {
            //_movement.Crouch(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Read.
        _axisMove = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Read.
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _movement.Jump(true);
                break;
            case InputActionPhase.Canceled:
                _movement.Jump(false);
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Waiting:
                break;
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {

        bool adsView = false;

        if (AdsButtonView.Instance != null)
        {
            adsView = AdsButtonView.Instance.Parent.activeSelf;
        }

        if (AdsScreen.Instance.gameObject.activeSelf || adsView || CheckPointManager.Instance.IsWin)// || BlockCountManager.Instance.BlocksCount == 0)
        {
            return;
        }

        //Read.
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OnEscDown?.Invoke();
                break;
            case InputActionPhase.Canceled:
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Waiting:
                break;
        }
    }

    public virtual void FixedUpdate()
    {
        bool adsView = false;

        if (AdsButtonView.Instance != null)
        {
            adsView = AdsButtonView.Instance.Parent.activeSelf;
        }

        if (SettingScreen.Instance.gameObject.activeSelf || AdsScreen.Instance.gameObject.activeSelf || adsView || CheckPointManager.Instance.IsWin)// || BlockCountManager.Instance.BlocksCount == 0)
        {
            return;
        }

        float horizontal = 0;
        float vertical = 0;

        if (_axisMove.y > 0)
        {
            if (_accelerationVerticalBackwardTimeCur == 0)
            {
                _accelerationVerticalForwardTimeCur += Time.deltaTime;
                _accelerationVerticalForwardTimeCur = _accelerationVerticalForwardTimeCur > _accelerationTime ? _accelerationTime : _accelerationVerticalForwardTimeCur;
            }
        }
        else
        {
            _accelerationVerticalForwardTimeCur = _accelerationVerticalForwardTimeCur < 0 ? 0 : _accelerationVerticalForwardTimeCur - Time.deltaTime;
        }

        if (_axisMove.y < 0)
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

        /*
        if (Input.GetKey("w"))
        {
            if (_accelerationVerticalBackwardTimeCur == 0)
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
        */
        _accelerationVerticalForwardDuration = _accelerationVerticalForwardTimeCur / _accelerationTime;
        _accelerationVerticalBackwardDuration = _accelerationVerticalBackwardTimeCur / _accelerationTime;

        vertical = _accelerationCurve.Evaluate(_accelerationVerticalForwardDuration) - _accelerationCurve.Evaluate(_accelerationVerticalBackwardDuration);

        if (_axisMove.x > 0)
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

        if (_axisMove.x < 0)
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

        /*

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
        */
        _accelerationHorizontalForwardDuration = _accelerationHorizontalForwardTimeCur / _accelerationTime;
        _accelerationHorizontalBackwardDuration = _accelerationHorizontalBackwardTimeCur / _accelerationTime;

        horizontal = _accelerationCurve.Evaluate(_accelerationHorizontalForwardDuration) - _accelerationCurve.Evaluate(_accelerationHorizontalBackwardDuration);

        _movement.Move(horizontal, vertical);
    }
    /*
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

    }*/
    /*
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
    }*/
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    public static CharacterStateController Instance;

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private KeyboardInput _keyboardInput;
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _groundCheckTransform;

    [SerializeField]
    private GroundCheck _groundCheck;
    private UnderWaterVolume _underWaterVolume { get => UnderWaterVolume.Instance; }

    [SerializeField]
    private bool _sendState = true;


    [SerializeField]
    private CharState _stateGroundStay;
    [SerializeField]
    private CharState _stateGroundCrouch;
    [SerializeField]
    private CharState _stateUnderWater;
    [SerializeField]
    private CharState _stateLadder;
    [SerializeField]
    private CharState _stateCannon;
    [SerializeField]
    private CharState _curentState;

    [SerializeField]
    private float _timeTransition = 0.5f;

    private float _timeDelay = 5;


    private void Awake()
    {
        //Set in PlayerNetworkBehaviour
        /*if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("CharacterStateController already exist");
            Destroy(this);
        }/**/
    }

    void Start()
    {
        _curentState = _stateGroundStay;
        ChangeStateCoroutine(State.stay);
    }

    void Update()
    {

        if (Time.timeSinceLevelLoad > _timeDelay)
        {
            bool playerIsUnderWaterPlus = _cameraTransform.position.y - 1 < _underWaterVolume.Cross.y;


            if (playerIsUnderWaterPlus && _sendState)
            {
                ChangeState(State.underwater);
                _sendState = false;
            }

            if (!playerIsUnderWaterPlus && !_sendState && _groundCheck.IsGrounded)
            {
                ChangeState(State.stay);
                //_sendState = true;
            }
        }
    }

    public void ChangeState(State state)
    {
        StopCoroutine(ChangeStateCoroutine(state));
        StartCoroutine(ChangeStateCoroutine(state));
    }

    IEnumerator ChangeStateCoroutine(State state)
    {
        _curentState.movement.enabled = false;
        _curentState.view.enabled = false;


        switch (state)
        {
            case State.stay:
                _curentState = _stateGroundStay;
                _sendState = true;
                break;
            case State.crouch:
                _curentState = _stateGroundCrouch;
                break;
            case State.underwater:
                _curentState = _stateUnderWater;
                break;
            case State.ladder:
                _curentState = _stateLadder;
                break;
            case State.cannon:
                _curentState = _stateCannon;
                break;
            default:
                _curentState = _stateGroundStay;
                break;
        }

        _curentState.movement.enabled = true;
        _curentState.view.enabled = true;

        _keyboardInput.SetMovement(_curentState.movement);
        _curentState.movement.Init();

        float charackterHeight = _characterController.height;
        float newCharackterHeight = _curentState.charackterHeight;

        Vector3 cameraPosition = _cameraTransform.localPosition;
        Vector3 newCameraPosition = _cameraTransform.localPosition;
        newCameraPosition.y = _curentState.cameraPosition;

        Vector3 groundCheckPosition = _groundCheckTransform.localPosition;
        Vector3 newGroundCheckPosition = _groundCheckTransform.localPosition;
        newGroundCheckPosition.y = _curentState.groundCheckPosition;

        float t = 0;
        float deltatime = 1 / (_timeTransition * 30);
        float frametime = 1 / 30;

        while (t < 1)
        {
            yield return new WaitForSeconds(frametime);
            _characterController.height = charackterHeight + (newCharackterHeight - charackterHeight) * t;
            _cameraTransform.localPosition = Vector3.Lerp(cameraPosition, newCameraPosition, t);
            _groundCheckTransform.localPosition = Vector3.Lerp(groundCheckPosition, newGroundCheckPosition, t);
            t += deltatime;
        }

        _cameraTransform.localPosition = newCameraPosition;
        _characterController.height = newCharackterHeight;
        _groundCheckTransform.localPosition = newGroundCheckPosition;

        yield return null;
    }
}

[Serializable]
public class CharState
{
    public Movement movement;
    public CameraView view;
    public float charackterHeight;
    public float cameraPosition;
    public float groundCheckPosition;
}

public enum State
{
    stay,
    crouch,
    underwater,
    ladder,
    cannon
}

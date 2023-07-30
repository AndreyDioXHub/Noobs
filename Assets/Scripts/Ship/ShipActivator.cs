using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using cyraxchel.inputs;
using UnityEngine.InputSystem;

public class ShipActivator : MonoBehaviour
{
    CharacterController _controller = null;


    public StarterAssetsCustom _shipController;
    public InputAction directAction;
    Action MoveAction;
    Action MoveFixedAction;
    [SerializeField, Header("Test test")]
    Transform helm;
    [SerializeField]
    ShipMove ship;
    
    [SerializeField]
    string _activationText = "Press <b><color=red>E</color></b> for activate ship drive";
    [SerializeField]
    string _deactivationText = "Press<b><color=red>Q</color></b> for release ship drive";

    public UnityEvent<string> OnStateChange;
    [SerializeField]
    Vector2 _directionWADS;

    PlayerInput pinput;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction = EmptyMove;
        MoveFixedAction = EmptyMove;
        pinput = GetComponent<PlayerInput>();
        /*var inp = new StarterAssetsCustom();
        inp.ShipControl.Signal.performed += OnSignal;
        inp.ShipControl.StartStopControls.performed += OnActivateTriggerE;
        inp.ShipControl.Move.performed += WASDCALLED;
        _shipController = inp;/**/
        //inp.Enable();
        //inp.ShipControl.Enable();
        //inp.ShipControl.Move.Enable();
        //directAction.Enable();
    }

    /*private void OnWASD(InputAction.CallbackContext _callback)
    {
        Debug.Log("WASD CALL");
        _directionWADS = _shipController.ShipControl.WASD.ReadValue<Vector2>();
    }/**/

    private void OnActivateTriggerE(InputAction.CallbackContext obj) {
        Debug.Log("E activated");
        if(_controller != null) {
            _controller.enabled = false;
            MoveAction = MoveShip;
            MoveFixedAction = MoveShipFixed;
            OnStateChange.Invoke(_deactivationText);
        }
    }

    private void OnSignal(InputAction.CallbackContext obj) {
        Debug.Log("SIGNAL performed");
        if(_controller != null) {
            //Release controller
            _controller.enabled = true;
            _controller = null;
            MoveAction = EmptyMove;
            MoveFixedAction = EmptyMove;
        }
    }

    private void EmptyMove() {
        

    }

    private void OnDestroy() {
        if(pinput != null) pinput.actions = null;
    }

    private void MoveShip() {
        //Move helm
        //_directionWADS = _shipController.ShipControl.WASD.ReadValue<Vector2>();
        //_directionWADS = directAction.ReadValue<Vector2>();
        helm.Rotate(Vector3.up, _directionWADS.x, Space.Self);
        ship.SetKeelRotationSingle(_directionWADS.x);
        ship.SetSailsRotationSingle(_directionWADS.y);
    }

    // Update is called once per frame
    void Update() {
        MoveAction();
    }

    private void FixedUpdate() {
        MoveFixedAction();
    }
    void MoveShipFixed () { 
        //Vector2 _direction = _shipController.ShipControl.WASD.ReadValue<Vector2>();
        //Debug.Log(_direction.x);
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Collision with {other.gameObject}");
        if (other.gameObject.tag == "Player") {
            _controller = other.gameObject.GetComponent<CharacterController>();
            OnStateChange.Invoke(_activationText);
            //PlayerInput _cinput = other.gameObject.GetComponent<PlayerInput>();
            //_cinput.defaultActionMap = "ShipControl";
            //_cinput.onActionTriggered += ListenCommand;
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log($"Collision OFF with {other.gameObject}");
        if (other.gameObject.tag == "Player") {
            _controller = null;
            OnStateChange.Invoke("");
        }
    }

    /*private void ListenCommand(InputAction.CallbackContext obj) {
        Debug.Log(obj.action.name);
    }/**/

    public void OnActivateTrigger() {
        Debug.Log("Try call E event");
        if(_controller != null) {
            //Listen E press
            Debug.Log("On Button activation called");
            //Deactivate controller for FPS
        }


    }

    public void EnableSingleControling() {
        MoveAction = MoveShip;
        //MoveFixedAction = MoveShipFixed;
    }


    public void WASDCALLED(InputAction.CallbackContext context)
    {
        Debug.Log("WASD2 CALLED");
        _directionWADS = context.ReadValue<Vector2>();
        
    }
}

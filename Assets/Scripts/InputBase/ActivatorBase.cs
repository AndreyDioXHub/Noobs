using cyraxchel.inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace cyraxchel.inputs {
    public class ActivatorBase : MonoBehaviour {
        [SerializeField]
        protected string _activationText = "Press <b><color=red>E</color></b> for activate ship drive";
        [SerializeField]
        protected string _deactivationText = "Press<b><color=red>Q</color></b> for release ship drive";
        [SerializeField]
        bool EnableInputOnStart = false;
        [SerializeField]
        protected string ColliderTag = "Player";
        [SerializeField]
        protected GameObject VirtualCamera;
        [SerializeField, Tooltip("If filled, set playercapsule to it position")]
        GameObject PositionOnGrab;

        protected StarterAssetsCustom _inpController;
        protected CharacterController _controller = null;
        protected PlayerInput _pinput;
        protected StarterAssets.FirstPersonController _fps;

        protected Action UpdateAction;
        protected Action FixedUpdateAction;


        public UnityEvent<string> OnStateChange;
        // Start is called before the first frame update
        void Start() {
            Debug.Log("ActivatorBase START");
            UpdateAction = EmptyMove;
            FixedUpdateAction = EmptyMove;
            if(VirtualCamera != null) VirtualCamera.SetActive(false);
            OnStart();
            _inpController = new StarterAssetsCustom();
            SubscribeInputssOnStart();
            if (EnableInputOnStart) {
                EnableInputController(true);
            }
        }

        private void Update() {
            UpdateAction();
        }
        private void FixedUpdate() {
            FixedUpdateAction();
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log($"Collision with {other.gameObject}");
            if (other.gameObject.tag == ColliderTag) {
                _controller = other.gameObject.GetComponent<CharacterController>();
                _pinput = other.gameObject.GetComponent<PlayerInput>();
                _fps = other.gameObject.GetComponent<StarterAssets.FirstPersonController>();
                EnableInputController(true);    //Start listen custom controller
                OnStateChange.Invoke(_activationText);
            }
        }
        private void OnTriggerExit(Collider other) {
            Debug.Log($"Collision OFF with {other.gameObject}");
            if (other.gameObject.tag == "Player") {
                _controller = null;
                EnableInputController(false);   //Disable input
                OnStateChange.Invoke("");
            }
        }

        protected void GrabPlayer() {
            if(_controller != null) {
                // _controller.enabled = false;
                if (PositionOnGrab != null) {
                    _fps.enabled = false;
                    _controller.enabled = false;
                    _pinput.transform.position = PositionOnGrab.transform.position;
                }
                _pinput.enabled = false;
                if (VirtualCamera != null) VirtualCamera.SetActive(true);
                
               // _fps.enabled = false;
                UpdateAction = OnUpdateAction;
                FixedUpdateAction = OnFixedUpdateAction;
                OnStateChange.Invoke(_deactivationText);
            }
        }

        protected void ReleasePlayer() {
            if(_controller != null) {
                _pinput.enabled = true;
                if (VirtualCamera != null) VirtualCamera.SetActive(false);
                _fps.enabled = true;
                _controller.enabled = true;
                OnTriggerExit(_controller.gameObject.GetComponent<Collider>());
                UpdateAction = EmptyMove;
                FixedUpdateAction = EmptyMove;
                _controller = null;
            }
        }

        protected void SwitchPlayerInput() {
            if(_pinput.enabled) {
                GrabPlayer();
            } else {
                ReleasePlayer();
            }
        }

        protected void EmptyMove() { }

        #region OVERRIDES
        /// <summary>
        /// Set InputController state
        /// </summary>
        /// <param name="isEnable"></param>
        protected virtual void EnableInputController(bool isEnable) {
            if (isEnable) {
                _inpController.Enable();
            } else {
                _inpController.Disable();
            }
        }

        protected virtual void OnFixedUpdateAction() {
            //Override me
        }
        protected virtual void OnUpdateAction() {
            //Override me
        }

        protected virtual void SubscribeInputssOnStart() {
            //Override me
            throw new NotImplementedException("Implement this method in your child class!");
        }

        protected virtual void OnStart() {
            //Override me
        }
        #endregion
    }
}
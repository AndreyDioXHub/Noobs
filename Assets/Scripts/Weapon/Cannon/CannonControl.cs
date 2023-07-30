using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cyraxchel.inputs;
using UnityEngine.InputSystem;
using System;

namespace cyraxchel.gun {

    public class CannonControl : ActivatorBase {

        [Header("Barrel")]
        [SerializeField]
        Transform _barrel;
        [SerializeField]
        Vector2 VerticalAngles;
        [SerializeField]
        Vector2 HorisontalAngle;
        [SerializeField]
        GameObject cannonball;
        [SerializeField]
        float GunForce = 5;
        [SerializeField]
        ParticleSystem Smoke;
        [SerializeField]
        GameObject ExplosionTemplate;

        Vector3 _startBarrelRotation;
        [SerializeField]
        float _speed = 0.1f;

        [Header("Extra")]
        [SerializeField]
        bool allowMunchgausen = false;
        

        private const float _threshold = 0.01f;

        protected override void OnStart() {
            Debug.Log("Barrel start");
            base.OnStart();
            _startBarrelRotation = _barrel.localEulerAngles;
            /**HorisontalAngle.x += _startBarrelRotation.x;
            HorisontalAngle.y += _startBarrelRotation.x;
            VerticalAngles.y += _startBarrelRotation.y;
            VerticalAngles.y += _startBarrelRotation.y;**/
        }

        protected override void SubscribeInputssOnStart() {
            _inpController.CannonControl.StartStopControls.performed += _ => { SwitchPlayerInput(); };
            _inpController.CannonControl.Fire.performed += CannonFire;
            _inpController.CannonControl.FireMunchgausen.started += MunchgFire;
            _inpController.CannonControl.FireMunchgausen.performed += MunchgFirePerform;
        }


        private void CannonFire(InputAction.CallbackContext obj) {
            Fire();
        }

        public void Fire() {
            GameObject cball = Instantiate(cannonball);
            cball.transform.position = _barrel.position + _barrel.forward * 0.5f;
            Bullet _bullet = cball.GetComponent<Bullet>();
            if(_bullet != null)
            {
                _bullet.OnCollided += ShowExplosion;
                _bullet.InitBullet(GunForce, _barrel.forward);
            } else
            {
                var rbody = cball.GetComponent<Rigidbody>();
                rbody.AddForce(GunForce * _barrel.forward);
            }
            Smoke.Play();
        }

        private void ShowExplosion(Transform _bullet, Transform _target) {
            GameObject _explosion = Instantiate(ExplosionTemplate);
            _explosion.transform.position = _bullet.position;
        }

        private float _cinemachineTargetPitch;
        private float _rotationVelocity;
        protected override void OnUpdateAction() {
            //Move barrel by mouse
            Vector2 _mouse = _inpController.CannonControl.Look.ReadValue<Vector2>();
            if (_mouse.sqrMagnitude >= _threshold) {
                //Don't multiply mouse input by Time.deltaTime
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += _mouse.y * _speed * deltaTimeMultiplier;
                _rotationVelocity += _mouse.x * _speed * deltaTimeMultiplier;
                _rotationVelocity = ClampAngle2(_rotationVelocity, HorisontalAngle.x, HorisontalAngle.y);

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle2(_cinemachineTargetPitch, VerticalAngles.x, VerticalAngles.y);

                // Update Cinemachine camera target pitch
                _barrel.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch,_rotationVelocity, 0.0f);

            }
        }

        private float ClampAngle(float angle, float from, float to) {
            // accepts e.g. -80, 80
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
        }

        private float ClampAngle2(float lfAngle, float lfMin, float lfMax) {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private bool IsCurrentDeviceMouse {
            get {
                #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return true;
                #else
                return false;
                #endif
            }
        }

        #region Munchgausen fire

        float munchg_preparetime = 3.0f;
        float munchg_time = 0;
        [SerializeField]
        float munchg_force = 40;

        private void MunchgFire(InputAction.CallbackContext _context) {
            Debug.Log("MUNCHGAUSEN LOADING!");
            munchg_time = Time.time;
            //TODO Start show prepare
            if(VirtualCamera != null) {
                VirtualCamera.SetActive(false);
            }
        }
        private void MunchgFirePerform(InputAction.CallbackContext obj) {
            //Debug.Log("MUNCHGAUSEN TRY FIRE!");
            if ((Time.time - munchg_time) > munchg_preparetime && _controller != null) {
                //TODO Fire
                Debug.Log("MUNCHGAUSEN FIRE!");
                _controller.enabled = true;
                _controller.Move(_barrel.forward * munchg_force);
                ReleasePlayer();
                
            } else {
                //TODO Reverse load animation
            }
        }

        
        #endregion
    }
}
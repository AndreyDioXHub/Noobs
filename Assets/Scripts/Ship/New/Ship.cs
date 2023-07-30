using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace cyraxchel.ship
{
    public class Ship : MonoBehaviour
    {
        #region Editor Properties
        [Header("Характеристики")]
        [SerializeField, Tooltip("Максимальная скорость, км/ч")]
        float maxSpeed = 20;
        [SerializeField, Tooltip("Текущая скорость. Если задать не ноль, то корабль будет сразу плыть.")]
        float currentSpeed = 0;
        [SerializeField, Range(-1, 0), Tooltip("Инертность корабля (до -1)")]
        float inert = -0.2f;
        [SerializeField, Tooltip("Максимальный угол поворота корабля")]
        float MaxAngle = 30;    //maxAngleRotation
        [SerializeField, Tooltip("Kell range rotation")]
        Vector2 MaxKeelRotation = new Vector2(-40, 40);     //maxKeelRotation
        [SerializeField, Range(0, 1), Tooltip("Воздействие якоря")]
        float anchor = 0;
        [Tooltip("We can stop moving for debug")]
        public bool CanMove = true;

        [Header("Sub elements")]
        [SerializeField]
        Sails sailsController;
        [SerializeField]
        Keel keelController;
        #endregion

        #region Private fields
        [SerializeField]
        float acceleration = 0;
        Vector3 totalSailDirection = Vector3.zero;  //Direction of all sails. Will calculated in own script SailsController.cs
        Vector3 wind;   //Current Wind. Will be read from WorldController
        float keelRotation = 0;
        
        //Helpers
        float currentAdditionalAngle = 0;
        float prevRotation = 0;
        float crossWindInSails;
        float aspeed = 0;
        //Anhors Properties
        float minAnchorForce = 0.1f;
        #endregion

        #region GameObjects
        public Transform Keel;
        public Transform ShipBody;
        #endregion



        // Start is called before the first frame update
        void Awake()
        {
            if(ShipBody == null) {
                ShipBody = transform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            GetRotationByKeel();
        }

        void FixedUpdate() {
            Vector3 fwrd = ShipBody.forward;
            acceleration = GetAcceleration();
            currentSpeed += (acceleration + inert) * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            Vector3 dist = fwrd * (currentSpeed * Time.deltaTime);
            Vector3 pos = ShipBody.position + dist;
            if (CanMove) ShipBody.position = pos;
        }

        #region Public methods

        /// <summary>
        /// Установка поворота киля.
        /// Допустимые пределы от -40 до 40 градусов.
        /// </summary>
        /// <param name="angle"></param>
        public void SetKeelRotation(float angle) {
            Vector3 eangle = Keel.transform.localEulerAngles;
            eangle.y = Sails.NormilizeAndClapmAngle(angle, MaxKeelRotation);
            Keel.transform.localEulerAngles = eangle;
        }

        public void SetKeelRotationSingle(float value) {
            value = value * -1;
            float _range = Mathf.Clamp(value, -1, 1) + Keel.transform.localEulerAngles.y;
            Debug.Log($"value = {value}   range = {_range}");
            SetKeelRotation(_range);
        }

        #endregion


        #region Calculating

        private void GetRotationByKeel() {
            float _speedD = currentSpeed / maxSpeed;
            keelRotation = 1 - Vector3.Dot(Keel.transform.forward, ShipBody.forward);
            keelRotation *= Math.Sign(Vector3.Dot(Keel.transform.forward, ShipBody.right));
            currentAdditionalAngle = keelRotation * MaxAngle * Time.deltaTime;
            Quaternion rot = ShipBody.rotation;
            Vector3 nrot = ShipBody.rotation.eulerAngles;
            prevRotation = nrot.y;
            nrot.y -= currentAdditionalAngle * _speedD / Mathf.Clamp((1 - anchor), minAnchorForce, 1);
            rot.eulerAngles = nrot;
            ShipBody.rotation = rot;
            //Calculate angle speed (simple)
            float drot = prevRotation - nrot.y;
            if (Math.Abs(drot) > 0.01f) {   //TODO Set correct threshold
                //Расчет поворота с креном
                aspeed = drot * 180 * -1;// 90 - Math.Abs(drot);
                Vector3 _shipWrot = ShipBody.localEulerAngles;
                Quaternion newRotQ = Quaternion.AngleAxis(aspeed, Vector3.forward);
                //            _shipCarkas.localRotation = Quaternion.AngleAxis(_aspeed, Vector3.forward);
                ShipBody.localRotation = Quaternion.Lerp(ShipBody.localRotation, newRotQ, Time.deltaTime);
                _shipWrot.z = aspeed;
                //TODO Require normilize localEulerAngles
                _shipWrot = Vector3.Lerp(ShipBody.localEulerAngles, _shipWrot, Time.deltaTime);
                //Or use Quaternion
                //_shipCarkas.localEulerAngles = _shipWrot;
            } else {
                //Просто поворот без крена
                ShipBody.localRotation = Quaternion.Lerp(ShipBody.localRotation, Quaternion.identity, Time.deltaTime);
                //_shipCarkas.localEulerAngles = Vector3.Lerp(_shipCarkas.localEulerAngles, Vector3.zero, Time.deltaTime);
            }
        }

        /// <summary>
        /// Расчет ускорения движения корабля.
        /// Учитываю раскрытость парусов и общую сонаправленность парусов
        /// </summary>
        /// <see cref="Sails"/>
        /// <returns></returns>
        private float GetAcceleration()
        {
            return sailsController.GetSailsPower(WindGlobal.WindV3, 1);

        }

        #endregion
    }
}
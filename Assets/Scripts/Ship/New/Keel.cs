using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace cyraxchel.ship {
    public class Keel : MonoBehaviour {

        [SerializeField]
        Transform keelTransform;

        [SerializeField, Tooltip("Kell range rotation")]
        Vector2 MaxKeelRotation = new Vector2(-40, 40);     //maxKeelRotation

        // Start is called before the first frame update
        void Awake() {
            SetKeelRotation(0);
        }


        #region Public methods

        /// <summary>
        /// Установка поворота киля.
        /// Допустимые пределы от -40 до 40 градусов.
        /// </summary>
        /// <param name="angle"></param>
        public void SetKeelRotation(float angle) {
            Vector3 eangle = keelTransform.localEulerAngles;
            eangle.y = Sails.NormilizeAndClapmAngle(angle, MaxKeelRotation);
            keelTransform.localEulerAngles = eangle;
        }

        public void SetKeelRotationSingle(float value) {
            value = value * -1;
            float _range = Mathf.Clamp(value, -1, 1) + keelTransform.localEulerAngles.y;
            Debug.Log($"value = {value}   range = {_range}");
            SetKeelRotation(_range);
        }

        public Vector3 GetKeelRotation() {
            return keelTransform.forward;
        }

        #endregion
    }
}
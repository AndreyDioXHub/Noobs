using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace cyraxchel.ship
{
    public class Sails : MonoBehaviour
    {
        #region Events

        event Action<float> OnTotalWeightReady = delegate { };

        #endregion

        #region Editor fields
        [SerializeField]
        List<Sail> SailsList;

        [SerializeField, Tooltip("Режим управления парусами")]
        SailControlType sailsMode = SailControlType.Union;

        [SerializeField]
        Transform shipBody;

        #endregion

        

        #region Monobehaviour callbacks

        // Start is called before the first frame update
        void Start() {
            InitSails();
        }

        /// <summary>
        /// Пересчет весов каждого паруса.
        /// </summary>
        private void InitSails() {
            float totalWeights = 0f;
            foreach (var item in SailsList) {
                totalWeights += item.sailWeight;
                OnTotalWeightReady += item.NormilizeWeights;
            }
            if(totalWeights > 0) {
                OnTotalWeightReady?.Invoke(totalWeights);
            }
        }

        // Update is called once per frame
        void Update() {

        }
        #endregion

        #region Public methods

        public float GetSailsPower(Vector3 windDirection, float windPower = 1.0f) {
            
            float _power = 0;
            foreach (var item in SailsList) {
                _power += item.GetThrustFairWind(windDirection, shipBody);
            }
            float totalPower = _power * windPower;

            return totalPower;
        }


        /// <summary>
        /// Normilize And clamp angle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static float NormilizeAndClapmAngle(float angle, Vector2 range)
        {
            if (angle > 180 && angle <= 360)
            {
                angle = angle - 360;
            }
            angle = Mathf.Clamp(angle, range.x, range.y);
            //Debug.Log($"clapm angle = {angle}");
            return angle;
        }

        #endregion

        public enum SailControlType
        {
            Union,
            Separate
        }
    }
}
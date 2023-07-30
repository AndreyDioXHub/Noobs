using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyraxchel.ship {
    /// <summary>
    /// Направление ветра. Тут надо реализовать различное поведение ветра.
    /// </summary>
    public class WindGlobal : MonoBehaviour {

        /// <summary>
        /// Wind in Vector2 format
        /// </summary>
        public static Vector2 Wind { get; private set; } = Vector3.zero;

        /// <summary>
        /// Wind in Vecrtor3 format (used X and Z values)
        /// </summary>
        public static Vector3 WindV3 { get {
                Vector3 _w = new Vector3(Wind.x, 0, Wind.y);
                return _w;
            } }

        Func<Vector2> CalcFunction;
        [SerializeField]
        Vector2 _wind;
        
        void Awake() {
            CalcFunction = DefaultWindCalculation;
            UpdateWind();
        }

        /// <summary>
        /// Default wind direction generation
        /// </summary>
        /// <returns></returns>
        private Vector2 DefaultWindCalculation() {
            //Generate random
            return UnityEngine.Random.insideUnitCircle;
        }


        #region Public methods

        public void UpdateWind() {
            Wind = CalcFunction();
        }

        public void SetWindFunction(Func<Vector2> _func) {
            CalcFunction = _func;
        }

        [ContextMenu("Set Wind")]
        public void SetWindFromEditorVector() {
            Wind = _wind;
        }

        public void SetWindFromValues(float xw, float yw) {
            Debug.Log($"Update wind to {xw}, {yw}");
            Wind = new Vector2(xw, yw);
            _wind = Wind;
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyraxchel.ship {
    [Serializable]
    public class Sail {
        #region Events

        public event Action OnSailWeightChange = delegate { };
        public event Action<float> OnSailAreaChange = delegate { };

        #endregion

        public string Name;

        [SerializeField, Tooltip("Вьюха для отображения состояния паруса")]
        internal GameObject sailView;

        [SerializeField, Tooltip("Вес паруса в общей системе")]
        internal float sailWeight = 1;
        internal float sailWeightNormilized { get; set; } = 0f;

        [SerializeField, Range(0.0f, 1.0f), Tooltip("Уроверь раскрытия паруса")]
        float _sailArea = 1;
        internal float sailAreaValue { get=>_sailArea; private set =>_sailArea=value; }

        [SerializeField, Tooltip("Максимальный поворот паруса")]
        Vector2 maxSailRotation = new Vector2(-80, 80);

        //Текущая тяга паруса. Меняется в зависимости от раскрытия паруса и его поворота относительно ветра
        float thrustK = 1f;

        #region Public Methods 

        /// <summary>
        /// Нормализовать вес паруса
        /// </summary>
        /// <param name="totalWights">Общая сумма весов всех парусов</param>
        public void NormilizeWeights(float totalWights) {
            sailWeightNormilized = sailWeight/totalWights;
        }

        /// <summary>
        /// Возвращает тягу паруса при попутном ветре (1).
        /// </summary>
        /// <param name="fairWindK">Коэффициент попутности ветра</param>
        /// <returns></returns>
        public float GetThrustFairWind(float fairWindK = 1) {
            thrustK = sailAreaValue * sailWeightNormilized * fairWindK;
            return thrustK;
        }

        /// <summary>
        /// Тяга паруса при указанном ветре.
        /// </summary>
        /// <param name="wind">Vector3 направления ветра.</param>
        /// <returns></returns>
        public float GetThrustFairWind(Vector3 wind, Transform shipBody) {
            if(sailView == null) return 0f;
            float fairWindK = Vector3.Dot(wind, sailView.transform.forward);    //Это относительно ветра.
            if(shipBody != null) {
                float bodySailK = Math.Clamp(Vector3.Dot(sailView.transform.forward, shipBody.forward), -1,1);  //Относительно тела корабля
                fairWindK *= bodySailK;
            }
            return GetThrustFairWind(fairWindK);
        }

        /// <summary>
        /// Установить новое значение раскрытия паруса
        /// </summary>
        /// <param name="value">Значение принудительно приводится к диапазону от 0 до 1</param>
        public void SetSailArea(float value) {
            sailAreaValue = Mathf.Clamp01(value);
            OnSailAreaChange?.Invoke(sailAreaValue);
        }

        /// <summary>
        /// Повернуть мачту на нужный угол
        /// </summary>
        /// <param name="angle">Угол планируемого поворота</param>
        public void SetSailRotation(float angle) {
            if(sailView == null) {
                return;
            }
            angle = Sails.NormilizeAndClapmAngle(angle, maxSailRotation);
            Vector3 eangle = sailView.transform.localEulerAngles;
            eangle.y = angle;
            Debug.Log(eangle.y);
            sailView.transform.localEulerAngles = eangle;
        }

        #endregion
    }
}
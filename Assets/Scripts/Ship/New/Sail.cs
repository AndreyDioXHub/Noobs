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

        [SerializeField, Tooltip("����� ��� ����������� ��������� ������")]
        internal GameObject sailView;

        [SerializeField, Tooltip("��� ������ � ����� �������")]
        internal float sailWeight = 1;
        internal float sailWeightNormilized { get; set; } = 0f;

        [SerializeField, Range(0.0f, 1.0f), Tooltip("������� ��������� ������")]
        float _sailArea = 1;
        internal float sailAreaValue { get=>_sailArea; private set =>_sailArea=value; }

        [SerializeField, Tooltip("������������ ������� ������")]
        Vector2 maxSailRotation = new Vector2(-80, 80);

        //������� ���� ������. �������� � ����������� �� ��������� ������ � ��� �������� ������������ �����
        float thrustK = 1f;

        #region Public Methods 

        /// <summary>
        /// ������������� ��� ������
        /// </summary>
        /// <param name="totalWights">����� ����� ����� ���� �������</param>
        public void NormilizeWeights(float totalWights) {
            sailWeightNormilized = sailWeight/totalWights;
        }

        /// <summary>
        /// ���������� ���� ������ ��� �������� ����� (1).
        /// </summary>
        /// <param name="fairWindK">����������� ���������� �����</param>
        /// <returns></returns>
        public float GetThrustFairWind(float fairWindK = 1) {
            thrustK = sailAreaValue * sailWeightNormilized * fairWindK;
            return thrustK;
        }

        /// <summary>
        /// ���� ������ ��� ��������� �����.
        /// </summary>
        /// <param name="wind">Vector3 ����������� �����.</param>
        /// <returns></returns>
        public float GetThrustFairWind(Vector3 wind, Transform shipBody) {
            if(sailView == null) return 0f;
            float fairWindK = Vector3.Dot(wind, sailView.transform.forward);    //��� ������������ �����.
            if(shipBody != null) {
                float bodySailK = Math.Clamp(Vector3.Dot(sailView.transform.forward, shipBody.forward), -1,1);  //������������ ���� �������
                fairWindK *= bodySailK;
            }
            return GetThrustFairWind(fairWindK);
        }

        /// <summary>
        /// ���������� ����� �������� ��������� ������
        /// </summary>
        /// <param name="value">�������� ������������� ���������� � ��������� �� 0 �� 1</param>
        public void SetSailArea(float value) {
            sailAreaValue = Mathf.Clamp01(value);
            OnSailAreaChange?.Invoke(sailAreaValue);
        }

        /// <summary>
        /// ��������� ����� �� ������ ����
        /// </summary>
        /// <param name="angle">���� ������������ ��������</param>
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
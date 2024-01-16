using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyraxchel.ai.bots {
    public class VectorBotView : MonoBehaviour, IBotView {

        [SerializeField]
        float AngleView = 180;

        [SerializeField]
        float attackRange = 10;


        [SerializeField]
        float sightRange = 50;

        [SerializeField, Tooltip("Время, чтобы забыть преследование, если игрок выпал из обзора")]
        float forgetTime = 3;
        float curentTime = 0;

        float _sightdistp2;

        [SerializeField]
        Transform head;


        #region Debug
        [SerializeField]
        float d_AngleViewCalc;

        [SerializeField]
        bool d_PlayerSighted = false;
        #endregion
        public bool CanAttackPlayer(Vector3 botPosition, float attackRange, LayerMask whatIsPlayer) {
            return false;
        }

        public bool CanAttackPlayer() {
            return false;
        }

        public bool PlayerInSightView(Vector3 botPosition, float sightRange, LayerMask whatIsPlayer) {
            return false;
        }

        public bool PlayerInSightView() {
            // Пока только на одного игрока
            Transform playTransfirm = PlayersAI.Players[0].transform;

            Vector3 dist = playTransfirm.position - head.position;
            if (dist.sqrMagnitude < _sightdistp2) {
                dist = dist.normalized;
                float dotv = Vector3.Dot(head.forward, dist);
                float _angle = Mathf.Acos(dotv) * Mathf.Rad2Deg;
                d_AngleViewCalc = _angle;
                d_PlayerSighted = _angle <= AngleView;
                bool insight = _angle <= AngleView;
                if(insight) {
                    curentTime = forgetTime;
                } else {
                    if(curentTime > 0) {
                        insight = true;
                    }
                }
                return insight;
            }

            return false;
        }


        // Start is called before the first frame update
        void Awake() {
            _sightdistp2 = sightRange * sightRange;
        }

        // Update is called once per frame
        void Update() {
            if (curentTime > 0) {
                curentTime -= Time.deltaTime;
            }
        }

        private void OnDrawGizmos() {
            if(PlayersAI.Players == null) { return; }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(head.position, head.forward*sightRange + head.position);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(head.position, PlayersAI.Players[0].transform.position);

            Gizmos.color = Color.blue;

            Vector3 angleaxis = Quaternion.AngleAxis(AngleView * 0.5f, Vector3.up) * head.forward;

            Gizmos.DrawLine(head.position, angleaxis * sightRange + head.position);

            angleaxis = Quaternion.AngleAxis(AngleView * -0.5f, Vector3.up) * head.forward;
            Gizmos.DrawLine(head.position, angleaxis * sightRange + head.position);

        }
    }
}
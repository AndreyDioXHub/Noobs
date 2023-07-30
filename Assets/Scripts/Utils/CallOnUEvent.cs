using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace utils {

    public class CallOnUEvent : MonoBehaviour {
        public UnityEvent callOnEnable;
        public UnityEvent callOnDisable;
        public UnityEvent OnStart;

        public UnityEvent OnDelayEnable;
        public UnityEvent OnDelayStart;
        public UnityEvent OnDestroyObject;


        public float delaySeconds = 1.0f;

        bool inited = false;

        public void OnEnable() {
            callOnEnable?.Invoke();
            StartCoroutine(OnEnableDelay());
        }
        private IEnumerator OnEnableDelay() {
            yield return new WaitForSeconds(delaySeconds);
            Debug.Log("delay OnEnable event");
            OnDelayEnable?.Invoke();
        }

        public void OnDisable() {

            callOnDisable.Invoke();
        }

        public void Start() {
            inited = true;
            OnStart?.Invoke();
            StartCoroutine(StartDelay());
        }

        private IEnumerator StartDelay() {
            yield return new WaitForSeconds(delaySeconds);
            Debug.Log("delay start event");
            OnDelayStart?.Invoke();
        }

        private void OnDestroy() {
            OnDestroyObject?.Invoke();
        }
    }
}
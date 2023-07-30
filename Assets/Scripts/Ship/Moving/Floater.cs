using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigiBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    private void FixedUpdate() {
        if (transform.position.y < 0f) {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rigiBody.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFly : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    public event Action OnDestroyObject = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        speed = WorldValues.Instance.WindSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 baseDirection = WorldValues.Instance.Wind * speed * Time.deltaTime;
        //transform.LookAt(baseDirection + transform.position);
        Vector3 sinAdd = Vector3.right * Mathf.Sign(Time.deltaTime * speed);
        transform.Translate(baseDirection + sinAdd);
        transform.Translate(sinAdd, Space.Self);
    }

    private void OnDestroy() {
        OnDestroyObject?.Invoke();
    }
}

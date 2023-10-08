using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelDirection : MonoBehaviour
{
    private float _transitionTime = 0.5f, _transitionTimeCur;
    private Vector3 _axisMove = new Vector3(), _axisMovePrev = new Vector3();

    public void OnMove(InputAction.CallbackContext context)
    {
        _axisMovePrev = transform.localPosition;
        Vector2 val = context.ReadValue<Vector2>();
        _axisMove = new Vector3(val.x, 0, val.y); 
        _transitionTimeCur = 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _transitionTimeCur += Time.deltaTime;
        _transitionTimeCur = _transitionTimeCur > _transitionTime ? _transitionTime : _transitionTimeCur;

        transform.localPosition = Vector3.Lerp(_axisMovePrev, _axisMove, _transitionTimeCur / _transitionTime);
    }
}

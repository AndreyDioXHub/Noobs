using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelDirection : MonoBehaviour
{
    private float _transitionTime = 0.1f, _transitionTimeCur;
    private Vector3 _axisMove = new Vector3(), _axisMovePrev = new Vector3(), _worldPos;

    public void OnMove(InputAction.CallbackContext context)
    {
        _axisMovePrev = transform.localPosition;
        Vector2 val = context.ReadValue<Vector2>();
        _axisMove = new Vector3(val.x, 0, val.y);
        //Debug.Log(_axisMove);
        _transitionTimeCur = 0;
        _worldPos = transform.position;
    }

    void Start()
    {
        _worldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_axisMove.magnitude > 0)
        {
            _transitionTimeCur += Time.deltaTime;
            _transitionTimeCur = _transitionTimeCur > _transitionTime ? _transitionTime : _transitionTimeCur;

            transform.localPosition = Vector3.Lerp(_axisMovePrev, _axisMove, _transitionTimeCur / _transitionTime);
        }
        else
        {
            _worldPos.y = transform.position.y;
            transform.position = _worldPos;

        }
    }
}

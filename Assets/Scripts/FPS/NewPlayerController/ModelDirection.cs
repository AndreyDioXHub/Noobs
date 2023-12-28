using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelDirection : MonoBehaviour
{
    private float _transitionTime = 0.1f, _transitionTimeCur;
    private Vector3 _axisMove = new Vector3(), _axisMovePrev = new Vector3();
    [SerializeField]
    private Transform _worldPos;
    [SerializeField]
    private Vector3 m;

    public void OnMove(InputAction.CallbackContext context)
    {
        //_axisMovePrev = transform.localPosition;
        Vector2 val = context.ReadValue<Vector2>();
        _axisMove = new Vector3(val.x, 0, val.y);
        //Debug.Log(_axisMove);
        _transitionTimeCur = 0;
        //_worldPos = transform.position;
    }

    public void OnAvatarMove(Vector3 axisNet)
    {
        _axisMove = axisNet;
        _transitionTimeCur = 0;
        //_worldPos = transform.position;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_axisMove.magnitude > 0)
        {
            _axisMovePrev = _axisMove;
        }
    }

    private void LateUpdate()
    {
        if (_axisMove.magnitude > 0)
        {
            _transitionTimeCur += Time.deltaTime;
            _transitionTimeCur = _transitionTimeCur > _transitionTime ? _transitionTime : _transitionTimeCur;

            m = Vector3.Lerp(_axisMovePrev, _axisMove, _transitionTimeCur / _transitionTime);

            transform.position = _worldPos.position + _worldPos.forward * m.z + _worldPos.right * m.x;
        }
        else
        {
            //_worldPos.y = transform.position.y;
            //transform.position = _worldPos.position;
        }
    }
}

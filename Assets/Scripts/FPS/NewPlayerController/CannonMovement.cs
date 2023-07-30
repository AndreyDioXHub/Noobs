using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMovement : Movement
{
    public static CannonMovement Instance;

    [SerializeField]
    private Transform _cannonBase;
    [SerializeField]
    private Transform _cannonBarrel;
    [SerializeField]
    private Transform _cannonSit;
    [SerializeField]
    protected Vector2 _minMaxAngle = new Vector2(-10, 30);
    [SerializeField]
    protected float _mouseSensitivity = 600f;

    protected float _xRotation;
    private Vector3 _playerInitPossition;
    private float _toCenterTransitionTime = 0.2f;
    private float _toCenterTransitionTimeCur;

    public override void Awake()
    {
        //base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public override void Update()
    {
        //base.Update();

        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation += mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);

        _cannonBarrel.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    public override void FixedUpdate()
    {
        //base.Update();

        _gravityVector.y += _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        _blender.AddOffcet(_gravityVector);

        _toCenterTransitionTimeCur += Time.fixedDeltaTime;
        _toCenterTransitionTimeCur = _toCenterTransitionTimeCur > _toCenterTransitionTime ? _toCenterTransitionTime : _toCenterTransitionTimeCur;

        Vector3 LerpPosition = Vector3.Lerp(transform.position, _cannonSit.position, _toCenterTransitionTimeCur / _toCenterTransitionTime);
        Vector3 LerpOffcet = LerpPosition - transform.position;
        LerpOffcet.y = 0;

        _blender.AddOffcet(LerpOffcet);
    }

    public override void Init()
    {
        _playerSpeed.SetSpeed(_speedWalk);
        //base.Init();
    }

    public void InitCannon(Transform cannonBase, Transform cannonBarrel, Transform sit)
    {
        _cannonBase = cannonBase;
        _cannonBarrel = cannonBarrel;
        _cannonSit = sit;
        _toCenterTransitionTimeCur = 0;
        _playerInitPossition = transform.position;
        _gravityVector = Vector3.zero;
    }

    public override void Move(float horizontal, float vertical)
    {
        //base.Move(horizontal, vertical);
    }

    public override void Jump(bool isJump)
    {
        //base.Jump(isJump);
    }

    public override void Run(bool isRun)
    {
        //base.Run(isRun);
    }

    public override void Crouch(bool isCrouch)
    {
        //base.Crouch(isCrouch);
    }
}

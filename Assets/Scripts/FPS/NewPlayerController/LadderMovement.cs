using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : Movement
{
    public static LadderMovement Instance;

    [SerializeField]
    private bool _isEnd;
    [SerializeField]
    private Transform _ladder;
    [SerializeField]
    private Transform _ladderEnd;

    private Vector3 _playerInitPossition;

    private float _toCenterTransitionTime = 0.2f;
    private float _toCenterTransitionTimeCur;

    public override void Awake()
    {
        //base.Awake();

        /*if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }/**/
    }


    public override void FixedUpdate()
    {
        //base.Update();
    }

    public override void Init()
    {
        _playerSpeed.SetSpeed(_speedWalk);
        //base.Init();
    }

    public void InitLadder(Transform ladder, Transform ladderend)
    {
        _ladder = ladder;
        _ladderEnd = ladderend;
        _toCenterTransitionTimeCur = 0;

        _playerInitPossition = Vector3.zero;

        if (transform.position.y < _ladderEnd.position.y)
        {
            _playerInitPossition = transform.position;
        }
    }

    public override void Move(float horizontal, float vertical)
    {
        Vector3 dif = transform.position - _ladderEnd.position;
        dif.y = 0;

        //Debug.Log((dif));

        float dot = 1;

        if (!_isTransition && _ladder != null )
        {

            dot = Vector3.Dot(_ladder.forward, transform.forward);
            dot = dot > 0 ? 1 : -1;

            // Debug.Log(Vector3.Dot(_ladder.forward, transform.forward));

            if (transform.position.y < _ladderEnd.position.y && _playerInitPossition != Vector3.zero)
            {
                _toCenterTransitionTimeCur += Time.fixedDeltaTime;
                _toCenterTransitionTimeCur = _toCenterTransitionTimeCur > _toCenterTransitionTime ? _toCenterTransitionTime : _toCenterTransitionTimeCur;
               // Debug.Log(_toCenterTransitionTimeCur / _toCenterTransitionTime);
                Vector3 LerpPosition = Vector3.Lerp(_playerInitPossition, _ladder.transform.position, _toCenterTransitionTimeCur / _toCenterTransitionTime);

                Vector3 LerpOffcet = LerpPosition - transform.position;
                LerpOffcet.y = 0;

                _blender.AddOffcet(LerpOffcet);

                /*if (LerpOffcet.magnitude > 0.1f)
                {
                    _blender.AddOffcet(LerpOffcet);
                }*/

                //Debug.Log($"LerpOffcet {LerpOffcet}");
            }
        }

        if (transform.position.y > _ladderEnd.position.y)
        {
            Vector3 playerPos = transform.position - (_ladderEnd.position - _ladderEnd.forward*0.1f);
            playerPos.y = 0;
            playerPos = playerPos.normalized;

            float goStraightOrDown = Vector3.Dot(_ladderEnd.forward, playerPos);
            goStraightOrDown = goStraightOrDown > 0 ? 1 : -1;
            //Debug.Log((goStraightOrDown));

            if (goStraightOrDown > 0)
            {
                _offcetMove = (_ladder.forward * vertical * dot) * _playerSpeed.Speed * Time.fixedDeltaTime;
            }
            else
            {
                _offcetMove = -(transform.up) * _playerSpeed.Speed * Time.fixedDeltaTime;

                if (_playerInitPossition == Vector3.zero)
                {
                    _playerInitPossition = transform.position;
                }
            }
        }
        else
        {
            //Debug.Log($"up down {vertical * dot}");

            _offcetMove = (transform.up * vertical * dot) * _playerSpeed.Speed * Time.fixedDeltaTime;
            //Debug.Log($"up down {_offcetMove}");
        }

        _blender.AddOffcet(_offcetMove);


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

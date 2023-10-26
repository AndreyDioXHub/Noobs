using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobBouncer : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;


    [SerializeField]
    private float _jumpHeight = 15;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private int _jumpsCount = 1, _jumpsCountCur;
    [SerializeField]
    private bool _destroyByDistance;
    [SerializeField]
    private GameObject _player;


   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_destroyByDistance)
        {
            if(_player != null)
            {
                if(Vector3.Distance(_player.transform.position, _parent.transform.position) > 4)
                {
                    Destroy(_parent);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _animator.SetBool("Jump", true);

        if (other.tag.Equals("Player"))
        {
            ScoreView.Instance.AddScore();

            _player = other.gameObject;
            _jumpsCountCur++;
            if (other.TryGetComponent<Movement>(out Movement movement))
            {
                movement.Push(transform.up, _jumpHeight);

                if (other.TryGetComponent<GroundCheck>(out GroundCheck groundCheck))
                {
                    groundCheck.Pause();
                }
            }
            if(_jumpsCountCur == _jumpsCount)
            {
                if (_jumpsCount != 0)
                {
                    NoodleJumpManager.Instance.SetIndex(_parent.transform.position);
                    if (_parent.TryGetComponent<Platforms>(out Platforms platforms))
                    {
                        platforms.DestroyPlatform();
                    }
                    //Destroy(_parent);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("Jump", false);

    }
}

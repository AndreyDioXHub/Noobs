using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderInteraction : MonoBehaviour
{
    [SerializeField]
    private bool _isActive;
    [SerializeField]
    private bool _isEnd;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private Transform _endTransform;

   void Start()
    {
        //Debug.Log(transform.forward);
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            if (_playerTransform.position.y > _endTransform.position.y)
            {
            }
            else
            {
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _playerTransform = null;
        }
    }

    public void Interaction()
    {
        if (_isActive)
        {
            CharacterStateController.Instance.ChangeState(State.stay);
            _isActive = false;
        }
        else
        {
            CharacterStateController.Instance.ChangeState(State.ladder);
            LadderMovement.Instance.InitLadder(transform, _endTransform);
            _isActive = true;
        }
        
    }

    public void LaeveLadder()
    {
        CharacterStateController.Instance.ChangeState(State.stay);
        _isActive = false;
    }
}

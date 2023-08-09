using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobBouncer : MonoBehaviour
{
    [SerializeField]
    private float _jumpHeight = 15;
    [SerializeField]
    private Animator _animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _animator.SetBool("Jump", true);

        if (other.tag.Equals("Player"))
        {
            if (other.TryGetComponent<Movement>(out Movement movement))
            {
                movement.Push(transform.up, _jumpHeight);

                if (other.TryGetComponent<GroundCheck>(out GroundCheck groundCheck))
                {
                    groundCheck.Pause();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("Jump", false);
    }
}

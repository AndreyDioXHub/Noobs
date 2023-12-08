using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobBouncer : NetworkBehaviour
{
    [SerializeField]
    private float _jumpHeight = 15;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _audioSoursePrefab;

    void Start()
    {
    }
    public override void OnStartLocalPlayer()
    {
        Debug.Log($"NoobBouncer: isLocalPlayer: {isLocalPlayer} isClient: {isClient} isServer: {isServer}");
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
            if(_audioSoursePrefab != null)
            {
                if (isClient || isLocalPlayer)
                {
                    CmdSpawnAudio();
                }
            }

            if (other.TryGetComponent(out RobloxController movement))
            {
                movement.Push(transform.up, _jumpHeight);

                if (other.TryGetComponent(out GroundCheck groundCheck))
                {
                    groundCheck.Pause();
                }
            }
        }
    }

    [Command]
    void CmdSpawnAudio()
    {
        GameObject bulletClone = Instantiate(_audioSoursePrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bulletClone);
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("Jump", false);
    }


    private void OnDestroy()
    {
    }

    private void OnDisable()
    {
    }

}

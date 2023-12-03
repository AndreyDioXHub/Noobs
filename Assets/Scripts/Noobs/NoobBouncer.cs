using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobBouncer : MonoBehaviour
{
    [SerializeField]
    private float _jumpHeight = 15;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioClip _jumper;
    [SerializeField]
    private GameObject _audioSoursePrefab;

    private Queue<GameObject> _audioSources = new Queue<GameObject>();
    [SerializeField]
    private float _despawnTime = 2;
    [SerializeField]
    private bool _isDespawning;

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
            if(_audioSoursePrefab != null)
            {
                var go = Instantiate(_audioSoursePrefab, transform);

                var audio = go.GetComponent<AudioSource>();
                audio.clip = _jumper;
                audio.Play();
                _audioSources.Enqueue(go);

                if (!_isDespawning)
                {
                    StartCoroutine(DespawnAudio());
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

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("Jump", false);
    }

    IEnumerator DespawnAudio()
    {
        _isDespawning = true;

        while (_audioSources.Count > 0)
        {
            yield return new WaitForSeconds(_despawnTime);
            Destroy(_audioSources.Dequeue());
        }

        _isDespawning = false;
    }

    private void OnDestroy()
    {
        DeactivateCouroutines();
    }

    private void OnDisable()
    {
        DeactivateCouroutines();
    }

    private void DeactivateCouroutines()
    {
        StopAllCoroutines();

        while (_audioSources.Count > 0)
        {
            Destroy(_audioSources.Dequeue());
        }
    }
}

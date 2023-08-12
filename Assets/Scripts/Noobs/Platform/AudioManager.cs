using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _sources;

    void Start()
    {
        _sources = GetComponents<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        foreach(var source in _sources)
        {
            source.enabled = AudioState.AUDIO_ON;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformLVLAnimatorController : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _text0;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _time = 10;
    [SerializeField]
    private float _timeCur;
    [SerializeField]
    private List<string> _animationSequence = new List<string>();
    [SerializeField]
    private int _index = 0;
    [SerializeField]
    private bool _isPaused;
    [SerializeField]
    private bool _isStarted;


    void Start()
    {
        
    }

    void Update()
    {
        if (_isStarted)
        {
            _timeCur += Time.deltaTime;

            if (_timeCur > _time)
            {
                if (_isPaused)
                {
                    _isPaused = false;
                    _animator.Play("Platform Idle Animation");
                }
                else
                {
                    if (_index < _animationSequence.Count)
                    {
                        _animator.Play(_animationSequence[_index]);
                    }
                    _index++;
                    _text0.text = $"{_index}";
                     _isPaused = true;

                }
                _timeCur = 0;
            }
        }        
    }


    public void Init(List<string> animationSequence)
    {
        _animationSequence = animationSequence;
        _isStarted = true;
    }
}

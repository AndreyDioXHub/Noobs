using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformLVLAnimatorController : MonoBehaviour
{
    /*
    [SerializeField]
    private TextMeshProUGUI _text0;*/
    [SerializeField]
    private string _idleAnimationName;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _timePlay = 10;
    [SerializeField]
    private float _timePause = 10;
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
    [SerializeField]
    private bool _needCoins;

    [SerializeField]
    private GameObject _coinPrefab;


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
                    _time = _timePause;
                    _isPaused = false;
                    _animator.Play(_idleAnimationName);

                    if (_needCoins)
                    {
                        StartCoroutine(SpawnCoinsCoroutine());
                    }
                }
                else
                {
                    _time = _timePlay;

                    if (_index < _animationSequence.Count)
                    {
                        _animator.Play(_animationSequence[_index]);
                    }

                    _index++;
                    //_text0.text = $"{_index}";
                     _isPaused = true;

                }
                _timeCur = 0;
            }
        }        
    }

    IEnumerator SpawnCoinsCoroutine()
    {
        int i = 0;

        while (i < 20)
        {
            GameObject eatEffect = Instantiate(_coinPrefab);
            float randomX = Random.Range(-14, 14);
            float randomZ = Random.Range(-14, 14);
            eatEffect.transform.position = new Vector3(randomX, 1.15f, randomZ);

            Destroy(eatEffect, 10);
            i++;
            yield return new WaitForSeconds(0.05f);// WaitForEndOfFrame();
        }
    }


    public void Init(List<string> animationSequence)
    {
        _animationSequence = animationSequence;
        _isStarted = true;
        _time = _timePause; 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource _walk;
    [SerializeField]
    private AudioSource _jump;
    [SerializeField]
    private AudioSource _land;
    [SerializeField]
    private List<AudioSource> _dies = new List<AudioSource>();
    [SerializeField]
    private GroundCheck _groundCheck;

    [SerializeField]
    private bool _isGround;
    [SerializeField]
    private bool _isGroundPrev;


    void Start()
    {
        
    }

    void Update()
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }

        _isGround = _groundCheck.IsGrounded;

        if(_isGround && !_isGroundPrev)
        {
            PlayLand();
        }
    }

    private void LateUpdate()
    {
        _isGroundPrev = _isGround;
    }



    public void PlayMove(bool isMove)
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }

        if (isMove)
        {
            if (!_walk.isPlaying)
            {
                _walk.Play();
            }
        }
        else
        {
            if (_walk.isPlaying)
            {
                _walk.Stop();
            }
        }
    }

    public void PlayJump()
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }
        _jump.Play();
    }
    public void PlayLand()
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }
        _land.Play();
    }

    public void PlayDie()
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }
        foreach (var d in _dies)
        {
            d.Play();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    [SerializeField]
    private AnimatorController _controller;
    [SerializeField]
    private AudioSource _walk;
    [SerializeField]
    private AudioSource _jump;
    [SerializeField]
    private AudioSource _land;
    [SerializeField]
    private List<AudioSource> _dies = new List<AudioSource>();


    void Start()
    {
        _controller.MovingStateChange += MovingStateChange;
    }

    private void MovingStateChange(bool isjump)
    {
        if (isjump)
        {
            if (_jump.isPlaying)
            {
                _jump.Stop();
            }

            if (!_land.isPlaying)
            {
                _land.Play();
            }
        }
        else
        {
            if (!_jump.isPlaying)
            {
                _jump.Play();
            }

            if (_land.isPlaying)
            {
                _land.Stop();
            }
        }
        //PlayLand();
    }

    void Update()
    {
        if (_controller.IsLocalPlayer)
        {
            if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin
                || ChatTexts.IsActive || AdsManager.AdsPlaying)
            {
                return;
            }
        }
        else
        {

        }

        if (_controller.AxisMove.magnitude > 0 && _controller.IsGrounded)
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

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

    }

    void Update()
    {
        if (_controller.IsLocalPlayer)
        {
            if (ChatTexts.IsActive)
            {
                return;
            }
        }
        else
        {
            if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.IsWIN 
                || AdsManager.AdsPlaying)
            {
                return;
            }
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
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.IsWIN
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }
        foreach (var d in _dies)
        {
            d.Play();
        }
    }

    public void PlayJumpLand(bool isGrounded)
    {
        if (_controller.IsLocalPlayer)
        {
            if (ChatTexts.IsActive)
            {
                return;
            }
        }
        else
        {
            if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.IsWIN
                || AdsManager.AdsPlaying)
            {
                return;
            }
        }


        if (isGrounded == false)
        {
            if (!_jump.isPlaying)
            {
                _jump.Play();
            }
        }
        else
        {
            if (!_land.isPlaying)
            {
                _land.Play();
            }
        }
    }

    public void PlayLand()
    {
        if (SettingScreen.IsActive || AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.IsWIN
            || ChatTexts.IsActive || AdsManager.AdsPlaying)
        {
            return;
        }

    }

}

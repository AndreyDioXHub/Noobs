using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _prevSkin;
    [SerializeField]
    private List<string> _skinNames = new List<string>();
    [SerializeField]
    private AnimatorController _animatorController;

    [SerializeField]
    NetworkAnimator _netAnimator;

    void Start()
    {
    }

    public void Init(CharType type)
    {

        Destroy(_prevSkin);

        Animator animator = null;

        switch (type)
        {
            case CharType.player:
                string skin = PlayerPrefs.GetString("PlatformCurentSkins", "skin0");
                var gop = Instantiate(Resources.Load<GameObject>(skin), transform);
                animator = gop.GetComponent<Animator>();
                _animatorController.Init(animator);
                _netAnimator.animator = animator;
                _prevSkin = gop;
                break;
            case CharType.avatar:
                string randomskin = _skinNames[Random.Range(0, _skinNames.Count)];
                var goa = Instantiate(Resources.Load<GameObject>(randomskin), transform);
                animator = goa.GetComponent<Animator>();
                _animatorController.Init(animator);
                _netAnimator.animator = animator;
                _prevSkin = goa;
                break;
            case CharType.bot:
                string randomskinb = _skinNames[Random.Range(0, _skinNames.Count)];
                var gob = Instantiate(Resources.Load<GameObject>(randomskinb), transform);
                animator = gob.GetComponent<Animator>();
                _animatorController.Init(animator);
                _netAnimator.animator = animator;
                _prevSkin = gob;
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    internal void SetAvatarSkin(int newindex) {
        // Remove previous
        Destroy(_prevSkin);
        
        Animator animator = null;

        string skinname = _skinNames[newindex];
        var goa = Instantiate(Resources.Load<GameObject>(skinname), transform);
        animator = goa.GetComponent<Animator>();
        _animatorController.Init(animator);
        _netAnimator.animator = animator;
    }
}

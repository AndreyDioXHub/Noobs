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
    private KeyBoardRecorder _recorder;

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
                string skin = PlayerPrefs.GetString(PlayerPrefsConsts.CURENTSKIN, "skin0");
                var gop = Instantiate(Resources.Load<GameObject>(skin), transform);
                animator = gop.GetComponent<Animator>();
                _animatorController.Init(animator);
                _prevSkin = gop;
                break;
            case CharType.avatar:
                string randomskin = _skinNames[Random.Range(0, _skinNames.Count)];
                var goa = Instantiate(Resources.Load<GameObject>(randomskin), transform);
                animator = goa.GetComponent<Animator>();
                _animatorController.Init(animator);
                _prevSkin = goa;
                break;
            case CharType.bot:
                string randomskinb = RandomSkin();// _skinNames[Random.Range(0, _skinNames.Count)];
                var gob = Instantiate(Resources.Load<GameObject>(randomskinb), transform);
                animator = gob.GetComponent<Animator>();
                _animatorController.Init(animator);
                _recorder.SetAnimator(animator);
                _prevSkin = gob;
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public string RandomSkin()
    {
        string randomskin = "";

        int skinID = Random.Range(0, 80);

        if (skinID < 40)
        {
            randomskin = _skinNames[0];
        }
        else if(skinID < 60)
        {

            randomskin = _skinNames[1];
        }
        else if(skinID < 70)
        {

            randomskin = _skinNames[2];
        }
        else if(skinID < 75)
        {

            randomskin = _skinNames[3];
        }
        else if(skinID < 77)
        {

            randomskin = _skinNames[4];
        }
        else if(skinID < 78)
        {

            randomskin = _skinNames[5];
        }
        else if(skinID < 79)
        {

            randomskin = _skinNames[6];
        }
        else if(skinID < 80)
        {

            randomskin = _skinNames[7];
        }

        return randomskin;
    }

    /*
    internal void SetAvatarSkin(int newindex) {
        // Remove previous
        Destroy(_prevSkin);
        
        Animator animator = null;

        string skinname = _skinNames[newindex];
        var goa = Instantiate(Resources.Load<GameObject>(skinname), transform);
        animator = goa.GetComponent<Animator>();
        _animatorController.Init(animator);
    }*/
}

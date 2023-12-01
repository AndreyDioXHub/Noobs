using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public CheckPointState State = CheckPointState.passive;
    public bool IsFinish;
    public BoxCollider _box;
    private bool _itIsWasActive;
    [SerializeField]
    private int _checkPointIndex;
    [SerializeField]
    private Button _checkPointButton;
    [SerializeField]
    private GameObject _checkPointAvalable;
    [SerializeField]
    private GameObject _checkPointNotAvalable;

    [SerializeField]
    private bool _avaleble;

    void Start()
    {
        _box = GetComponent<BoxCollider>();
        _avaleble = PlayerPrefs.GetInt($"{PlayerPrefsConsts.checkpoint}{_checkPointIndex}", 0) == 1;

        if (_checkPointButton != null)
        {
            if (_avaleble)
            {
                _checkPointButton.interactable = true;
                _checkPointAvalable.SetActive(true);
                _checkPointNotAvalable.SetActive(false);
            }
            else
            {
                _checkPointButton.interactable = false;
                _checkPointAvalable.SetActive(false);
                _checkPointNotAvalable.SetActive(true);
            }
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CheckPointManager.Instance.SetActiveCheckPoint(this);

            if (_checkPointButton != null)
            {
                if (_checkPointIndex >= 0 && !_avaleble)
                {
                    PlayerPrefs.SetInt($"{PlayerPrefsConsts.checkpoint}{_checkPointIndex}", 1);
                    _avaleble = true;
                    _checkPointButton.interactable = true;
                    _checkPointAvalable.SetActive(true);
                    _checkPointNotAvalable.SetActive(false);
                }
            }
                
        }

        //_box.enabled = false;
    }
    /*
    public bool TrySetActive()
    {
        bool result = false;

        if (_itIsWasActive)
        {

        }
        else
        {
            State = CheckPointState.active;
            _itIsWasActive = true;
        }

        return result;
    }*/
}

public enum CheckPointState
{
    passive,
    active
}

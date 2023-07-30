using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject _canonColliderReload;
    [SerializeField]
    private InteractableObject _reloadInteractableObject;
    [SerializeField]
    private Transform _base;
    [SerializeField]
    private Transform _barrel;
    [SerializeField]
    private Transform _sit;
    [SerializeField]
    private bool _isActive;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Activate()
    {
        if (_isActive)
        {
            CharacterStateController.Instance.ChangeState(State.stay);
            _isActive = false;
            _reloadInteractableObject.Deactivate();
            _canonColliderReload.SetActive(false);

        }
        else
        {
            CharacterStateController.Instance.ChangeState(State.cannon);
            CannonMovement.Instance.InitCannon(_base, _barrel, _sit);
            _isActive = true;
            _canonColliderReload.SetActive(true);
        }
    }

    public void Exit()
    {
        _isActive = false;
        _reloadInteractableObject.Deactivate();
        _canonColliderReload.SetActive(false);
    }
}

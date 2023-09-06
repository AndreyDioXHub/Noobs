using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionHat : MonoBehaviour
{
    [SerializeField]
    private CharType _type;
    [SerializeField]
    private GameObject _nameCanvas;
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private CapsuleCollider _netWorkCollideer;
    [SerializeField]
    private GroundCheck _grounCheck;
    [SerializeField]
    private PositionOffcetBlender _blender;
    [SerializeField]
    private KeyboardInput _playerInput;
    [SerializeField]
    private BotInput _botInput;
    [SerializeField]
    private CameraView _playerCameraInput;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private PlayerSpeed _speed;
    [SerializeField]
    private Movement _playerMovement;
    [SerializeField]
    private LifeManager _lifeManager;
    [SerializeField]
    private SkinManager _skinManager;


    void Start()
    {
        
    }

    void Update()
    {
        /*
        if (GameManager.Instance.IsWin || GameManager.Instance.IsLose)
        {
            Pause();
        }*/
    }

    public void Pause()
    {
        _grounCheck.enabled = false;
        _characterController.enabled = false;
        _blender.enabled = false;
        _playerInput.enabled = false;
        _botInput.enabled = false;
        _playerCameraInput.enabled = false;
        _speed.enabled = false;
        _playerMovement.enabled = false;
        _lifeManager.enabled = false;
    }

    public void Play()
    {
        switch (_type)
        {
            case CharType.player:
                _grounCheck.enabled = true;
                _characterController.enabled = true;
                _blender.enabled = true;
                _playerInput.enabled = true;
                _botInput.enabled = false;
                _playerCameraInput.enabled = true;
                _speed.enabled = true;
                _playerMovement.enabled = true;
                _lifeManager.enabled = true;
                _netWorkCollideer.enabled = false;
                _grounCheck.Pause();
                break;
            case CharType.avatar:
                _grounCheck.enabled = false;
                _characterController.enabled = false;
                _blender.enabled = false;
                _playerInput.enabled = false;
                _botInput.enabled = false;
                _playerCameraInput.enabled = false;
                _speed.enabled = false;
                _playerMovement.enabled = false;
                _lifeManager.enabled = false;
                _netWorkCollideer.enabled = true;
                break;
            case CharType.bot:
                _grounCheck.enabled = true;
                _characterController.enabled = true;
                _blender.enabled = true;
                _playerInput.enabled = false;
                _botInput.enabled = true;
                _playerCameraInput.enabled = false;
                _speed.enabled = true;
                _playerMovement.enabled = true;
                _lifeManager.enabled = true;
                _netWorkCollideer.enabled = false;
                _grounCheck.Pause();
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void Init(CharType type)
    {
        _type = type;
        gameObject.name = type.ToString();
        Play();
        /*
        switch (_type)
        {
            case CharType.player:

                tag = "Player";

                _skinManager.Init(CharType.player);
                _nameCanvas.SetActive(false);
                _grounCheck.enabled = true;
                _characterController.enabled = true;
                _blender.enabled = true;
                _playerInput.enabled = true;
                _botInput.enabled = false;
                _playerCameraInput.enabled = true;
                _camera.SetActive(true);
                _speed.enabled = true;
                _playerMovement.enabled = true;
                _lifeManager.enabled = true;
                _netWorkCollideer.enabled = false;
                break;
            case CharType.avatar:

                tag = "Avatar";

                _skinManager.Init(CharType.avatar);
                _nameCanvas.SetActive(true);
                _nameCanvas.GetComponentInChildren<NameManager>().Init();
                _grounCheck.enabled = false;
                _characterController.enabled = false;
                _blender.enabled = false;
                _playerInput.enabled = false;
                _botInput.enabled = false;
                _playerCameraInput.enabled = false;
                Destroy(_camera);
                _speed.enabled = false;
                _playerMovement.enabled = false;
                _lifeManager.enabled = false;
                _netWorkCollideer.enabled = true;
                break;
            case CharType.bot:

                tag = "Bot";

                _skinManager.Init(CharType.bot);
                _nameCanvas.SetActive(true);
                _nameCanvas.GetComponentInChildren<NameManager>().Init();
                _grounCheck.enabled = true;
                _characterController.enabled = true;
                _blender.enabled = true;
                _playerInput.enabled = false;
                _botInput.enabled = true;
                _playerCameraInput.enabled = false;
                Destroy(_camera);
                _speed.enabled = true;
                _playerMovement.enabled = true;
                _lifeManager.enabled = true;
                _netWorkCollideer.enabled = false;
                _grounCheck.Pause();
                break;
            default:
                Destroy(gameObject);
                break;
        }*/
    }

    internal void Init() {
        Init(_type);
    }
}

public enum CharType
{
    player,
    avatar,
    bot
}

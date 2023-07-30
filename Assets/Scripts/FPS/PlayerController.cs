using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField]
    private CharacterController _character;
    [SerializeField]
    private PlayerMovement _curMovement;
    [SerializeField]
    private Gun _curGun;
    [SerializeField]
    private List<PlayerMovement> _movements = new List<PlayerMovement>();//0-walk, 

    [SerializeField]
    private Transform _playerCamera;
    [SerializeField]
    private float _mouseSensitivity = 100f;
    [SerializeField]
    private Vector2 _minMaxAngle;
    [SerializeField]
    private Vector2 _mouse;

    public float _xRotation = 0f;
    private float _yRotation = 0f;

    [SerializeField]
    private float _interactableDistance = 2;
    private bool _foundInteractable = false;

    public CharacterController Character
    {
        get { return _character; }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        #region Mouse Look and Look RayCast
        _xRotation -= _mouse.y * _mouseSensitivity * Time.deltaTime;
        _yRotation = _mouse.x * _mouseSensitivity * Time.deltaTime;

        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);
        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * _yRotation);

        Ray ray = new Ray(_playerCamera.position, _playerCamera.forward); // Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit[] hits; 
        hits = Physics.RaycastAll(ray, _interactableDistance);

        if(hits.Length > 0)
        {
            if (hits[0].collider.tag.Equals(WorldValues.Instance.InteractableObjectsTag) == true)
            {
                Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _interactableDistance, Color.green);
                _foundInteractable = true;
            }
            else
            {
                Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _interactableDistance, Color.red);
                _foundInteractable = false;
            }
        }
        else
        {
            Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _interactableDistance, Color.red);
            _foundInteractable = false;
        }
        #endregion
    }

    public void OnMouseLookX(InputAction.CallbackContext value)
    {
        _mouse.x = value.ReadValue<float>();
        //Debug.Log(value);
    }

    public void OnMouseLookY(InputAction.CallbackContext value)
    {
        _mouse.y = value.ReadValue<float>();
        // Debug.Log(value);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        _curMovement.MoveValue(value.ReadValue<Vector2>());
        if (value.phase.ToString().Equals("Started") || value.phase.ToString().Equals("Canceled"))
        {
            //Debug.Log(value.phase.ToString());
            
        }
        //
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.phase.ToString().Equals("Started"))
        {
            _curMovement.JumpValue();
        }
    }

    public void OnFire(InputAction.CallbackContext value)
    {
        //Debug.Log(value.phase.ToString());
        if (value.phase.ToString().Equals("Started"))
        {
            //Debug.Log("fire");
            _curGun.StartShooting();
           // _curMovement.JumpValue();
        }
        else if (value.phase.ToString().Equals("Canceled"))
        {
            _curGun.StopShooting();
        }

        //_curMovement.JumpValue();
    }

    public void OnReloading(InputAction.CallbackContext value)
    {
        if (value.phase.ToString().Equals("Started"))
        {
            _curGun.GunReloading();
        }
    }
}

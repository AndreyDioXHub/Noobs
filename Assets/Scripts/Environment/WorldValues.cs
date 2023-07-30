using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldValues : MonoBehaviour
{
    public static WorldValues Instance;

    [SerializeField]
    private string _playerObjectName;
    [SerializeField]
    private string _interactableObjectsTag;
    [SerializeField]
    private GameObject _windMill;
    [SerializeField]
    private float _windSpeed = 5f;


    private Vector3 _gravity = new Vector3(0, -9.8f, 0);
    private Vector3 _wind;

    public string PlayerObjectName
    {
        get => _playerObjectName;
    }

    public float WindSpeed {
        get => _windSpeed;
    }

    public string InteractableObjectsTag
    {
        get => _interactableObjectsTag;
    }

    public Vector3 Wind
    {
        get => _wind;
    }

    public Vector3 WindNormilize {
        get => _windMill.transform.forward;
    }

    public Vector3 Gravity
    {
        get => _gravity;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _wind = _windMill.transform.forward * _windSpeed;
    }
}

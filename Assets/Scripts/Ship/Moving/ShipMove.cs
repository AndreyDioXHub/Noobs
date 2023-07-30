using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour
{
    [Header("Характеристики")]
    [SerializeField, Tooltip("Max ship speed in km/h")]
    float maxSpeed = 20;
    [SerializeField, Tooltip("Текущая скорость. Если задать не ноль, то корабль будет сразу плыть.")]
    float currentSpeed = 0; //Current speed of ship
    [SerializeField, Range(-1, 0), Tooltip("Инертность корабля (до -1)")]
    float inert = -0.2f;
    [SerializeField, Tooltip("Максимальный угол поворота корабля")]
    float MaxAngle = 30;
    [SerializeField, Tooltip("Kell range rotation")]
    Vector2 MaxKeelRotation = new Vector2(-40, 40);
    [SerializeField, Tooltip("Mast range rotation")]
    Vector2 MaxMastRotation = new Vector2(-80, 80);
    [SerializeField, Range(0, 1), Tooltip("Воздействие якоря")]
    float anchor = 0;
    [Tooltip("We can stop moving for debug")]
    public bool CanMove = true;
    

    //HIDED [SerializeField, Range(0,1)]
    float acceleration = 0;
    
    
    
    
    [Header("External elements")]
    [Header("Movement")]
    //[SerializeField]
    Vector3 sailDirection;  //Rotation of sail
    //[SerializeField]
    Vector3 wind;
    [Header("Rotation")]
    //[SerializeField]
    float rotationKeel = 0;
    
    //[SerializeField]
    float currentAdditionalAngle = 0;
    //[SerializeField]
    Vector3 deltaKeel;
    
    float minAnchorForce = 0.1f;

    float prevRotation = 0;

    [Header("GameObjects")]
    public GameObject Sails;
    public GameObject Wind;
    public GameObject Keel;
    [SerializeField, Tooltip("Используется для наклона при повороте")]
    Transform _shipCarkas;

    float CrossWindSail;
    float _aspeed = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        sailDirection = Sails.transform.forward;
        GetRotationByKeel();
        //Show flag
        Wind.transform.LookAt(Wind.transform.position + wind * 10);
    }

    /// <summary>
    /// Calc sheep rotation by keel rotation
    /// </summary>
    private void GetRotationByKeel() {
        float _speedD = currentSpeed / maxSpeed;
        rotationKeel = 1-Vector3.Dot(Keel.transform.forward, transform.forward);        
        rotationKeel *= Math.Sign(Vector3.Dot(Keel.transform.forward, transform.right));
        currentAdditionalAngle = rotationKeel * MaxAngle * Time.deltaTime;
        Quaternion rot = transform.rotation;
        Vector3 nrot = transform.rotation.eulerAngles;
        prevRotation = nrot.y;
        nrot.y -= currentAdditionalAngle *  _speedD / Mathf.Clamp((1-anchor), minAnchorForce,1);
        rot.eulerAngles = nrot;
        transform.rotation = rot;
        //Calculate angle speed (sinple)
        float drot = prevRotation - nrot.y;
        if(Math.Abs(drot) > 0.01f) {   //TODO Set correct threshold
            _aspeed = drot * 180 * -1;// 90 - Math.Abs(drot);
            Vector3 _shipWrot = _shipCarkas.localEulerAngles;
            Quaternion newRotQ = Quaternion.AngleAxis(_aspeed, Vector3.forward);
//            _shipCarkas.localRotation = Quaternion.AngleAxis(_aspeed, Vector3.forward);
            _shipCarkas.localRotation = Quaternion.Lerp(_shipCarkas.localRotation, newRotQ, Time.deltaTime);
            _shipWrot.z =  _aspeed;
            //TODO Require normilize localEulerAngles
            _shipWrot = Vector3.Lerp(_shipCarkas.localEulerAngles, _shipWrot, Time.deltaTime);
            //Or use Quaternion
            //_shipCarkas.localEulerAngles = _shipWrot;
        } else {
            _shipCarkas.localRotation = Quaternion.Lerp(_shipCarkas.localRotation, Quaternion.identity, Time.deltaTime);
            //_shipCarkas.localEulerAngles = Vector3.Lerp(_shipCarkas.localEulerAngles, Vector3.zero, Time.deltaTime);
        }

    }

    /// <summary>
    /// Установка поворота киля.
    /// Допустимые пределы от -40 до 40 градусов.
    /// </summary>
    /// <param name="angle"></param>
    public void SetKeelRotation(float angle) {
        Vector3 eangle = Keel.transform.localEulerAngles;
        eangle.y = NormilizeAndClapmAngle(angle, MaxKeelRotation);
        Keel.transform.localEulerAngles = eangle;

    }

    private float NormilizeAndClapmAngle(float angle, Vector2 range) {
        if(angle > 180 && angle <= 360) {
            angle = angle - 360;
        }
        angle = Mathf.Clamp(angle, range.x, range.y);
        Debug.Log($"clapm angle = {angle}");
        return angle;
    }

    public void SetKeelRotationSingle(float value) {
        value = value * -1;
        float _range = Mathf.Clamp(value, -1, 1) + Keel.transform.localEulerAngles.y;
        Debug.Log($"value = {value}   range = {_range}");
        SetKeelRotation(_range);
    }

    /// <summary>
    /// Установка положения мачты. 
    /// Допустимые пределы от -80 до 80 градусов (позднее вынесем в отдельные настройки)
    /// </summary>
    /// <param name="angle"></param>
    public void SetSailsRotation(float angle) {
        Vector3 eangle = Sails.transform.localEulerAngles;
        eangle.y = NormilizeAndClapmAngle(angle, MaxMastRotation);
        Debug.Log(eangle.y);
        Sails.transform.localEulerAngles = eangle;
    }

    public void SetSailsRotationSingle(float value) {
        float _range = Mathf.Clamp(value, -1, 1) + Sails.transform.localEulerAngles.y;
        SetSailsRotation(_range);
    }

    private void FixedUpdate() {
        Vector3 fwrd = transform.forward;
        acceleration = GetAcceleration();
        currentSpeed += (acceleration + inert)*Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        Vector3 dist = fwrd * (currentSpeed *Time.deltaTime);
        Vector3 pos = transform.position + dist;
        if(CanMove) transform.position = pos;
    }

    /// <summary>
    /// Расчет ускорения движения корабля.
    /// Позднее в расчет нужно включить поддержку нескольких мачт
    /// </summary>
    /// <returns></returns>
    private float GetAcceleration() {
        //TODO Потом вынести расчет sailDirection в отдельный метод, который будет складывать 
        // позиции всех мачт.
        //TODO Добавить параметр раскрытости всем парусов, который так же будет влиять на движущую силу.
        CrossWindSail = Vector3.Dot(wind, sailDirection);
        float movepower = Mathf.Clamp(Vector3.Dot(sailDirection, transform.forward) * CrossWindSail, -1, 1);
        return movepower;
    }

    /// <summary>
    /// Временный метод задания направления ветра.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UpdateWind(float x, float y) {
        wind.x = y;
        wind.z = -x;
        wind.y = 0;
    }
}

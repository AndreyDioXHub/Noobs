using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Bullet : MonoBehaviour
{
    public event Action<Transform, Transform> OnCollided = delegate { };

    private Transform _inoffTarget;
    private float _lifeTime = 60f;

    private float _startSpeed;
    private float _time = 0;
    private float _nexttime = 0;
    private Vector3 _direction;    
    private Vector3 _origin;

    private Vector3 _position;
    private Vector3 _nextPosition;

    private Vector3 _bulletDirection;
    private float _bulletDeltaPath = 0;

    private bool _bulletExist = false;
    [SerializeField]
    public bool SetHitForce = false;
    //private bool _stopThread = false;
    /*public float MyTime
    {
        get => _time;

    }
    public float MyNexttime
    {
        get => _nexttime;

    }

    public float StartSpeed
    {
        get => _startSpeed;

    }

    public Vector3 Direction
    {
        get => _direction;
    }

    public Vector3 Origin 
    {
        get => _origin;
    }

    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }
    public Vector3 NextPosition
    {
        get => _nextPosition;
        set => _nextPosition = value;
    }
    public Vector3 BulletDirection
    {
        set => _bulletDirection = value;
    }
    public float BulletDeltaPath
    {
        set => _bulletDeltaPath = value;
    }*/

    public Bullet(float startspeed, Vector3 direction)
    {
        _startSpeed = startspeed;
        _direction = direction;
    }

    void Start()
    {
        _lifeTime = 60f;
    }

    // Update is called once per frame
    void Update()
    {
       /* if(_time > _lifeTime)
        {
            _bulletExist = false;
        }*/

        _time += Time.deltaTime;
        _nexttime = _time + Time.deltaTime;

        _position = _origin + _startSpeed * _direction * _time + WorldValues.Instance.Wind * _time * _time + WorldValues.Instance.Gravity * _time * _time / 2f;
        _nextPosition = _origin + _startSpeed * _direction * _nexttime + WorldValues.Instance.Wind * _nexttime * _nexttime + WorldValues.Instance.Gravity * _nexttime * _nexttime / 2f;
        _bulletDirection = (_nextPosition - _position).normalized;
        _bulletDeltaPath = Vector3.Distance(_nextPosition, _position);

        transform.position = _position;
        _bulletDirection = (_nextPosition - _position).normalized;
        _bulletDeltaPath = Vector3.Distance(_nextPosition, _position);

        if (Physics.Raycast(_position, _bulletDirection, out RaycastHit hit, _bulletDeltaPath))
        {
            if (hit.collider.name.Equals(WorldValues.Instance.PlayerObjectName) == true)
            {
                Debug.DrawRay(_position, _bulletDirection * _bulletDeltaPath, Color.white);
            }
            else
            {
                Debug.DrawRay(_position, _bulletDirection * _bulletDeltaPath, Color.red);
                HitHandler(hit.collider.transform);
            }
        }
        else
        {
            Debug.DrawRay(_position, _bulletDirection * _bulletDeltaPath, Color.white);
        }

        if(_bulletExist == false)
        {
            DestroyBullet();
        }

        //Debug.Log(_time+" "+ _lifeTime);
        if (_time > _lifeTime)
        {
            _bulletExist = false;
        }
    }

    /*async private void BulletTrajectory()
    {
        await Task.Run(BulletTrajectoryTast);
    }

    private void BulletTrajectoryTast()
    {
        while (_bulletExist == true || _stopThread == false)
        {
            _position = _origin + _startSpeed * _direction * _time + WorldValues.Instance.Wind * _time * _time + WorldValues.Instance.Gravity * _time * _time / 2f;
            _nextPosition = _origin + _startSpeed * _direction * _nexttime + WorldValues.Instance.Wind * _nexttime * _nexttime + WorldValues.Instance.Gravity * _nexttime * _nexttime / 2f;
            _bulletDirection = (_nextPosition - _position).normalized;
            _bulletDeltaPath = Vector3.Distance(_nextPosition, _position);
        }
    }
    */
    public void InitBullet(float startspeed, Vector3 direction)
    {
        Debug.Log("InitBullet");
        _startSpeed = startspeed;
        _direction = direction;
        _origin = transform.position;
        _bulletExist = true;
        //_stopThread = false;
        //BulletTrajectory();
    }

    /*private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        _stopThread = true;
        //_bulletExist = false;
    }*/

    private void HitHandler(Transform hitted)
    {
        if(SetHitForce) {
            Collider[] _colliders = Physics.OverlapSphere(transform.position, 3);
            foreach (var item in _colliders) {
                Rigidbody _body = item.GetComponent<Rigidbody>();
                if (_body != null) {
                    //float _radius = (hitted.position - transform.position).magnitude * 5;
                    _body.AddExplosionForce(_startSpeed, transform.position, 3, 1, ForceMode.Impulse);
                }
            }

            //Rigidbody _body = hitted.gameObject.GetComponent<Rigidbody>();
            
        }
        OnCollided?.Invoke(transform, hitted);
        //Debug.Log("bullet hit the target: " + hitted.name);
        _bulletExist = false;
        _inoffTarget = hitted;
        
        //Destroy(this.gameObject);
    }

    private void DestroyBullet()
    {
        Debug.Log("bullet was destoyed");
        Destroy(this.gameObject);
    }
}

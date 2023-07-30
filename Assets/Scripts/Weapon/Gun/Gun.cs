using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private int _LinePointOffcet = 5;
    [SerializeField]
    private GunType _gunType;
    [SerializeField]
    private bool _showPath;
    [SerializeField]
    private LineRenderer _linePath;

    private Vector3[] _points = new Vector3[100];

    private bool _isShoot = false;
    private bool _canShoot = true;
    private float _shotDelay = 0;
    private int _currentBulletCount;
    

    private void Start()
    {
        InitNewGun();
        //_origin = transform.position;
    }

    public void StartShooting()
    {
        _isShoot = true;        
    }

    public void StopShooting()
    {
        _isShoot = false;
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        bullet.transform.eulerAngles = transform.eulerAngles;
        Bullet newbullet = bullet.AddComponent<Bullet>();
        newbullet.InitBullet(_gunType.BulletSpeed, transform.forward);
    }

    public void GunReloading()
    {
        StartCoroutine(GunReloadingCourutine());
    }

    private IEnumerator GunReloadingCourutine()
    {
        yield return new WaitForSeconds(_gunType.ReloadingTime);
        _canShoot = true;
        _currentBulletCount = _gunType.MagazineSize[0];
    }

    private IEnumerator ShotHandlerCoroutine()
    {
        _currentBulletCount = _currentBulletCount - _gunType.BulletPerShoot;
        _canShoot = false;        
        yield return new WaitForSeconds(_shotDelay);
        _canShoot = true;
        if (_currentBulletCount <= 0)
        {
            _canShoot = false;
        }
    }

    private void Update()
    {
        if (_showPath == true)
        {
            Show();
        }
        else
        {
            _linePath.gameObject.SetActive(false);
        }

        if (_isShoot == true)
        {
            if (_canShoot == true)
            {
                SpawnBullet();                
                StartCoroutine(ShotHandlerCoroutine());
            }
        }
    }

    public void InitNewGun()
    {
        _LinePointOffcet = (160 - (int)_gunType.BulletSpeed) / 30;
        _shotDelay = 1f / _gunType.FireRate;
        _currentBulletCount = _gunType.MagazineSize[0];
    }

    public void Show()
    {
        if (_linePath.gameObject.activeSelf == false)
        {
            _linePath.gameObject.SetActive(true);
        }

        for (int i = 0; i < _points.Length; i++)
        {
            float time = (i + _LinePointOffcet)* 0.02f;
            float nexttime = (i + _LinePointOffcet + 1) * 0.02f;
            _points[i] = transform.position + _gunType.BulletSpeed * transform.forward * time + WorldValues.Instance.Wind * time * time + WorldValues.Instance.Gravity * time * time / 2f;
            Vector3 nextpoint = transform.position + _gunType.BulletSpeed * transform.forward * nexttime + WorldValues.Instance.Wind * nexttime * nexttime + WorldValues.Instance.Gravity * nexttime * nexttime / 2f;
            float distance = Vector3.Distance(_points[i], nextpoint);
            Vector3 direction = (nextpoint - _points[i]).normalized;

            if (Physics.Raycast(_points[i], direction, out RaycastHit hit, distance))
            {
                if (hit.collider.name.Equals(WorldValues.Instance.PlayerObjectName) == true)
                {
                    Debug.DrawRay(_points[i], (nextpoint - _points[i]) * distance, Color.white);
                }
                else
                {
                    //Debug.Log(hit.collider.name + " " + (i + 1) + " " + distance);

                    Debug.DrawRay(_points[i], direction * distance, Color.red);
                    _linePath.positionCount = i + 1;
                    break;
                }                
            }
            else
            {
                Debug.DrawRay(_points[i], (nextpoint - _points[i]) * distance, Color.white);
            }

        }

        _linePath.SetPositions(_points);
    }

    private void OnDisable()
    {
        StopThisGun();
    }

    private void OnDestroy()
    {
        StopThisGun();
    }

    private void StopThisGun()
    {
        StopAllCoroutines();
    }
}

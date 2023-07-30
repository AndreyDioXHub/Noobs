using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersRing : MonoBehaviour
{
    public int SkinID { get => _skinID; }

    [SerializeField]
    private List<Transform> _pedestals = new List<Transform>();
    [SerializeField]
    private float _angleStep;
    [SerializeField]
    private Vector3 _destenationAnngle;
    [SerializeField]
    private Vector3 _curnAnngle;
    [SerializeField]
    private float _time = 1;
    [SerializeField]
    private float _timeCur;

    [SerializeField]
    private int _skinID;


    void Start()
    {
        _angleStep = 360 / _pedestals.Count;
        _skinID = PlayerPrefs.GetInt("PlatformSkinID", 0);
        transform.eulerAngles = new Vector3(0, _skinID * _angleStep, 0);
        _curnAnngle = transform.eulerAngles;
        _destenationAnngle = transform.eulerAngles;
    }

    void Update()
    {
        _timeCur += Time.deltaTime;
        _timeCur = _timeCur > _time ? _time : _timeCur;    
        transform.eulerAngles = Vector3.Lerp(_curnAnngle, _destenationAnngle, _timeCur / _time);
    }

    public void Next()
    {
        if(_timeCur == _time)
        {
            _timeCur = 0;
            _destenationAnngle = transform.eulerAngles;
            _curnAnngle = transform.eulerAngles;
            _destenationAnngle.y += _angleStep;
            _skinID++;
            _skinID = _skinID > _pedestals.Count - 1 ? 0 : _skinID;
            PlayerPrefs.SetInt("PlatformSkinID", _skinID);
        }
    }

    public void Prev()
    {
        if (_timeCur == _time)
        {
            _timeCur = 0;
            _destenationAnngle = transform.eulerAngles;
            _curnAnngle = transform.eulerAngles;
            _destenationAnngle.y -= _angleStep;
            _skinID--;
            _skinID = _skinID < 0 ? _pedestals.Count - 1 : _skinID;
            PlayerPrefs.SetInt("PlatformSkinID", _skinID);
        }
    }

    [ContextMenu("Zerable")]
    public void Zerable()
    {
        PlayerPrefs.SetInt("PlatformCoins", 0);
        PlayerPrefs.SetString("PlatformSkins", "");
    }
}

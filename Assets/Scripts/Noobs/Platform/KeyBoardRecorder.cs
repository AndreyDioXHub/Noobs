using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class KeyBoardRecorder : MonoBehaviour
{
    [SerializeField]
    private List<TextAsset> _assets = new List<TextAsset>();
    [SerializeField]
    private DistributionHat _hat;
    [SerializeField]
    private AnimatorController _animatorController;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _cameraCenter;
    [SerializeField]
    private List<Positions> _positions = new List<Positions>();
    [SerializeField]
    private Transform _dir;
    [SerializeField]
    protected Transform _model;
    [SerializeField]
    private bool _isStart;

    [SerializeField]
    private ReplayState _state = ReplayState.replay;
    [SerializeField]
    private int _index = 0;


    void Start()
    {
        _hat.Pause(); 
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void Prepare()
    {
        gameObject.SetActive(true);

        switch (_state)
        {
            case ReplayState.replay:
                _cameraCenter.SetActive(false);
                gameObject.tag = "Bot";
                _animatorController.enabled = false;
                _hat.SetType(CharType.bot);
                string json = "";

                if (_assets.Count > 0)
                {
                    try
                    {
                        Debug.Log("Bot loaded from data");

                        json = _assets[PlatformGameManager.Instance.Index].ToString();
                        _positions = JsonConvert.DeserializeObject<List<Positions>>(json);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Debug.Log("Bot not loaded");
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    string path = Path.Combine(Application.streamingAssetsPath, $"{PlatformGameManager.Instance.Index}", $"{name} index ({PlatformGameManager.Instance.Index}).json");

                    if (File.Exists(path))
                    {
                        Debug.Log("Bot loaded from Streaming Assets");
                        json = File.ReadAllText(path);
                        _positions = JsonConvert.DeserializeObject<List<Positions>>(json);
                    }
                    else
                    {
                        Debug.Log("Bot not loaded");
                        gameObject.SetActive(false);
                    }
                }

                break;
            case ReplayState.record:
                _hat.SetType(CharType.player);
                _cameraCenter.SetActive(true);
                break;
            default:
                break;

        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_isStart)
        {
            switch (_state)
            {
                case ReplayState.replay:
                    transform.position = new Vector3(_positions[_index].mpX, _positions[_index].mpY, _positions[_index].mpZ);
                    transform.eulerAngles = new Vector3(_positions[_index].meX, _positions[_index].meY, _positions[_index].meZ);
                    _dir.localPosition = new Vector3(_positions[_index].dpX, _positions[_index].dpY, _positions[_index].dpZ);
                    _index++;

                    if (_index > 0)
                    {
                        try
                        {
                            if(_animator != null)
                            {
                                if (Mathf.Abs(_positions[_index - 1].mpY - _positions[_index].mpY) > 0.001f)
                                {
                                    _animator.SetBool("Fall", true);
                                    //Debug.Log(_positions[_index - 1].mpY - _positions[_index].mpY);
                                }
                                else
                                {
                                    _animator.SetBool("Fall", false);

                                    if (_positions[_index - 1].mpX != _positions[_index].mpX || _positions[_index - 1].mpZ != _positions[_index].mpZ)
                                    {
                                        _animator.SetBool("Run", true);
                                    }
                                    else
                                    {
                                        _animator.SetBool("Run", false);
                                    }
                                }
                            }                            
                        }
                        catch (ArgumentOutOfRangeException e)
                        {

                            Destroy(gameObject);
                            PlayerCount.Instance.RemovePlayer();
                        }

                    }

                    _model.LookAt(_dir, _model.up);
                    break;
                case ReplayState.record:
                    _positions.Add(new Positions(transform.position.x, transform.position.y, transform.position.z,
                        transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z,
                        _dir.localPosition.x, _dir.localPosition.y, _dir.localPosition.z));
                    break;
                default:
                    break;

            }
        }
        
    }

    public void StartProcess()
    {
        _isStart = true;

        switch (_state)
        {
            case ReplayState.replay:
                break;
            case ReplayState.record:
                _hat.Play();
                break;
            default:
                break;

        }
    }

    public void StartRecord()
    {
        _state = ReplayState.record;
    }

    public void StopRecord()
    {
        _state = ReplayState.replay;
        gameObject.SetActive(false);
        string json = JsonConvert.SerializeObject(_positions);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, $"{name} index ({PlatformGameManager.Instance.Index}).json"), json);
    }
}

public enum ReplayState
{
    record,
    replay
}

[Serializable]
public class Positions
{
    public float mpX;
    public float mpY;
    public float mpZ;

    public float meX;
    public float meY;
    public float meZ;

    public float dpX;
    public float dpY;
    public float dpZ;
    public Positions(float mpX, float mpY, float mpZ, float meX, float meY, float meZ, float dpX, float dpY, float dpZ)
    {
        this.mpX = mpX;
        this.mpY = mpY;
        this.mpZ = mpZ;

        this.meX = meX;
        this.meY = meY;
        this.meZ = meZ;

        this.dpX = dpX;
        this.dpY = dpY;
        this.dpZ = dpZ;
    }
}

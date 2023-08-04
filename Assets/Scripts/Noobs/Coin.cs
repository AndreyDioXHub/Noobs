using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool HaveCoin { get => _haveCoin; }

    [SerializeField]
    private Animator _coinAnimator;
    [SerializeField]
    private Transform _coinAnchor;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Vector3 _coinOriginalPosition;
    [SerializeField]
    private float _time;
    [SerializeField]
    private bool _isProcess;
    [SerializeField]
    private bool _needFlash = true;
    [SerializeField]
    private bool _haveCoin = true;

    private bool _haveMaster ;

    public static List<Coin> Instances { get; private set; } = new List<Coin>();



   //[ContextMenu("GetCoin")]
    public void GetCoin(GameObject player)
    {
        if (!_haveMaster)
        {
            if(player != null)
            {
                _haveMaster = true;

                if (_haveCoin)
                {
                    _coinAnimator.SetTrigger("Get");

                    _coinOriginalPosition = _coinAnchor.position;
                    _player = player.transform;
                    _isProcess = true;

                    if (player.tag.Equals("Player"))
                    {
                        CoinManager.Instance.AddCoin();
                    }
                }
            }

            Destroy(gameObject, 3f);
        }
    }

    void Start()
    {
        if(ServerNetworkBehaviour.Instance != null && ServerNetworkBehaviour.Instance.isServerOnly) {
            Debug.Log("Enable server mode");
            EnableServerMode();
            return;
        }


        _haveCoin = Random.Range(0, 10) == 0;

        if (_haveCoin)
        {           
            CoinManager.Instance.RegisterCoin();
        }
        else
        {
            _coinAnchor.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isProcess)
        {
            if(_player == null)
            {
                Destroy(gameObject);
            }
            else
            {
                _time += Time.deltaTime;
                _time = _time > 0.55f ? 0.55f : _time;
                _coinAnchor.position = Vector3.Lerp(_coinOriginalPosition, _player.position, _time / 0.55f);

                if (_time > 0.5f && _needFlash)
                {
                    GameObject eatEffect = Instantiate(Resources.Load<GameObject>("WorldSphereEatEffect"));
                    eatEffect.transform.position = _player.position;
                    _needFlash = false;
                }
            }
        }
    }

    [ContextMenu("EnableServerMode")]
    public void EnableServerMode()
    {
        _coinAnchor.gameObject.SetActive(false);
        this.enabled = false;
    }

    [ContextMenu("DisableServerMode")]
    public void DisableServerMode()
    {
        _coinAnchor.gameObject.SetActive(true);
        this.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        GetCoin(other.gameObject);
    }
}

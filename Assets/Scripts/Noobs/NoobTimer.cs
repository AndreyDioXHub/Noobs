using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoobTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text0;
    [SerializeField]
    private GameObject _playerPrefab;


    void Start()
    {
        var go = Instantiate(_playerPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        _text0.text = $"{Time.timeSinceLevelLoad}";
    }
}

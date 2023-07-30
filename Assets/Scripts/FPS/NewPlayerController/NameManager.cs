using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _peopleCountText;
    [SerializeField]
    private List<string> _names = new List<string>();
    [SerializeField]
    private GameObject _mainCamera;



    void Start()
    {
        _peopleCountText.text = $"{_names[Random.Range(0, _names.Count)]}";
        _mainCamera = Camera.main.gameObject;
    }

    void Update()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.position - gameObject.transform.position, Vector3.up);
    }
}

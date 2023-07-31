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
        //_peopleCountText.text = $"{_names[Random.Range(0, _names.Count)]}";
        //_mainCamera = Camera.main.gameObject;
        StartCoroutine(FindMainCamera());
    }

    private IEnumerator FindMainCamera() {
        bool camNotfound = true;
        while(camNotfound) {
            camNotfound = Camera.main == null;
            yield return new WaitForSeconds(0.05f);
        }
        Init();
    }

    void Update()
    {
        if(_mainCamera != null)
            gameObject.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.position - gameObject.transform.position, Vector3.up);
    }

    public void Init() {
        _peopleCountText.text = $"{_names[Random.Range(0, _names.Count)]}";
        _mainCamera = Camera.main.gameObject;
    }
}

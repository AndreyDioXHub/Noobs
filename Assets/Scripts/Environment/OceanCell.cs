using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _cell;

    // Start is called before the first frame update
    void Start()
    {
        _cell.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(GameObject cell)
    {
        _cell = cell;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Ocean.Instance.ActivateNewOceanCell(_cell);
            //_cell.SetActive(true);
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _cell.SetActive(false);
        }
    }*/
}

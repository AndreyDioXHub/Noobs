using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairColorModel : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _hairs = new List<GameObject>();
    [SerializeField]
    private int _index;
    private float _time = 0.1f;

   private void OnEnable()
    {
        if (SkinManager.Instance == null)
        {
            StartCoroutine(LaterOnEnable());
        }
        else
        {
            Select(SkinManager.Instance.HairSelectedIndex);
            SkinManager.Instance.OnHairChanged.AddListener(Select);
        }
    }

    IEnumerator LaterOnEnable()
    {
        yield return new WaitForSeconds(_time);

        if (SkinManager.Instance == null)
        {
            StartCoroutine(LaterOnEnable());
        }
        else
        {
            Select(SkinManager.Instance.HairSelectedIndex);
            SkinManager.Instance.OnHairChanged.AddListener(Select);
        }
    }

    private void OnDisable()
    {
        SkinManager.Instance.OnHairChanged.RemoveListener(Select);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Select(int index)
    {
        if (_index == index)
        {
            foreach(var hair in _hairs)
            {
                hair.SetActive(true);
            }
        }
        else
        {
            foreach (var hair in _hairs)
            {
                hair.SetActive(false);
            }
        }
    }

}

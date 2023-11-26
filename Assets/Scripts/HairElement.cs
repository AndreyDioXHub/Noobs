using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairElement : MonoBehaviour
{
    [SerializeField]
    private GameObject _active;
    [SerializeField]
    private int _index;
    [SerializeField]
    private Button _button;


    public void Init(int index)
    {
        _index = index;
        _active.SetActive(false);
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Select);
    }

    public void Select(int index, Color buyColor)
    {
        if (_index == index)
        {
            _active.SetActive(true);
            _active.GetComponent<Image>().color = buyColor;
        }
        else
        {
            _active.SetActive(false);
        }
    }

    public void Select()
    {
        GetComponentInParent<HairSelected>().SelectNewColor(_index);
    }

    void Start()
    {
    }

    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalitraElement : MonoBehaviour
{
    public Color EquipedColor { get => _color; }

    [SerializeField]
    private Image _image;
    [SerializeField]
    private GameObject _active;
    [SerializeField]
    private int _index;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Button _button;


    public void Init(int index)
    {
        _image.color = _color;
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
        GetComponentInParent<Palitra>().SelectNewColor(_index);
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}

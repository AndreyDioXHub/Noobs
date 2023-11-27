using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Palitra : MonoBehaviour
{
    public UnityEvent<int> OnColorBuyed = new UnityEvent<int>();
    public UnityEvent<Color> OnColorSelect = new UnityEvent<Color>();
    public UnityEvent<Color> OnColorEquiped = new UnityEvent<Color>();

    [SerializeField]
    private PalitraElement[] _elements;
    [SerializeField]
    private int _selectedColorIndex;
    [SerializeField]
    private int _equipedColorIndex = 0;
    [SerializeField]
    private Color _buyed;
    [SerializeField]
    private Color _notBuyed;
    [SerializeField]
    private GameObject _buyButton;
    [SerializeField]
    private GameObject _equipButton;
    [SerializeField]
    private GameObject _notEnoughButton;
    [SerializeField]
    private int _cost;


    private Dictionary<int, bool> _avalebleColors = new Dictionary<int, bool>();/*
    {
        { 0, true},
        { 1, false},
        { 2, false},
        { 3, false},
        { 4, false},
        { 5, false},
        { 6, false},
        { 7, false},
        { 8, false},
        { 9, false},
        { 10, false},
        { 11, false},
    };*/

    void Start()
    {
    }

    public void Init(List<bool> avalable)
    {
        for(int i=0; i< avalable.Count; i++)
        {
            _avalebleColors.Add(i, avalable[i]);

            if (avalable[i])
            {
                _equipedColorIndex = i;
                _selectedColorIndex = i;
            }
        }
    }

    private void OnEnable()
    {
        var elements = transform.GetComponentsInChildren<PalitraElement>();
        _elements = elements;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Init(i);
            _elements[i].Select(_selectedColorIndex, WatColor(i));
        }

        _buyButton.GetComponent<Button>().onClick.AddListener(BuyColor);
        _equipButton.GetComponent<Button>().onClick.AddListener(EquipNewColor);

        WatButtons();
        OnColorSelect?.Invoke(_elements[_selectedColorIndex].EquipedColor);
    }

    public void SelectNewColor(int index)
    {
        _selectedColorIndex = index;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_selectedColorIndex, WatColor(_selectedColorIndex));
        }
        WatButtons();
        OnColorSelect?.Invoke(_elements[_selectedColorIndex].EquipedColor);
    }

    public void EquipNewColor()
    {
        _equipedColorIndex = _selectedColorIndex;
        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_selectedColorIndex, WatColor(_selectedColorIndex));
        }
        WatButtons();
        OnColorEquiped?.Invoke(_elements[_equipedColorIndex].EquipedColor);
    }

    public void BuyColor()
    {
        if (CoinManager.Instance.TryToBuy(_cost))
        {
            _avalebleColors[_selectedColorIndex] = true;
            OnColorBuyed?.Invoke(_selectedColorIndex);
            WatButtons();
        }
    }

    public void WatButtons()
    {
        if (_avalebleColors[_selectedColorIndex])
        {
            if (_equipedColorIndex == _selectedColorIndex)
            {
                _buyButton.SetActive(false);
                _notEnoughButton.SetActive(false);
                _equipButton.SetActive(false);
            }
            else
            {
                _buyButton.SetActive(false);
                _notEnoughButton.SetActive(false);
                _equipButton.SetActive(true);
            }
        }
        else
        {
            _equipButton.SetActive(false);

            if (CoinManager.Instance.CheckMoney(_cost))
            {
                _buyButton.SetActive(true);
                _notEnoughButton.SetActive(false);
            }
            else
            {
                _buyButton.SetActive(false);
                _notEnoughButton.SetActive(true);
            }
        }
    }

    public Color WatColor(int index)
    {
        return _avalebleColors[index] ? _buyed : _notBuyed;
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HairSelected : MonoBehaviour
{
    public UnityEvent<int> OnFaceSelect = new UnityEvent<int>();
    public UnityEvent<int> OnFaceEquiped = new UnityEvent<int>();

    [SerializeField]
    private HairElement[] _elements;
    [SerializeField]
    private int _selectedFaceIndex;
    [SerializeField]
    private int _equipedFaceIndex = 0;
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


    private Dictionary<int, bool> _avalebleFaces = new Dictionary<int, bool>();/*
    {
        { 0, true},
        { 1, false},
        { 2, false},
        { 3, false},
        { 4, false},
        { 5, false},
        { 6, false},
        { 7, false},
    };*/

    void Start()
    {
    }
    public void Init(List<bool> avalable)
    {
        for (int i = 0; i < avalable.Count; i++)
        {
            _avalebleFaces.Add(i, avalable[i]);

            if (avalable[i])
            {
                _equipedFaceIndex = i;
                _selectedFaceIndex = i;
            }
        }
    }

    private void OnEnable()
    {
        var elements = transform.GetComponentsInChildren<HairElement>();
        _elements = elements;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Init(i);
            _elements[i].Select(_selectedFaceIndex, WatColor(i));
        }

        _buyButton.GetComponent<Button>().onClick.AddListener(BuyColor);
        _equipButton.GetComponent<Button>().onClick.AddListener(EquipNewColor);

        WatButtons();
        OnFaceSelect?.Invoke(_selectedFaceIndex);
    }

    public void SelectNewColor(int index)
    {
        _selectedFaceIndex = index;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_selectedFaceIndex, WatColor(_selectedFaceIndex));
        }
        WatButtons();
        OnFaceSelect?.Invoke(_selectedFaceIndex);
    }

    public void EquipNewColor()
    {
        _equipedFaceIndex = _selectedFaceIndex;
        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_selectedFaceIndex, WatColor(_selectedFaceIndex));
        }
        WatButtons();
        OnFaceEquiped?.Invoke(_equipedFaceIndex);
    }

    public void BuyColor()
    {
        if (CoinManager.Instance.TryToBuy(_cost))
        {
            _avalebleFaces[_selectedFaceIndex] = true;
            WatButtons();
        }
    }

    public void WatButtons()
    {
        if (_avalebleFaces[_selectedFaceIndex])
        {
            if (_equipedFaceIndex == _selectedFaceIndex)
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
        return _avalebleFaces[index] ? _buyed : _notBuyed;
    }

    void Update()
    {

    }
}

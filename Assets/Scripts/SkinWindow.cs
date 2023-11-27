
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinWindow : MonoBehaviour
{
    public UnityEvent<int> OnElementBuyed = new UnityEvent<int>();
    public UnityEvent<int> OnElementSelect = new UnityEvent<int>();
    public UnityEvent<int> OnElementEquiped = new UnityEvent<int>();

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private SkinWindowElement[] _elements;
    [SerializeField]
    private int _selectedElementIndex;
    [SerializeField]
    private int _equipedElementIndex = 0;
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


    private Dictionary<int, bool> _avalebleElements = new Dictionary<int, bool>();

    void Start()
    {
    }

    public void Init(List<bool> avalable, int selectedIndex)
    {
        for (int i = 0; i < avalable.Count; i++)
        {
            _avalebleElements.Add(i, avalable[i]);
        }

        if (selectedIndex >= 0)
        {
            _equipedElementIndex = selectedIndex;
            _selectedElementIndex = selectedIndex;
        }
    }

    private void OnEnable()
    {
        var elements = _content.GetComponentsInChildren<SkinWindowElement>();
        _elements = elements;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Init(i, this);
            _elements[i].Select(_selectedElementIndex, WatColor(i));
        }

        _buyButton.GetComponent<Button>().onClick.AddListener(BuyElement);
        _equipButton.GetComponent<Button>().onClick.AddListener(EquipElement);

        WatButtons();
        OnElementSelect?.Invoke(_selectedElementIndex);
    }

    public void SelectElement(int index)
    {
        _selectedElementIndex = index;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_selectedElementIndex, WatColor(_selectedElementIndex));
        }
        WatButtons();
        OnElementSelect?.Invoke(_selectedElementIndex);
    }

    public void EquipElement()
    {
        _equipedElementIndex = _selectedElementIndex;
        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_selectedElementIndex, WatColor(_selectedElementIndex));
        }
        WatButtons();
        OnElementEquiped?.Invoke(_equipedElementIndex);
    }

    public void BuyElement()
    {
        if (CoinManager.Instance.TryToBuy(_cost))
        {
            _avalebleElements[_selectedElementIndex] = true;
            OnElementBuyed?.Invoke(_selectedElementIndex);
            WatButtons();
        }
    }

    public void WatButtons()
    {
        if (_avalebleElements[_selectedElementIndex])
        {
            if (_equipedElementIndex == _selectedElementIndex)
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
        return _avalebleElements[index] ? _buyed : _notBuyed;
    }

    void Update()
    {

    }
}

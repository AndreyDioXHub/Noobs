using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityEvent OnInteraction = new UnityEvent();
    public UnityEvent OnExit = new UnityEvent();

    public InteractableObjectType InteractableType { get => _interactableType; }

    [SerializeField]
    private string _key = "E";
    [SerializeField]
    private string _curentText;
    [SerializeField]
    private string _interactableText = "Активировать <b><color=red>keyCode</color></b>";
    [SerializeField]
    private string _activeText = "Отпустить <b><color=red>keyCode</color></b>";
    [SerializeField]
    private InteractableObjectType _interactableType;
    [SerializeField]
    private bool _isActive; 

    void Start()
    {
        _interactableText = _interactableText.Replace("keyCode", _key);
        _activeText = _activeText.Replace("keyCode", _key);
        _curentText = _interactableText;
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        Interact(_key);
    }

    public void Interact(string key)
    {
        //Debug.Log($"{_key} {key}");
        
        if (key.Equals(_key))
        {
            OnInteraction?.Invoke();

            _isActive = !_isActive;

            if (_isActive)
            {
                StatusText.Instance.RemoveText(_curentText);
                _curentText = _activeText;
                StatusText.Instance.AddText(_curentText);
            }
            else
            {
                StatusText.Instance.RemoveText(_curentText);
                _curentText = _interactableText;
                StatusText.Instance.AddText(_curentText);
            }

            //Debug.Log(key);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !_isActive)
        {
            StatusText.Instance.AddText(_curentText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        StatusText.Instance.RemoveText(_curentText);
        _curentText = _interactableText;
        _isActive = false;
        OnExit?.Invoke();
    }
}

public enum InteractableObjectType
{
    ladder,
    cannon
}

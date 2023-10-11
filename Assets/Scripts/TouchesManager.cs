using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class TouchesManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text0;
    [SerializeField]
    private List<RectTransform> _rects = new List<RectTransform>();
    [SerializeField]
    private OnScreenButton _forward;
    [SerializeField]
    private OnScreenButton _back;
    [SerializeField]
    private OnScreenButton _left;
    [SerializeField]
    private OnScreenButton _right;
    [SerializeField]
    private OnScreenButton _jump;
    [SerializeField]
    private OnScreenStick _mouse;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _text0.text = $"{Input.touchCount}";
        int t = Input.touchCount;
        var touches = Input.touches;

        for (int i=0; i< _rects.Count; i++)
        {
            _rects[i].anchoredPosition = new Vector2(0,0);
        }

        for (int i = 0; i < t; i++)
        {
            if(i < _rects.Count)
            {
                _rects[i].anchoredPosition = touches[i].position;
            }
        }

        if (touches.Length == 0)
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoobTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _text0.text = $"{Time.timeSinceLevelLoad}";
    }
}

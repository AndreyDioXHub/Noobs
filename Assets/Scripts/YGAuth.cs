using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class YGAuth : MonoBehaviour
{
    [SerializeField]
    private YandexGame _sdk;

    // Start is called before the first frame update
    void Start()
    {
        _sdk._OpenAuthDialog();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

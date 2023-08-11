using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    [SerializeField]
    private SkinManagerStart _skinManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Play()
    {
        PlayerPrefs.SetString(PlayerPrefsConsts.CURENTSKIN, _skinManager.CurentSkin);
        SceneManager.LoadScene(1);
        //PiratesNetworkManager.singleton.StartHost();
    }
}

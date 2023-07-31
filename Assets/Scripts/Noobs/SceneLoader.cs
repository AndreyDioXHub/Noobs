using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    NetworkManager networkManager;

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
        PlayerPrefs.SetString("PlatformCurentSkins", _skinManager.CurentSkin);
        //SceneManager.LoadScene(1);
        networkManager.StartHost();
    }
}

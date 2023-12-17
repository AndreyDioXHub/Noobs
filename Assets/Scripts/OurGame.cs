using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGame : MonoBehaviour
{
    [SerializeField]
    private string _url;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OpenOurGames()
    {
        string domain = "ru";// YandexGame.EnvironmentData.domain;
        _url = $"https://yandex.{domain}/games/developer?name=Lisenok%20Neposlushniy";

        Application.OpenURL(_url);
    }
}

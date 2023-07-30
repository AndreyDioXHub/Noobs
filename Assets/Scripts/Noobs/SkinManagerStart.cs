using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

public class SkinManagerStart : MonoBehaviour
{
    public string CurentSkin 
    { 
        get 
        {
            return _infos[_ring.SkinID].name;
        }
    }

    [SerializeField]
    private CharactersRing _ring;
    [SerializeField]
    private CoinManagerStart _coinManager;

    [SerializeField]
    private List<SkinInfo> _infos = new List<SkinInfo>()
    {
        new SkinInfo("skin0", true, 0),
        new SkinInfo("skin1", false, 200),
        new SkinInfo("skin2", false, 300),
        new SkinInfo("skin3", false, 500),
        new SkinInfo("skin4", false, 700),
        new SkinInfo("skin5", false, 900),
        new SkinInfo("skin6", false, 1500),
        new SkinInfo("skin7", false, 3000),
    };

    [SerializeField]
    private TextMeshProUGUI _costText0;
    [SerializeField]
    private TextMeshProUGUI _costText1;

    [SerializeField]
    private GameObject _playButton;
    [SerializeField]
    private GameObject _buyButton;

    void Start()
    {
        string skinJson = PlayerPrefs.GetString("PlatformSkins", ""); //JsonConvert.DeserializeObject<int[,]>(maprawjson);

        if (!string.IsNullOrEmpty(skinJson))
        {
            _infos = JsonConvert.DeserializeObject<List<SkinInfo>>(skinJson);
        }
    }

    void Update()
    {
        _costText0.text = $"{_infos[_ring.SkinID].cost}";
        _costText1.text = $"{_infos[_ring.SkinID].cost}";

        if (_infos[_ring.SkinID].avaleble)
        {
            _playButton.SetActive(true);
            _buyButton.SetActive(false);
        }
        else
        {
            _playButton.SetActive(false);
            _buyButton.SetActive(true);
        }
    }

    public void Buy()
    {
        if (_coinManager.TryBuy(_infos[_ring.SkinID].cost))
        {
            _infos[_ring.SkinID].avaleble = true;
            string skinJson = JsonConvert.SerializeObject(_infos);
            PlayerPrefs.SetString("PlatformSkins", skinJson);
        }
        else
        {

        }
    }
}

[Serializable]
public class SkinInfo
{
    public string name;
    public bool avaleble;
    public int cost;

    public SkinInfo(string name, bool avaleble, int cost)
    {
        this.name = name;
        this.avaleble = avaleble;
        this.cost = cost;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class BuyPomni : MonoBehaviour
{
    public UnityEvent<string> OnInfoUpdated = new UnityEvent<string>();

    [SerializeField]
    private string _pomniID = "Pomni";
    [SerializeField]
    private GameObject _buyButton;

    void Start()
    {
        StartCoroutine(UpdatePurchse());
        YandexGame.PurchaseSuccessEvent += OnPurchaseSuccess;
    }

    IEnumerator UpdatePurchse()
    {
        yield return new WaitForSeconds(1);

        Purchase purchase = YandexGame.PurchaseByID(_pomniID);

        if(purchase != null)
        {
            if (purchase.purchased > 0)
            {
                _buyButton.SetActive(false);

                string infoJSON = PlayerPrefs.GetString(PlayerPrefsConsts.USER_SKIN_KEY, "");

                SkinsInfo info = JsonConvert.DeserializeObject<SkinsInfo>(infoJSON);
                info.sexes[2] = true;
                infoJSON = JsonConvert.SerializeObject(info);
                OnInfoUpdated?.Invoke(infoJSON);
            }
        }
    }

    public void OnPurchaseSuccess(string key)
    {
        if (key.Equals(_pomniID))
        {
            _buyButton.SetActive(false);

            string infoJSON = PlayerPrefs.GetString(PlayerPrefsConsts.USER_SKIN_KEY, "");

            SkinsInfo info = JsonConvert.DeserializeObject<SkinsInfo>(infoJSON);
            info.sexes[2] = true;
            infoJSON = JsonConvert.SerializeObject(info);
            OnInfoUpdated?.Invoke(infoJSON);
            SkinManager.Instance.EquipSex(2);
        }
    }
    public void BuySkin()
    {
        YandexGame.BuyPayments(_pomniID);
    }

    void Update()
    {
        
    }
}

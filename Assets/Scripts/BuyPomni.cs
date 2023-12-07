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
        while (!PlayerSave.Instance.DataIsload)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1);

        Purchase purchase = YandexGame.PurchaseByID(_pomniID);

        if(purchase != null)
        {
            if (purchase.purchased > 0)
            {
                _buyButton.SetActive(false);

                SkinsInfo info = SkinManager.Info;
                info.sexes[2] = true;
                string infoJSON = JsonConvert.SerializeObject(info);
                OnInfoUpdated?.Invoke(infoJSON);
            }
        }
    }

    public void OnPurchaseSuccess(string key)
    {
        if (key.Equals(_pomniID))
        {
            _buyButton.SetActive(false);

            SkinsInfo info = SkinManager.Info;
            info.sexes[2] = true;
            string infoJSON = JsonConvert.SerializeObject(info);
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

    private void OnDestroy()
    {
        YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccess;
    }
}

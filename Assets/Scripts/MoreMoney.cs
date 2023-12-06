using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MoreMoney : MonoBehaviour
{

    void Start()
    {
        //StartCoroutine(UpdatePurchse());
        YandexGame.PurchaseSuccessEvent += OnPurchaseSuccess;
    }
    /*
    IEnumerator UpdatePurchse()
    {
        yield return new WaitForSeconds(1);

        Purchase purchase100 = YandexGame.PurchaseByID("buy100");
        Purchase purchase500 = YandexGame.PurchaseByID("buy100");
        Purchase purchase600 = YandexGame.PurchaseByID("buy100");

        if (purchase != null)
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
    }*/

    public void OnPurchaseSuccess(string key)
    {
        switch (key)
        {
            case "buy100":
                CoinManager.Instance.AddMoney(100);
                break;
            case "buy500":
                CoinManager.Instance.AddMoney(500);
                break;
            case "buy1000":
                CoinManager.Instance.AddMoney(1000);
                break;
        }
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccess;
    }

    private void OnDestroy()
    {
        YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccess;
    }

    public void BuySkin(string key)
    {
        YandexGame.BuyPayments(key);
    }

    void Update()
    {
        
    }
}

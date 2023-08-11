using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{

    [SerializeField]
    private int _life = 3;


    void Start()
    {
        
    }

    void Update()
    {
        
        if (transform.position.y < 0)
        {
            DieFunction();
            this.enabled = false;
        }
    }

    public void MinusLife()
    {
        _life--;

        if (_life <= 0)
        {
            DieFunction();
        }
    }

    private void DieFunction() {
        switch (tag) {
            case "Player":
                //Проверить, что не сервер
                if (ServerNetworkBehaviour.Instance == null) {
                    GameManager.Instance.IsLose = true;
                }
                break;
            case "Avatar":
            case "Bot":
                GetComponent<DistributionHat>().Pause();
                gameObject.SetActive(false);
                break;
            default:
                break;
        }

        PlayerCount.Instance.RemovePlayer();
        Debug.Log("Dead");
    }


    private void OnDestroy()
    {
    }
}

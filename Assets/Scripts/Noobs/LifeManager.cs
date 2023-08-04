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
            switch (tag)
            {
                case "Player":
                    break;
                case "Avatar":
                    break;
                case "Bot":
                    PlayerCount.Instance.RemovePlayer();
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    public void MinusLife()
    {
        _life--;

        if (_life <= 0)
        {
            switch (tag)
            {
                case "Player":
                    GameManager.Instance.IsLose = true;
                    break;
                case "Avatar":
                    break;
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
    }

    private void OnDestroy()
    {
    }
}

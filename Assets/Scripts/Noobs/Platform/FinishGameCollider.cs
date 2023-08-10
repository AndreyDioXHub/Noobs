using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGameCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bot"))
        {
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
        }
        else
        {
            if (other.TryGetComponent(out KeyBoardRecorder k))
            {
                k.StopRecord();
            }
        }
    }
}

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientConfiguration : NetworkBehaviour
{
    [SerializeField] GameObject clientTarget;

    public override void OnStartClient() 
    {
        base.OnStartClient();
        clientTarget?.SetActive(true);
    }

    public override void OnStartServer() 
    {
        base.OnStartServer();

        if(isServerOnly)
        {
            if(clientTarget != null)
            {
                Destroy(clientTarget);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButtonControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void LeaveStage() 
    {
        if(ObstacleNetworkManager.singleton == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            ObstacleNetworkManager.singleton.StopClient();
        }
    }
}

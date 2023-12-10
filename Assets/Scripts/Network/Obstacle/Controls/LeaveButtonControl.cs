using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButtonControl : MonoBehaviour
{
    [SerializeField]
    private bool _leaveOrLoad;

    public void LeaveStage() 
    {
        if(_leaveOrLoad)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            ObstacleNetworkManager.singleton.StopClient();
        }
    }
}

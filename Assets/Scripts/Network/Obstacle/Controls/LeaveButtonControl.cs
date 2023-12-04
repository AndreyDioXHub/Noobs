using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveButtonControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void LeaveStage() {
        ObstacleNetworkManager.singleton.StopClient();
    }
}

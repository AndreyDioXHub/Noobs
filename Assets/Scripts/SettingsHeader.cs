using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHeader : MonoBehaviour
{
    [SerializeField]
    private List<Stateble> _statebles = new List<Stateble>();


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SwitchScreen(int activeid, bool state)
    {
        if (state)
        {
            foreach (var stateble in _statebles)
            {
                if (stateble.ID != activeid)
                {
                    stateble.MakePassive();
                }
            }
        }
    }
}

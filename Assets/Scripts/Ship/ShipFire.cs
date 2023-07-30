using cyraxchel.gun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipFire : MonoBehaviour
{

    public InputAction cannon1;
    public InputAction cannon2;
    public InputAction cannon3;

    public CannonControl Cannon1;
    public CannonControl Cannon2;
    public CannonControl Cannon3;
    // Start is called before the first frame update
    void Start()
    {
        cannon1.performed += FireGun;
        cannon2.performed += FireGun;
        cannon3.performed += FireGun;
    }

    private void FireGun(InputAction.CallbackContext obj) {
        //        obj.action.
        Debug.Log("FIRE!" + obj.action.bindings[0]);
        if(obj.action == cannon1) {
            Cannon1.Fire();
        } else if(obj.action == cannon2) {
            Cannon2.Fire();
        } else if(obj.action == cannon3) {
            Cannon3.Fire();
        }
    }

    private void OnEnable() {
        cannon1.Enable();
        cannon2.Enable();
        cannon3.Enable();
    }
    private void OnDisable() {
        cannon1.Disable();
        cannon2.Disable();
        cannon3.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

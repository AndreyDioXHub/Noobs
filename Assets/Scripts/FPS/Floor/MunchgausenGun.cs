using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunchgausenGun : MonoBehaviour
{
    GameObject _current = null;
    CharacterController _char = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UserTryBeGunned(GameObject user) {
        Debug.Log("I want to be gunned");
        CharacterController _controller = user.GetComponent<CharacterController>();
        if(_controller != null) {
            _controller.enabled = false;
            _char = _controller;
        }
        user.transform.localPosition = Vector3.zero;    //TODO Here we can add animation to set in GUN
        _current = user;

    }

    public void PrepareFire() {
        if(_current != null) {
            Debug.Log("Send user to space");
        }
    }

    public void Jump() {
        if(_current != null) {
            Debug.Log("TODO Release user");
            Vector3 cpos = _current.transform.position;
            cpos.y += 10;
            _current.transform.position = cpos;
            _char.enabled = true;
            _current = null;
        }
    }
}

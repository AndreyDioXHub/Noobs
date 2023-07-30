using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorDetection : MonoBehaviour
{
    
    public UnityEvent<string> OnChangeFloor;
    public TagElement[] Tags;
    public CharacterController _controller;

    Vector3 startposition = Vector3.zero;
    // Start is called before the first frame update
    void Start() {
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update()  {
        
    }
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (_controller.isGrounded) {
            TagElement finded = Array.Find<TagElement>(Tags, tg => tg.TagName == hit.transform.tag);
            if(finded != null && finded.Holder != transform.parent) {
                transform.SetParent(finded.Holder, true);
                OnChangeFloor.Invoke(finded.TagName);
                finded.OnFoundAction?.Invoke();
            }
            
        }
    }

    public void Respawn() {
        transform.position = startposition;
    }

    [Serializable]
    public class TagElement {
        public string TagName = "Sea";
        public Transform Holder;
        public UnityEvent OnFoundAction;
    }
}

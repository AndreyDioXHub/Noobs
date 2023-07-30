using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerMovement : MonoBehaviour
{

    // Start is called before the first frame update
    public virtual void Start()
    {
        Init(PlayerController.Instance.Character);        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Init(CharacterController character)
    {

    }

    public virtual void Switch()
    {

    }

    public virtual void MoveValue(Vector2 inputMovement)
    {

    }

    public virtual void JumpValue()
    {

    }

    public virtual void RunValue(InputAction.CallbackContext value)
    {
        //Debug.Log("sprint");
        if (value.ReadValue<float>() == 1)
        {
            Debug.Log("sprint");
        }
        else
        {
            Debug.Log("walk");
        }
        

    }
}

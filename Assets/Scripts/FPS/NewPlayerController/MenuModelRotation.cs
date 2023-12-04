using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuModelRotation : MonoBehaviour
{
    [SerializeField]
    List<Transform> bodies;

    bool isDrag = false;
    Vector2 _axisMove = Vector2.zero;

    [SerializeField]
    float mouseSensitivity = 1f;
    [SerializeField]
    float rotY = 180;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDrag) {
            float mouseX = -Input.GetAxis("Mouse X");

            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            Quaternion localRotation = Quaternion.Euler(0, rotY, 0);
            
            foreach(Transform t in bodies) {
                t.rotation = localRotation;
            }
        }
    }


    private void OnMouseDown() {
        isDrag = true;
    }

    private void OnMouseUp() {
        isDrag = false;
    }

    private void OnMouseDrag() {
        
    }
    

    public void MouseDelta(InputAction.CallbackContext context) {
        _axisMove = context.ReadValue<Vector2>();
    }
}

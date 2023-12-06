using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuModelRotation : MonoBehaviour, IDragHandler
{
    [SerializeField]
    List<Transform> bodies;

    [SerializeField]
    float mouseSensitivity = 1f;
    [SerializeField]
    float rotY = 180;

    public void OnDrag(PointerEventData eventData)
    {
        float mouseX = -Input.GetAxis("Mouse X");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        Quaternion localRotation = Quaternion.Euler(0, rotY, 0);

        foreach (Transform t in bodies)
        {
            t.rotation = localRotation;
        }

    }
}

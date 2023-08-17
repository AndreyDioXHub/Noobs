using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field : MonoBehaviour
{
    [SerializeField]
    private List<Animator> _animators = new List<Animator>();

   void Start()
    {
        foreach(var animator in _animators)
        {
            animator.SetBool("Fall", true);
        }
    }

}

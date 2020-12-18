using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowElement : MonoBehaviour
{
    private Animator animator;

    public bool isOpen
    {
        set
        {
            animator.SetBool("Open", value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}

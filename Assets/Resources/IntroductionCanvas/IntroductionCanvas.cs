using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionCanvas : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public new bool enabled
    {
        set
        {
            animator.SetBool("Enable", value);
        }
    }

}

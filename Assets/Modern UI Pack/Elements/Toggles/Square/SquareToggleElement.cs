using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SquareToggleElement : Toggle
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        onValueChanged.AddListener(UpdateAnimator);
    }

    private void UpdateAnimator(bool value)
    {
        animator.SetBool("Value", value);
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        base.OnPointerEnter(pointerEventData);
        animator.SetBool("Highlighted", true);
    }

    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        base.OnPointerExit(pointerEventData);
        animator.SetBool("Highlighted", false);
    }

}

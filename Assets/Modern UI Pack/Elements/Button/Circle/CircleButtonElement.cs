using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleButtonElement : Button
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        animator.SetBool("Value", true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        animator.SetBool("Value", false);
    }
}

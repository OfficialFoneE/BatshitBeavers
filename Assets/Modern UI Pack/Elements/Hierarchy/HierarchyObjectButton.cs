using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UI.Elements.Hierarchy
{
    public class HierarchyObjectButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {

        private Animator animator;

        //private string PressedAnimatorKey = "isSelected";
        private string HighlighedAnimatorKey = "isHighlighted";
        //private string DisabledAnimatorKey = "isSelected";

        private void Awake()
        {
            animator = GetComponentInParent<Animator>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //animator.SetTrigger("Pressed");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetBool("isHighlighted", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetBool("isHighlighted", false);
        }
    }
}

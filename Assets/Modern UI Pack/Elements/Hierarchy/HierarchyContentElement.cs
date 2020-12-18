using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements.Hierarchy
{
    public class HierarchyContentElement : MonoBehaviour
    {
        private HierarchyBaseElement hierarchyBaseElement;

        private Canvas parentCanvas;

        private RectTransform rectTransform;

        public Rect rect;

        private bool isInitialised = false;

        private Vector2 topCornerAnchor = new Vector2(0, 1);

        private void Awake()
        {
            hierarchyBaseElement = GetComponentInParent<HierarchyBaseElement>();

            parentCanvas = GetComponentInParent<Canvas>();

            rectTransform = GetComponent<RectTransform>();

            rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

            isInitialised = true;
        }

        public void UpdateHierarchyObjectElementAnchors(List<HierarchyObjectElement> hierarchyObjectElements)
        {
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {
                hierarchyObjectElements[i].SetElementAnchors();
                UpdateHierarchyObjectElementAnchors(hierarchyObjectElements[i].hierarchyObjectElements);
            }
        }
        public void UpdateHierarchyObjectElementWidths(List<HierarchyObjectElement> hierarchyObjectElements)
        {
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {
                hierarchyObjectElements[i].SetElementWidth();
                UpdateHierarchyObjectElementWidths(hierarchyObjectElements[i].hierarchyObjectElements);
            }
        }

        public void RecaculateHierarchyObjectElements(List<HierarchyObjectElement> hierarchyObjectElements)
        {
            float heightOffset = 0;
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {

                hierarchyObjectElements[i].rectTransform.localPosition = new Vector2(rect.width / 2.0f, -hierarchyObjectElements[i].rectTransform.sizeDelta.y / 2.0f + heightOffset);
                heightOffset -= hierarchyObjectElements[i].rectTransform.sizeDelta.y;

                UpdateCompleteElement(hierarchyObjectElements[i]);
            }
        }

        //Updates this element and all its roots
        public void UpdateHierarchyBranch(HierarchyObjectElement hierarchyObjectElement)
        {
            HierarchyObjectElement childHierarchyObjectElement = hierarchyObjectElement;
            for (int i = 0; i < hierarchyObjectElement.depthIndex + 1; i++)
            {
                childHierarchyObjectElement.UpdateElementHeight();

                childHierarchyObjectElement.rectTransform.sizeDelta = new Vector2(rect.width, childHierarchyObjectElement.fullElementHeight);

                childHierarchyObjectElement = childHierarchyObjectElement.rootElement;
            }
        }

        //Update the main root elements of the hierarchy
        public void UpdateRootElements()
        {
            float heightOffset = 0;
            for (int i = 0; i < hierarchyBaseElement.parentElements.Count; i++)
            {
                hierarchyBaseElement.parentElements[i].rectTransform.localPosition = new Vector2(rect.width / 2.0f, -hierarchyBaseElement.parentElements[i].rectTransform.sizeDelta.y / 2.0f + heightOffset);
                hierarchyBaseElement.parentElements[i].labelRectTransform.localPosition = new Vector3(0, hierarchyBaseElement.parentElements[i].rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f);
                heightOffset -= hierarchyBaseElement.parentElements[i].rectTransform.sizeDelta.y;
            }
        }

        //Update all the children of a specific element
        public void UpdateChildrenElements(HierarchyObjectElement hierarchyObjectElement)
        {
            for (int i = 0; i < hierarchyObjectElement.hierarchyObjectElements.Count; i++)
            {
                UpdateCompleteElement(hierarchyObjectElement);
            }
        }

        //Places the element in the correct child spot inside its root element (Must be an element with a root element)
        public void UpdateElementPosition(HierarchyObjectElement hierarchyObjectElement)
        {
            float heightOffset = hierarchyObjectElement.rootElement.rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f;
            for (int i = 0; i < hierarchyObjectElement.siblingIndex; i++)
            {
                heightOffset -= hierarchyObjectElement.rootElement.hierarchyObjectElements[i].rectTransform.sizeDelta.y;
            }
            hierarchyObjectElement.rectTransform.localPosition = new Vector2(0, heightOffset - hierarchyObjectElement.rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f);
        }

        //Cannot call on an element that is a root element of the hierarchy
        private void UpdateCompleteElement(HierarchyObjectElement hierarchyObjectElement)
        {
            float heightOffset = hierarchyObjectElement.rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f;
            hierarchyObjectElement.labelRectTransform.localPosition = new Vector2(0, heightOffset);

            for (int i = 0; i < hierarchyObjectElement.hierarchyObjectElements.Count; i++)
            {
                hierarchyObjectElement.hierarchyObjectElements[i].rectTransform.localPosition = new Vector2(0, heightOffset - hierarchyObjectElement.hierarchyObjectElements[i].rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f);
                heightOffset -= hierarchyObjectElement.hierarchyObjectElements[i].rectTransform.sizeDelta.y;

                UpdateCompleteElement(hierarchyObjectElement.hierarchyObjectElements[i]);
            }
        }

        private void OnRectTransformDimensionsChange()
        {
            if (isInitialised)
            {
                rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);
                UpdateHierarchyObjectElementWidths(hierarchyBaseElement.parentElements);
                RecaculateHierarchyObjectElements(hierarchyBaseElement.parentElements);
                //UpdateHierarchyObjectElements();
            }
        }

    }
}

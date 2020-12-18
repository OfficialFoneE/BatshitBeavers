using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements.Hierarchy
{
    public class HierarchyContentElementV2 : MonoBehaviour
    {
        private HierarchyBaseElementV2 hierarchyBaseElement;

        private Canvas parentCanvas;

        private RectTransform rectTransform;

        private Rect _rect;
        public Rect rect
        {
            get
            {
                return _rect;
            }
        }

        private bool isInitialised = false;

        private Vector2 topCornerAnchor = new Vector2(0, 1);

        private void Awake()
        {
            hierarchyBaseElement = GetComponentInParent<HierarchyBaseElementV2>();

            parentCanvas = GetComponentInParent<Canvas>();

            rectTransform = GetComponent<RectTransform>();

            _rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

            isInitialised = true;
        }

        public void UpdateHierarchyObjectElementAnchors(List<HierarchyObjectElementV2> hierarchyObjectElements)
        {
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {
                hierarchyObjectElements[i].SetElementAnchors();
                UpdateHierarchyObjectElementAnchors(hierarchyObjectElements[i].hierarchyObjectElements);
            }
        }
        public void UpdateHierarchyObjectElementWidths(List<HierarchyObjectElementV2> hierarchyObjectElements)
        {
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {
                hierarchyObjectElements[i].SetElementWidth();
                UpdateHierarchyObjectElementWidths(hierarchyObjectElements[i].hierarchyObjectElements);
            }
        }

        public void RecaculateHierarchyObjectElements(List<HierarchyObjectElementV2> hierarchyObjectElements)
        {
            float heightOffset = 0;
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {

                hierarchyObjectElements[i].rectTransform.localPosition = new Vector2(_rect.width / 2.0f, -hierarchyObjectElements[i].rectTransform.sizeDelta.y / 2.0f + heightOffset);
                heightOffset -= hierarchyObjectElements[i].rectTransform.sizeDelta.y;

                UpdateCompleteElement(hierarchyObjectElements[i]);
            }
        }

        //Updates this element and all its roots
        public void UpdateHierarchyBranch(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            HierarchyObjectElementV2 childHierarchyObjectElement = hierarchyObjectElement;
            for (int i = 0; i < hierarchyObjectElement.depthIndex + 1; i++)
            {
                childHierarchyObjectElement.UpdateElementHeight();

                childHierarchyObjectElement.rectTransform.sizeDelta = new Vector2(_rect.width, childHierarchyObjectElement.fullElementHeight);

                childHierarchyObjectElement = childHierarchyObjectElement.rootElement;
            }
        }

        //Update the main root elements of the hierarchy
        public void UpdateRootElements()
        {
            float heightOffset = 0;
            for (int i = 0; i < hierarchyBaseElement.parentElements.Count; i++)
            {
                hierarchyBaseElement.parentElements[i].rectTransform.localPosition = new Vector2(_rect.width / 2.0f, -hierarchyBaseElement.parentElements[i].rectTransform.sizeDelta.y / 2.0f + heightOffset);
                hierarchyBaseElement.parentElements[i].labelRectTransform.localPosition = new Vector3(0, hierarchyBaseElement.parentElements[i].rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f);
                heightOffset -= hierarchyBaseElement.parentElements[i].rectTransform.sizeDelta.y;
            }
        }

        //Update all the children of a specific element
        public void UpdateChildrenElements(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            for (int i = 0; i < hierarchyObjectElement.hierarchyObjectElements.Count; i++)
            {
                UpdateCompleteElement(hierarchyObjectElement);
            }
        }

        //Places the element in the correct child spot inside its root element (Must be an element with a root element)
        public void UpdateElementPosition(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            float heightOffset = hierarchyObjectElement.rootElement.rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f;
            for (int i = 0; i < hierarchyObjectElement.siblingIndex; i++)
            {
                heightOffset -= hierarchyObjectElement.rootElement.hierarchyObjectElements[i].rectTransform.sizeDelta.y;
            }
            hierarchyObjectElement.rectTransform.localPosition = new Vector2(0, heightOffset - hierarchyObjectElement.rectTransform.sizeDelta.y / 2.0f - hierarchyBaseElement.hierarchyElementsHeight / 2.0f);
        }

        //Cannot call on an element that is a root element of the hierarchy
        private void UpdateCompleteElement(HierarchyObjectElementV2 hierarchyObjectElement)
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
                _rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);
                UpdateHierarchyObjectElementWidths(hierarchyBaseElement.parentElements);
                RecaculateHierarchyObjectElements(hierarchyBaseElement.parentElements);
                //UpdateHierarchyObjectElements();
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements.Hierarchy
{
    public abstract class HierarchyBaseElement : MonoBehaviour
    {

        public HierarchyLogicElement hierarchyLogicElement;

        private Transform _contentTransform;
        public Transform contentTransform
        {
            get
            {
                return _contentTransform;
            }

            protected set
            {
                _contentTransform = value;
            }
        }

        private HierarchyContentElement _hierarchyContentElement;
        public HierarchyContentElement hierarchyContentElement
        {
            get
            {
                return _hierarchyContentElement;
            }
        }

        public List<HierarchyObjectElement> parentElements = new List<HierarchyObjectElement>();
        public List<HierarchyObjectElement> selectedElements = new List<HierarchyObjectElement>();

        [SerializeField] protected bool allowMultiElementSelection = false;
        [SerializeField] protected bool canSelectDropdownElements = false;

        public delegate void OnElementSelected(HierarchyObjectElement hierarchyObjectElement);
        public event OnElementSelected onElementSelected;

        protected virtual void Awake()
        {
            onElementSelected += SelectElements;

            _hierarchyContentElement = GetComponentInChildren<HierarchyContentElement>();
        }

        private void SelectElements(HierarchyObjectElement hierarchyObjectElement)
        {
            if (!canSelectDropdownElements && hierarchyObjectElement.isDropdownElement)
            {
                return;
            }
            else
            {

            }

            if (allowMultiElementSelection)
            {
                //allow a multiselection click
            }
            else
            {
                for (int i = 0; i < selectedElements.Count; i++)
                {
                    selectedElements[i].isSelected = false;
                }
                selectedElements.Clear();

                hierarchyObjectElement.isSelected = true;

                selectedElements.Add(hierarchyObjectElement);
            }
        }

        public void CallEventOnElementSelected(HierarchyObjectElement hierarchyObjectElement)
        {
            if (onElementSelected != null)
            {
                onElementSelected.Invoke(hierarchyObjectElement);
            }
        }

        public void UpdateHierarchyLayoutElements()
        {
            hierarchyContentElement.RecaculateHierarchyObjectElements(parentElements);
        }

        public void EnableHierarchyBranch(HierarchyObjectElement hierarchyObjectElement)
        {
            StartCoroutine(UpdateHierarchyBranch(hierarchyObjectElement));
        }

        private IEnumerator UpdateHierarchyBranch(HierarchyObjectElement hierarchyObjectElement)
        {
            List<HierarchyObjectElement> expandingElements = new List<HierarchyObjectElement>();

            expandingElements.Add(hierarchyObjectElement);
            GetRootElement(hierarchyObjectElement);

            void GetRootElement(HierarchyObjectElement element)
            {
                if (element.rootElement != null)
                {
                    expandingElements.Add(element.rootElement);
                    GetRootElement(element.rootElement);
                }
            }

            for (int i = 0; i < expandingElements.Count; i++)
            {
                expandingElements[i].UpdateElementHeight();
            }

            for (int i = expandingElements.Count - 1; i >= 0; i--)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(expandingElements[i].rectTransform);
            }

            while (expandingElements.Count > 0)
            {
                for (int i = expandingElements.Count - 1; i >= 0; i--)
                {
                    var sizeDelta = expandingElements[i].rectTransform.sizeDelta;
                    expandingElements[i].rectTransform.sizeDelta = Vector2.Lerp(sizeDelta, expandingElements[i].desiredSize, _hierarchyListOpenSpeed);

                    if (Mathf.Abs(sizeDelta.y - expandingElements[i].desiredSize.y) < 0.5f)
                    {
                        expandingElements[i].rectTransform.sizeDelta = expandingElements[i].desiredSize;
                        expandingElements.RemoveAt(i);
                    }
                }

                UpdateHierarchyLayoutElements();

                yield return new WaitForFixedUpdate();
            }

        }





















        [SerializeField] protected float _hierarchyListObjectEnableDelay = 0.05f;
        public float hierarchyListObjectEnableDelay
        {
            get
            {
                return _hierarchyListObjectEnableDelay;
            }
        }
        [SerializeField] protected float _hierarchyListObjectDisableDelay = 0.05f;
        public float hierarchyListObjectDisableDelay
        {
            get
            {
                return _hierarchyListObjectDisableDelay;
            }
        }
        [SerializeField] protected float _hierarchyListOpenSpeed = 0.3f;
        public float hierarchyListOpenSpeed
        {
            get
            {
                return _hierarchyListOpenSpeed;
            }
        }
        [SerializeField] protected float _hierarchyListCloseSpeed = 0.3f;
        public float hierarchyListCloseSpeed
        {
            get
            {
                return _hierarchyListCloseSpeed;
            }
        }

        [SerializeField] protected float _hierarchyElementsHeight = 25;
        public float hierarchyElementsHeight
        {
            get
            {
                return _hierarchyElementsHeight;
            }
            set
            {
                _hierarchyElementsHeight = value;
            }
        }
    }
}

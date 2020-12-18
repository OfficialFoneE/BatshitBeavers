using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements.Hierarchy
{
    public class HierarchyListElement : HierarchyElement
    {

        private HierarchyBaseElement hierarchyBaseElement;

        private new GameObject gameObject;
        private Animator animator;
        private LayoutElement layoutElement;
        private Button button;

        private ArrowElement arrowElement;

        private List<HierarchyElement> hierarchyElements = new List<HierarchyElement>();

        private bool isExpanded = false;

        private Coroutine toggleHierarchyElementsCoroutine;
        private Coroutine expandListCoroutine;

        protected override void Awake()
        {
            hierarchyBaseElement = GetComponentInParent<HierarchyBaseElement>();

            rectTransform = GetComponent<RectTransform>();
            gameObject = rectTransform.gameObject;
            animator = GetComponent<Animator>();
            button = GetComponent<Button>();

            _labelRectTransform = (RectTransform)rectTransform.GetChild(0);

            arrowElement = GetComponentInChildren<ArrowElement>();
            elementTitle = _labelRectTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        protected override void Start()
        {
            button.onClick.AddListener(SetExpanded);

            //hierarchyElements.AddRange(GetComponentsInChildren<HierarchyElement>());

            _labelRectTransform.sizeDelta = new Vector2(_labelRectTransform.sizeDelta.x, hierarchyBaseElement.hierarchyElementsHeight);
        }

        private void SetExpanded()
        {
            isExpanded = !isExpanded;

            arrowElement.isOpen = isExpanded;

            StopAllCoroutines();

            toggleHierarchyElementsCoroutine = StartCoroutine(ToggleHierarchyElements(isExpanded));

            expandListCoroutine = StartCoroutine(ExpandList(isExpanded));

            //if (isExpanded)
            //{
            //    StartCoroutine(ExpandHierarchyElements(isExpanded));
            //}
        }

        private IEnumerator ToggleHierarchyElements(bool value)
        {

            if (value)
            {
                for (int i = 0; i < hierarchyElements.Count; i++)
                {
                    hierarchyElements[i].EnableHeirarchyElement(value);

                    //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (i + 1) * 31);

                    yield return new WaitForSecondsRealtime(hierarchyBaseElement.hierarchyListObjectEnableDelay);
                }
            }
            else
            {
                for (int i = hierarchyElements.Count - 1; i >= 0; i--)
                {
                    hierarchyElements[i].EnableHeirarchyElement(value);

                    //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (i + 1) * 31);

                    yield return new WaitForSecondsRealtime(hierarchyBaseElement.hierarchyListObjectDisableDelay);
                }
            }
        }

        private IEnumerator ExpandList(bool value)
        {
            float lerpSpeed;
            Vector2 finishedVector;
            if (value)
            {
                finishedVector = new Vector2(rectTransform.sizeDelta.x, (hierarchyElements.Count + 1) * hierarchyBaseElement.hierarchyElementsHeight);
                lerpSpeed = hierarchyBaseElement.hierarchyListOpenSpeed;
            }
            else
            {
                finishedVector = new Vector2(rectTransform.sizeDelta.x, hierarchyBaseElement.hierarchyElementsHeight);
                lerpSpeed = hierarchyBaseElement.hierarchyListCloseSpeed;
            }

            while (Mathf.Abs(rectTransform.sizeDelta.sqrMagnitude - finishedVector.sqrMagnitude) > 0.5f)
            {

                rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, finishedVector, lerpSpeed);

                yield return new WaitForFixedUpdate();
            }
        }

        public override void EnableHeirarchyElement(bool value)
        {
            animator.SetBool("Enable", value);
        }


        public void AddHeirarchyElement(HierarchyElement hierarchyElement)
        {
            hierarchyElements.Add(hierarchyElement);

            hierarchyElement.rectTransform.SetParent(rectTransform);
        }

        public void RemoveHeirarchyElement(HierarchyElement hierarchyElement)
        {
            //hierarchyElements.Add(hierarchyElement);
            //hierarchyElement.GetComponent<RectTransform>().SetParent(rectTransform);
        }

    }
}

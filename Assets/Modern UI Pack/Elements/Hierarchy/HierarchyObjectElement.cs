using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Elements.Hierarchy
{
    public class HierarchyObjectElement : HierarchyElement
    {
        private HierarchyBaseElement hierarchyBaseElement;

        private Animator animator;
        private Button button;

        private ArrowElement arrowElement;
        private GameObject arrowElementGameObject;
        private GameObject tabElementGameObject;

        public List<HierarchyObjectElement> hierarchyObjectElements = new List<HierarchyObjectElement>();

        private System.Type _bufferedHierarchyType;
        public System.Type bufferedHierarchyType
        {
            get
            {
                return _bufferedHierarchyType;
            }
            set
            {
                _bufferedHierarchyType = value;
            }
        }

        private BufferedHierarchyObjectElement _bufferedHierarchyObjectElement;
        public BufferedHierarchyObjectElement bufferedHierarchyObjectElement
        {
            get
            {
                return _bufferedHierarchyObjectElement;
            }
            set
            {
                _bufferedHierarchyObjectElement = value;
            }
        }

        private HierarchyObjectElement _rootElement;
        public HierarchyObjectElement rootElement
        {
            get
            {
                return _rootElement;
            }
        }

        private Vector2 _desiredSize;
        public Vector2 desiredSize
        {
            get
            {
                return _desiredSize;
            }
        }

        private float _elementHeight;
        public float elementHeight
        {
            set
            {
                _elementHeight = value;
            }
        }

        [SerializeField] private float _fullElementHeight;
        public float fullElementHeight
        {
            get
            {
                return _fullElementHeight;
            }
        }

        private int _siblingIndex;
        public int siblingIndex
        {
            get
            {
                return _siblingIndex;
            }
        }

        private int _depthIndex;
        public int depthIndex
        {
            get
            {
                return _depthIndex;
            }
        }

        private bool _isExpanded = false;
        public bool isExpanded
        {
            get
            {
                return _isExpanded;
            }
        }

        public bool isDropdownElement
        {
            get
            {
                return hierarchyObjectElements.Count == 0 ? false : true;
            }
        }

        public bool isSelected
        {
            get
            {
                return animator.GetBool("isSelected");
            }
            set
            {
                animator.SetBool("isSelected", value);
            }
        }

        public bool enableArrowElement
        {
            set
            {
                arrowElementGameObject.SetActive(value);
                tabElementGameObject.SetActive(!value);
            }
        }

        private static Vector2 topCornerAnchor = new Vector2(0, 1);

        protected override void Awake()
        {
            hierarchyBaseElement = GetComponentInParent<HierarchyBaseElement>();

            rectTransform = GetComponent<RectTransform>();
            animator = GetComponent<Animator>();
            button = GetComponentInChildren<Button>();

            _labelRectTransform = (RectTransform)rectTransform.GetChild(0);

            arrowElement = GetComponentInChildren<ArrowElement>(true);
            arrowElementGameObject = arrowElement.gameObject;
            tabElementGameObject = rectTransform.Find("Label").Find("HierarchyTabElement").gameObject;

            elementTitle = _labelRectTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            enableArrowElement = false;

            SetElementAnchors();
            SetElementWidth();
        }

        protected override void Start()
        {
            _fullElementHeight = _elementHeight;

            button.onClick.AddListener(OnClickCall);
        }

        private void OnClickCall()
        {
            if (hierarchyObjectElements.Count > 0)
            {
                _isExpanded = !_isExpanded;
                arrowElement.isOpen = _isExpanded;
                hierarchyBaseElement.EnableHierarchyBranch(this);
            }

            hierarchyBaseElement.CallEventOnElementSelected(this);
        }

        public void SetElementAnchors()
        {
            _labelRectTransform.anchorMax = topCornerAnchor;
            _labelRectTransform.anchorMin = topCornerAnchor;
            rectTransform.anchorMax = topCornerAnchor;
            rectTransform.anchorMin = topCornerAnchor;
        }
        public void SetElementWidth()
        {
            rectTransform.sizeDelta = _desiredSize = new Vector2(hierarchyBaseElement.hierarchyContentElement.rect.width, rectTransform.sizeDelta.y);
            _labelRectTransform.sizeDelta = new Vector2(hierarchyBaseElement.hierarchyContentElement.rect.width, hierarchyBaseElement.hierarchyElementsHeight);
        }

        public void UpdateElementHeight()
        {
            _fullElementHeight = _elementHeight;
            if (_isExpanded)
            {
                for (int i = 0; i < hierarchyObjectElements.Count; i++)
                {
                    hierarchyObjectElements[i].UpdateElementHeight();
                    _fullElementHeight += hierarchyObjectElements[i].fullElementHeight;
                }
            }

            _desiredSize.y = _fullElementHeight;
        }

        public override void EnableHeirarchyElement(bool value)
        {
            animator.SetBool("Enable", value);
        }

        public void ClearHeirarchyElements()
        {
            hierarchyObjectElements.Clear();
        }

        public void ResetHierarchyElement()
        {
            _isExpanded = false;
            UpdateElementHeight();
        }

        private void ChangeChildSiblingIndex(int childIndex, int newIndex)
        {
            newIndex = Mathf.Clamp(newIndex, 0, hierarchyObjectElements.Count);
            var hierarchyObjectElement = hierarchyObjectElements[childIndex];
            hierarchyObjectElements.RemoveAt(childIndex);
            hierarchyObjectElements.Insert(newIndex, hierarchyObjectElement);
            for (int i = 0; i < hierarchyObjectElements.Count; i++)
            {
                hierarchyObjectElements[i]._siblingIndex = i;
            }
        }

        public void SetSiblingIndex(int index)
        {
            _rootElement.ChangeChildSiblingIndex(_siblingIndex, index);
        }

        public void AddHeirarchyElement(HierarchyObjectElement hierarchyObjectElement)
        {
            hierarchyObjectElement._siblingIndex = hierarchyObjectElements.Count;
            hierarchyObjectElements.Add(hierarchyObjectElement);
            hierarchyObjectElement.rectTransform.SetParent(rectTransform);
            hierarchyObjectElement._rootElement = this;
            hierarchyObjectElement._depthIndex = _depthIndex + 1;

            //Debug.Log("ADDED A CHILD", hierarchyObjectElement);

            //UpdateElementHeight();
        }

        public void RemoveHeirarchyElement(HierarchyObjectElement hierarchyObjectElement)
        {
            hierarchyObjectElements.Remove(hierarchyObjectElement);
            hierarchyObjectElement._siblingIndex = 0;
            hierarchyObjectElement.rectTransform.SetParent(null);
            hierarchyObjectElement._rootElement = null;
            hierarchyObjectElement._depthIndex = 0;
            //hierarchyElement.GetComponent<RectTransform>().SetParent(rectTransform);
        }
    }
}

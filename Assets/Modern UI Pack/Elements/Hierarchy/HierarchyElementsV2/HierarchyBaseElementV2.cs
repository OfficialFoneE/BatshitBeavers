using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace UI.Elements.Hierarchy
{
    public class HierarchyBaseElementV2 : MonoBehaviour
    {
        [SerializeField] protected HierarchyAnimationValues hierarchyAnimationValues;

        public float hierarchyElementsHeight
        {
            get
            {
                return hierarchyAnimationValues.hierarchyElementsHeight;
            }
        }

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

        private HierarchyContentElementV2 _hierarchyContentElement;
        public HierarchyContentElementV2 hierarchyContentElement
        {
            get
            {
                return _hierarchyContentElement;
            }
        }

        protected List<HierarchyObjectElementV2> _parentElements = new List<HierarchyObjectElementV2>();
        public List<HierarchyObjectElementV2> parentElements
        {
            get
            {
                return _parentElements;
            }
        }

        protected List<HierarchyObjectElementV2> _selectedElements = new List<HierarchyObjectElementV2>();
        public List<HierarchyObjectElementV2> selectedElements
        {
            get
            {
                return _selectedElements;
            }
        }

        [SerializeField] protected bool allowMultiElementSelection = false;
        [SerializeField] protected bool canSelectDropdownElements = false;

        public delegate void OnElementSelected(HierarchyObjectElementV2 hierarchyObjectElement);
        public event OnElementSelected onElementSelected;

        protected virtual void Awake()
        {
            onElementSelected += SelectElements;

            _hierarchyContentElement = GetComponentInChildren<HierarchyContentElementV2>();

            bufferedHierarchyTabElements = new BufferedArray<BufferedRectTransform>(InstantiateBufferedHierarchyTabElement, BufferHierarchyTabElement);
        }

        private void SelectElements(HierarchyObjectElementV2 hierarchyObjectElement)
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
                for (int i = 0; i < _selectedElements.Count; i++)
                {
                    _selectedElements[i].isSelected = false;
                }
                _selectedElements.Clear();

                hierarchyObjectElement.isSelected = true;

                _selectedElements.Add(hierarchyObjectElement);
            }
        }

        public void CallEventOnElementSelected(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            if (onElementSelected != null)
            {
                onElementSelected.Invoke(hierarchyObjectElement);
            }
        }

        public void UpdateHierarchyLayoutElements()
        {
            hierarchyContentElement.RecaculateHierarchyObjectElements(_parentElements);
        }

        public void EnableHierarchyBranch(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            StartCoroutine(UpdateHierarchyBranch(hierarchyObjectElement));
        }

        private IEnumerator UpdateHierarchyBranch(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            List<HierarchyObjectElementV2> expandingElements = new List<HierarchyObjectElementV2>();

            expandingElements.Add(hierarchyObjectElement);
            GetRootElement(hierarchyObjectElement);

            void GetRootElement(HierarchyObjectElementV2 element)
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
                    expandingElements[i].rectTransform.sizeDelta = Vector2.Lerp(sizeDelta, expandingElements[i].desiredSize, hierarchyAnimationValues.hierarchyListOpenSpeed);

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

        #region HierarchyLogic
        private Dictionary<Type, HierarchyElementArchetypeBufferV2> hierarchyElementArchetypeBuffer = new Dictionary<Type, HierarchyElementArchetypeBufferV2>();

        #region Tabs
        private HierarchyElementArchetypeV2<BufferedRectTransform> tabHierarchyElementArchetype;
        private BufferedArray<BufferedRectTransform> bufferedHierarchyTabElements;
        private BufferedRectTransform InstantiateBufferedHierarchyTabElement()
        {
            return tabHierarchyElementArchetype.elementConstructor.Invoke(GameObject.Instantiate(tabHierarchyElementArchetype.elementPrefab, contentTransform));
        }
        private void BufferHierarchyTabElement(BufferedRectTransform bufferedRectTransform, bool value)
        {
            bufferedRectTransform.gameObject.SetActive(value);
        }
        #endregion

        public void SetTabElementArchetype(HierarchyElementArchetypeV2<BufferedRectTransform> hierarchyElementArchetype)
        {
            tabHierarchyElementArchetype = hierarchyElementArchetype;

        }
        public void AddElementArchetype<T>(HierarchyElementArchetypeV2<BufferedHierarchyObjectElementV2> hierarchyElementArchetype) where T : BufferedHierarchyObjectElementV2
        {
            hierarchyElementArchetypeBuffer.Add(typeof(T), new HierarchyElementArchetypeBufferV2(this, hierarchyElementArchetype));
        }

        #region NEEDED TO BE IMPLIMENTED AND TESTED!!!
        public void ClearHierarchy()
        {
            foreach (KeyValuePair<Type, HierarchyElementArchetypeBufferV2> keyValuePair in hierarchyElementArchetypeBuffer)
            {
                for (int i = 0; i < keyValuePair.Value.bufferedHierarchyObjectElements.bufferedCount; i++)
                {
                    keyValuePair.Value.bufferedHierarchyObjectElements[i].hierarchyObjectElement.ClearHeirarchyElements();
                }
                keyValuePair.Value.bufferedHierarchyObjectElements.UpdatePooledObjects(0);
            }
        }
        #endregion

        #region HierarchyCreationMethods
        //TODO: Return List<HierarchyObjectElementV2> which are then uses by the player to manipulate the data of the hierarchy
        public List<HierarchyObjectElementV2> CreateHierarchy(List<HierarchyObjectV2> hierarchyObjects)
        {

            List<HierarchyObjectElementV2> hierarchyObjectElements = new List<HierarchyObjectElementV2>();

            //ClearHierarchy();

            for (int i = 0; i < hierarchyObjects.Count; i++)
            {
                hierarchyObjectElements.Add(CreateHierarchy(hierarchyObjects[i]));
            }

            _parentElements.Clear();
            _parentElements.AddRange(hierarchyObjectElements);

            UpdateHierarchyLayoutElements();

            return hierarchyObjectElements;
        }

        private HierarchyObjectElementV2 CreateHierarchy(HierarchyObjectV2 hierarchyObject)
        {
            return CreateHierarchy(hierarchyObject, 0);
        }

        private HierarchyObjectElementV2 CreateHierarchy(HierarchyObjectV2 hierarchyObject, int depthOffset)
        {
            var bufferedHierarchyObject = hierarchyElementArchetypeBuffer[hierarchyObject.bufferedHierarchyType].bufferedHierarchyObjectElements.GetUnusedPooledObjects(1)[0];

            var hierarchyObjectElement = bufferedHierarchyObject.hierarchyObjectElement;

            hierarchyObjectElement.elementName = hierarchyObject.elementName;
            hierarchyObjectElement.bufferedHierarchyObjectElement = bufferedHierarchyObject;
            hierarchyObjectElement.bufferedHierarchyType = hierarchyObject.bufferedHierarchyType;

            if (hierarchyObject.childrenObjects.Count > 0)
            {
                //identifity the corresponding strucutre, initalise from that strucutre, 
                for (int i = 0; i < hierarchyObject.childrenObjects.Count; i++)
                {
                    var childHierarchyObject = CreateHierarchy(hierarchyObject.childrenObjects[i], depthOffset + 1);
                    hierarchyObjectElement.AddHeirarchyElement(childHierarchyObject);
                }

                hierarchyObjectElement.enableArrowElement = true;
            }
            else
            {
                hierarchyObjectElement.enableArrowElement = false;
            }

            hierarchyObjectElement.elementHeight = hierarchyAnimationValues.hierarchyElementsHeight;
            hierarchyObjectElement.AddTabElements(bufferedHierarchyTabElements.GetUnusedPooledObjects(depthOffset));
            //bufferedHierarchyObject.hierarchyObjectElement.ResetHierarchyElement();

            return hierarchyObjectElement;
        }
        #endregion

        //Some things have not been implimented fully!
        #region Add HierarchyObjects Methods
        public HierarchyObjectElementV2 AddHierarchyObjectWithoutUpdate(HierarchyObjectV2 hierarchyObject)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject);
            _parentElements.Add(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObject(HierarchyObjectV2 hierarchyObject)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject);
            hierarchyContentElement.UpdateRootElements();
            hierarchyContentElement.UpdateChildrenElements(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObjectWithoutUpdate(HierarchyObjectV2 hierarchyObject, int siblingIndex)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject);
            _parentElements.Insert(Mathf.Clamp(siblingIndex, 0, _parentElements.Count), hierarchyObjectElement);
            hierarchyObjectElement.rectTransform.SetSiblingIndex(siblingIndex);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObject(HierarchyObjectV2 hierarchyObject, int siblingIndex)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject, siblingIndex);
            hierarchyContentElement.UpdateRootElements();
            hierarchyContentElement.UpdateChildrenElements(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObjectWithoutUpdate(HierarchyObjectV2 hierarchyObject, HierarchyObjectElementV2 parent)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject, parent.depthIndex + 1);
            if (parent.hierarchyObjectElements.Count == 0)
            {
                parent.enableArrowElement = true;
            }
            parent.AddHeirarchyElement(hierarchyObjectElement);

            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObject(HierarchyObjectV2 hierarchyObject, HierarchyObjectElementV2 parent)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject, parent);

            //Cannot use this function as even though this places the Object correctly while being more optmised than updating all children elements
            //If the update is being is being optimised by calling AddHierarchyObjectWithoutUpdate, when finally calling AddHierarchyObject, the objects called WithoutUpdate would be missplaced
            //hierarchyBaseElement.hierarchyContentElement.UpdateElementPosition(hierarchyObjectElement);

            hierarchyContentElement.UpdateChildrenElements(parent);
            hierarchyContentElement.UpdateHierarchyBranch(parent);
            if (parent.isExpanded)
            {
                hierarchyContentElement.UpdateRootElements();
            }
            //Recalibrate only the content element of the parent heirarchy 
            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObjectWithoutUpdate(HierarchyObjectV2 hierarchyObject, HierarchyObjectElementV2 parent, int siblingIndex)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject, parent.depthIndex + 1);
            if (parent.hierarchyObjectElements.Count == 0)
            {
                parent.enableArrowElement = true;
            }
            parent.AddHeirarchyElement(hierarchyObjectElement);
            hierarchyObjectElement.SetSiblingIndex(siblingIndex);

            return hierarchyObjectElement;
        }
        public HierarchyObjectElementV2 AddHierarchyObject(HierarchyObjectV2 hierarchyObject, HierarchyObjectElementV2 parent, int siblingIndex)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject, parent, siblingIndex);

            //Cannot use this function as even though this places the Object correctly while being more optmised than updating all children elements
            //If the update is being is being optimised by calling AddHierarchyObjectWithoutUpdate, when finally calling AddHierarchyObject, the objects called WithoutUpdate would be missplaced
            //hierarchyBaseElement.hierarchyContentElement.UpdateElementPosition(hierarchyObjectElement);

            hierarchyContentElement.UpdateChildrenElements(parent);
            hierarchyContentElement.UpdateHierarchyBranch(parent);
            if (parent.isExpanded)
            {
                hierarchyContentElement.UpdateRootElements();
            }
            return hierarchyObjectElement;
        }
        #endregion

        //Some things have not been implimented fully!
        #region Remove HierarchyObjectV2 Methods
        public void RemoveHierarchyObjectWithoutUpdate(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            BufferHierarchyBranch(hierarchyObjectElement);
            if (hierarchyObjectElement.rootElement != null)
            {
                hierarchyObjectElement.rootElement.RemoveHeirarchyElement(hierarchyObjectElement);
            }
        }
        public void RemoveHierarchyObject(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            RemoveHierarchyObjectWithoutUpdate(hierarchyObjectElement);
            if (hierarchyObjectElement.rootElement != null)
            {
                hierarchyContentElement.UpdateHierarchyBranch(hierarchyObjectElement.rootElement);
            }
            else
            {
                Debug.LogError("THIS HAS NOT BEEN IMPLIMENTED");
                //remove the base hierarchyObject and then call hierarhcyEleme.UpdateRootElements
            }
        }
        private void BufferHierarchyBranch(HierarchyObjectElementV2 hierarchyObjectElement)
        {
            for (int i = hierarchyObjectElement.hierarchyObjectElements.Count - 1; i >= 0; i--)
            {
                BufferHierarchyBranch(hierarchyObjectElement.hierarchyObjectElements[0]);
                hierarchyObjectElement.RemoveHeirarchyElement(hierarchyObjectElement.hierarchyObjectElements[0]);
            }
            hierarchyElementArchetypeBuffer[hierarchyObjectElement.bufferedHierarchyType].bufferedHierarchyObjectElements.Buffer(hierarchyObjectElement.bufferedHierarchyObjectElement);
            bufferedHierarchyTabElements.BufferRange(hierarchyObjectElement.ClearTabElements());
        }
        #endregion

        #endregion
    }
}

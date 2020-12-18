using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace UI.Elements.Hierarchy
{
    public class HierarchyBaseComponent : MonoBehaviour
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

        private HierarchyContentElement _hierarchyContentElement;
        protected HierarchyContentElement hierarchyContentElement
        {
            get
            {
                return _hierarchyContentElement;
            }
        }

        protected List<HierarchyObjectElement> _parentElements = new List<HierarchyObjectElement>();
        public List<HierarchyObjectElement> parentElements
        {
            get
            {
                return _parentElements;
            }
        }

        protected List<HierarchyObjectElement> _selectedElements = new List<HierarchyObjectElement>();
        public List<HierarchyObjectElement> selectedElements
        {
            get
            {
                return _selectedElements;
            }
        }

        [SerializeField] protected bool allowMultiElementSelection = false;
        [SerializeField] protected bool canSelectDropdownElements = false;

        public delegate void OnElementSelected(HierarchyObjectElement hierarchyObjectElement);
        public event OnElementSelected onElementSelected;

        protected virtual void Awake()
        {
            onElementSelected += SelectElements;

            _hierarchyContentElement = GetComponentInChildren<HierarchyContentElement>();

            bufferedHierarchyTabElements = new BufferedArray<BufferedRectTransform>(InstantiateBufferedHierarchyTabElement, BufferHierarchyTabElement);
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
                for (int i = 0; i < _selectedElements.Count; i++)
                {
                    _selectedElements[i].isSelected = false;
                }
                _selectedElements.Clear();

                hierarchyObjectElement.isSelected = true;

                _selectedElements.Add(hierarchyObjectElement);
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
            hierarchyContentElement.RecaculateHierarchyObjectElements(_parentElements);
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
        private Dictionary<Type, HierarchyElementArchetypeBuffer> hierarchyElementArchetypeBuffer = new Dictionary<Type, HierarchyElementArchetypeBuffer>();

        #region Tabs
        private HierarchyElementArchetype<BufferedRectTransform> tabHierarchyElementArchetype;
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

        public void SetTabElementArchetype(HierarchyElementArchetype<BufferedRectTransform> hierarchyElementArchetype)
        {
            tabHierarchyElementArchetype = hierarchyElementArchetype;

        }
        public void AddElementArchetype<T>(HierarchyElementArchetype<BufferedHierarchyObjectElement> hierarchyElementArchetype) where T : BufferedHierarchyObjectElement
        {
            hierarchyElementArchetypeBuffer.Add(typeof(T), new HierarchyElementArchetypeBuffer(this, hierarchyElementArchetype));
        }

        #region NEEDED TO BE IMPLIMENTED AND TESTED!!!
        //public void ClearHierarchy()
        //{
        //    foreach (KeyValuePair<Type, BufferedHierarhcyElementStructure> keyValuePair in hierarchyElementArchetypeBuffer)
        //    {
        //        for (int i = 0; i < keyValuePair.Value.bufferedHierarchyObjectElements.bufferedCount; i++)
        //        {
        //            keyValuePair.Value.bufferedHierarchyObjectElements[i].hierarchyObjectElement.ClearHeirarchyElements();
        //        }
        //        keyValuePair.Value.bufferedHierarchyObjectElements.UpdatePooledObjects(0);
        //    }
        //}
        #endregion

        #region HierarchyCreationMethods
        //TODO: Return List<HierarchyObjectElement> which are then uses by the player to manipulate the data of the hierarchy
        public List<HierarchyObjectElement> CreateHierarchy(List<HierarchyObject> hierarchyObjects)
        {

            List<HierarchyObjectElement> hierarchyObjectElements = new List<HierarchyObjectElement>();

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

        private HierarchyObjectElement CreateHierarchy(HierarchyObject hierarchyObject)
        {
            return CreateHierarchy(hierarchyObject, 0);
        }

        private HierarchyObjectElement CreateHierarchy(HierarchyObject hierarchyObject, int depthOffset)
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
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject);
            _parentElements.Add(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObject(HierarchyObject hierarchyObject)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject);
            hierarchyContentElement.UpdateRootElements();
            hierarchyContentElement.UpdateChildrenElements(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject, int siblingIndex)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject);
            _parentElements.Insert(Mathf.Clamp(siblingIndex, 0, _parentElements.Count), hierarchyObjectElement);
            hierarchyObjectElement.rectTransform.SetSiblingIndex(siblingIndex);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObject(HierarchyObject hierarchyObject, int siblingIndex)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject, siblingIndex);
            hierarchyContentElement.UpdateRootElements();
            hierarchyContentElement.UpdateChildrenElements(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject, HierarchyObjectElement parent)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject, parent.depthIndex + 1);
            if (parent.hierarchyObjectElements.Count == 0)
            {
                parent.enableArrowElement = true;
            }
            parent.AddHeirarchyElement(hierarchyObjectElement);

            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObject(HierarchyObject hierarchyObject, HierarchyObjectElement parent)
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
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject, HierarchyObjectElement parent, int siblingIndex)
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
        public HierarchyObjectElement AddHierarchyObject(HierarchyObject hierarchyObject, HierarchyObjectElement parent, int siblingIndex)
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
        #region Remove HierarchyObject Methods
        public void RemoveHierarchyObjectWithoutUpdate(HierarchyObjectElement hierarchyObjectElement)
        {
            BufferHierarchyBranch(hierarchyObjectElement);
            if (hierarchyObjectElement.rootElement != null)
            {
                hierarchyObjectElement.rootElement.RemoveHeirarchyElement(hierarchyObjectElement);
            }
        }
        public void RemoveHierarchyObject(HierarchyObjectElement hierarchyObjectElement)
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
        private void BufferHierarchyBranch(HierarchyObjectElement hierarchyObjectElement)
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

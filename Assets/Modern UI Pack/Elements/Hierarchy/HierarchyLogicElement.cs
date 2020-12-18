using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

namespace UI.Elements.Hierarchy
{
    public class HierarchyLogicElement
    {

        private HierarchyBaseElement hierarchyBaseElement;

        public Transform contentTransform
        {
            get
            {
                return hierarchyBaseElement.contentTransform;
            } 
        }

        public Dictionary<Type, BufferedHierarhcyElementStructure> bufferedHierarhcyElementStructures = new Dictionary<Type, BufferedHierarhcyElementStructure>();

        private HierarchyElementCreationTemplate<BufferedRectTransform> hierarchyTabTemplate;
        public BufferedArray<BufferedRectTransform> bufferedHierarchyTabElements;
        private BufferedRectTransform InstantiateBufferedHierarchyTabElement()
        {
            return hierarchyTabTemplate.elementConstructor.Invoke(GameObject.Instantiate(hierarchyTabTemplate.elementPrefab, hierarchyBaseElement.contentTransform));
        }
        private void BufferHierarchyTabElement(BufferedRectTransform bufferedRectTransform, bool value)
        {
            bufferedRectTransform.gameObject.SetActive(value);
        }

        public HierarchyLogicElement(HierarchyElementCreationTemplate<BufferedRectTransform> hierarchyTabTemplate, HierarchyBaseElement hierarchyBaseElement)
        {
            this.hierarchyTabTemplate = hierarchyTabTemplate;

            bufferedHierarchyTabElements = new BufferedArray<BufferedRectTransform>(InstantiateBufferedHierarchyTabElement, BufferHierarchyTabElement);

            this.hierarchyBaseElement = hierarchyBaseElement;
        }

        public void CreateNewBufferedElementStructure<T>(HierarchyElementCreationTemplate<BufferedHierarchyObjectElement> hierarchyElementCreationTemplate) where T : BufferedHierarchyObjectElement
        {
            bufferedHierarhcyElementStructures.Add(typeof(T), new BufferedHierarhcyElementStructure(this, hierarchyElementCreationTemplate));
        }

        public void ClearHierarchy()
        {
            foreach (KeyValuePair<Type, BufferedHierarhcyElementStructure> keyValuePair in bufferedHierarhcyElementStructures)
            {
                for (int i = 0; i < keyValuePair.Value.bufferedHierarchyObjectElements.bufferedCount; i++)
                {
                    keyValuePair.Value.bufferedHierarchyObjectElements[i].hierarchyObjectElement.ClearHeirarchyElements();
                }
                keyValuePair.Value.bufferedHierarchyObjectElements.UpdatePooledObjects(0);
            }
        }

        //TODO: Return List<HierarchyObjectElement> which are then uses by the player to manipulate the data of the hierarchy
        public List<HierarchyObjectElement> CreateHierarchy(List<HierarchyObject> hierarchyObjects)
        {

            List<HierarchyObjectElement> hierarchyObjectElements = new List<HierarchyObjectElement>();

            ClearHierarchy();

            for (int i = 0; i < hierarchyObjects.Count; i++)
            {
                hierarchyObjectElements.Add(CreateHierarchy(hierarchyObjects[i]));
            }

            hierarchyBaseElement.parentElements.Clear();
            hierarchyBaseElement.parentElements.AddRange(hierarchyObjectElements);

            hierarchyBaseElement.UpdateHierarchyLayoutElements();

            return hierarchyObjectElements;
        }

        private HierarchyObjectElement CreateHierarchy(HierarchyObject hierarchyObject)
        {
            return CreateHierarchy(hierarchyObject, 0);
        }

        private HierarchyObjectElement CreateHierarchy(HierarchyObject hierarchyObject, int depthOffset)
        {
            var bufferedHierarchyObject = bufferedHierarhcyElementStructures[hierarchyObject.bufferedHierarchyType].bufferedHierarchyObjectElements.GetUnusedPooledObjects(1)[0];

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

            hierarchyObjectElement.elementHeight = hierarchyBaseElement.hierarchyElementsHeight;
            hierarchyObjectElement.AddTabElements(bufferedHierarchyTabElements.GetUnusedPooledObjects(depthOffset));
            //bufferedHierarchyObject.hierarchyObjectElement.ResetHierarchyElement();

            return hierarchyObjectElement;
        }

        //TODO: Return parent objects should be HierarchyObjectElement as they should alread be created by the hierarchy
        #region AddHierarchyObjectFunctions
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject);
            hierarchyBaseElement.parentElements.Add(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObject(HierarchyObject hierarchyObject)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject);
            hierarchyBaseElement.hierarchyContentElement.UpdateRootElements();
            hierarchyBaseElement.hierarchyContentElement.UpdateChildrenElements(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject, int siblingIndex)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject);
            hierarchyBaseElement.parentElements.Insert(Mathf.Clamp(siblingIndex, 0, hierarchyBaseElement.parentElements.Count), hierarchyObjectElement);
            hierarchyObjectElement.rectTransform.SetSiblingIndex(siblingIndex);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObject(HierarchyObject hierarchyObject, int siblingIndex)
        {
            var hierarchyObjectElement = AddHierarchyObjectWithoutUpdate(hierarchyObject, siblingIndex);
            hierarchyBaseElement.hierarchyContentElement.UpdateRootElements();
            hierarchyBaseElement.hierarchyContentElement.UpdateChildrenElements(hierarchyObjectElement);
            return hierarchyObjectElement;
        }
        public HierarchyObjectElement AddHierarchyObjectWithoutUpdate(HierarchyObject hierarchyObject, HierarchyObjectElement parent)
        {
            var hierarchyObjectElement = CreateHierarchy(hierarchyObject, parent.depthIndex + 1);
            if(parent.hierarchyObjectElements.Count == 0)
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

            hierarchyBaseElement.hierarchyContentElement.UpdateChildrenElements(parent);
            hierarchyBaseElement.hierarchyContentElement.UpdateHierarchyBranch(parent);
            if (parent.isExpanded)
            {
                hierarchyBaseElement.hierarchyContentElement.UpdateRootElements();
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

            hierarchyBaseElement.hierarchyContentElement.UpdateChildrenElements(parent);
            hierarchyBaseElement.hierarchyContentElement.UpdateHierarchyBranch(parent);
            if (parent.isExpanded)
            {
                hierarchyBaseElement.hierarchyContentElement.UpdateRootElements();
            }
            return hierarchyObjectElement;
        }
        #endregion

        public void RemoveHierarchyObjectWithoutUpdate(HierarchyObjectElement hierarchyObjectElement)
        {
            BufferHierarchyBranch(hierarchyObjectElement);
            if(hierarchyObjectElement.rootElement != null)
            {
                hierarchyObjectElement.rootElement.RemoveHeirarchyElement(hierarchyObjectElement);
            }
        }
        public void RemoveHierarchyObject(HierarchyObjectElement hierarchyObjectElement)
        {
            RemoveHierarchyObjectWithoutUpdate(hierarchyObjectElement);
            if (hierarchyObjectElement.rootElement != null)
            {
                hierarchyBaseElement.hierarchyContentElement.UpdateHierarchyBranch(hierarchyObjectElement.rootElement);
            } else
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
            bufferedHierarhcyElementStructures[hierarchyObjectElement.bufferedHierarchyType].bufferedHierarchyObjectElements.Buffer(hierarchyObjectElement.bufferedHierarchyObjectElement);
            bufferedHierarchyTabElements.BufferRange(hierarchyObjectElement.ClearTabElements());
        }
    }


    public class BufferedHierarhcyElementStructure
    {
        private HierarchyLogicElement hierarchyV2;
        private HierarchyElementCreationTemplate<BufferedHierarchyObjectElement> hierarchyObjectTemplate;
        public BufferedArray<BufferedHierarchyObjectElement> bufferedHierarchyObjectElements;

        private BufferedHierarchyObjectElement InstantiateBufferedObjectElement()
        {
            return hierarchyObjectTemplate.elementConstructor.Invoke(GameObject.Instantiate(hierarchyObjectTemplate.elementPrefab, hierarchyV2.contentTransform));
        }
        private void BufferBufferedObjectElement(BufferedHierarchyObjectElement bufferedObjectElement, bool value)
        {
            bufferedObjectElement.gameObject.SetActive(value);
        }

        public BufferedHierarhcyElementStructure(HierarchyLogicElement hierarchyV2, HierarchyElementCreationTemplate<BufferedHierarchyObjectElement> hierarchyObjectTemplate)
        {
            this.hierarchyV2 = hierarchyV2;
            this.hierarchyObjectTemplate = hierarchyObjectTemplate;
            bufferedHierarchyObjectElements = new BufferedArray<BufferedHierarchyObjectElement>(InstantiateBufferedObjectElement, BufferBufferedObjectElement);
        }
    }

    public class HierarchyElementCreationTemplate<T> where T : BufferedRectTransform
    {
        public GameObject elementPrefab;
        public Func<GameObject, T> elementConstructor;
        public HierarchyElementCreationTemplate(GameObject elementPrefab, Func<GameObject, T> elementConstructor)
        {
            this.elementPrefab = elementPrefab;
            this.elementConstructor = elementConstructor;
        }
    }

    public class HierarchyObject
    {
        public Type bufferedHierarchyType;

        public string elementName;
        public List<HierarchyObject> childrenObjects = new List<HierarchyObject>();

        public HierarchyObject(Type bufferedHierarchyType, string elementName)
        {
            //childrenObjects = new List<HierarchyObject>();
            this.bufferedHierarchyType = bufferedHierarchyType;
            this.elementName = elementName;
        }
    }

    //public struct HierarchyObject
    //{
    //    public Type bufferedHierarchyType;

    //    public string elementName;
    //    public List<HierarchyObject> childrenObjects;

    //    public BufferedHierarchyObjectElement bufferedHierarchyObject;

    //    public HierarchyObject(Type bufferedHierarchyType, string elementName)
    //    {
    //        bufferedHierarchyObject = null;
    //        childrenObjects = new List<HierarchyObject>();
    //        this.bufferedHierarchyType = bufferedHierarchyType;
    //        this.elementName = elementName;
    //    }
    //}

    public abstract class BufferedHierarchyObjectElement : BufferedRectTransform
    {
        public HierarchyObjectElement hierarchyObjectElement;

        public BufferedHierarchyObjectElement(GameObject gameObject) : base(gameObject)
        {
            hierarchyObjectElement = gameObject.GetComponent<HierarchyObjectElement>();
        }
    }
}


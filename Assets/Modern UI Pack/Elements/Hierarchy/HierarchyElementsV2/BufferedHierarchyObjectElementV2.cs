using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements.Hierarchy
{
    public abstract class BufferedHierarchyObjectElementV2 : BufferedRectTransform
    {
        public HierarchyObjectElementV2 hierarchyObjectElement;

        public BufferedHierarchyObjectElementV2(GameObject gameObject) : base(gameObject)
        {
            hierarchyObjectElement = gameObject.GetComponent<HierarchyObjectElementV2>();
        }
    }
}

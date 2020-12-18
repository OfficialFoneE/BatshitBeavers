using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI.Elements.Hierarchy
{
    public class HierarchyObjectV2
    {
        public Type bufferedHierarchyType;

        public string elementName;
        public List<HierarchyObjectV2> childrenObjects = new List<HierarchyObjectV2>();

        public HierarchyObjectV2(Type bufferedHierarchyType, string elementName)
        {
            //childrenObjects = new List<HierarchyObject>();
            this.bufferedHierarchyType = bufferedHierarchyType;
            this.elementName = elementName;
        }
    }
}

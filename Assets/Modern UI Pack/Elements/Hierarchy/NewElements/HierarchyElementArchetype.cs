using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI.Elements.Hierarchy
{
    public class HierarchyElementArchetype<T> where T : BufferedRectTransform
    {
        public GameObject elementPrefab;
        public Func<GameObject, T> elementConstructor;
        public Action<T, bool> elementBuffer;
        public HierarchyElementArchetype(GameObject elementPrefab, Func<GameObject, T> elementConstructor, Action<T, bool> elementBuffer)
        {
            this.elementPrefab = elementPrefab;
            this.elementConstructor = elementConstructor;
            this.elementBuffer = elementBuffer;
        }
    }
}

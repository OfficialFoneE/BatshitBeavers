using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements.Hierarchy
{
    public class HierarchyElementArchetypeBuffer
    {
        private HierarchyBaseComponent hierarchyBaseComponent;
        private HierarchyElementArchetype<BufferedHierarchyObjectElement> hierarchyElementArchetype;
        public BufferedArray<BufferedHierarchyObjectElement> bufferedHierarchyObjectElements;

        private BufferedHierarchyObjectElement InstantiateBufferedObjectElement()
        {
            return hierarchyElementArchetype.elementConstructor.Invoke(GameObject.Instantiate(hierarchyElementArchetype.elementPrefab, hierarchyBaseComponent.contentTransform));
        }
        private void BufferBufferedObjectElement(BufferedHierarchyObjectElement bufferedObjectElement, bool value)
        {
            hierarchyElementArchetype.elementBuffer.Invoke(bufferedObjectElement, value);
        }

        public HierarchyElementArchetypeBuffer(HierarchyBaseComponent hierarchyBaseComponent, HierarchyElementArchetype<BufferedHierarchyObjectElement> hierarchyElementArchetype)
        {
            this.hierarchyBaseComponent = hierarchyBaseComponent;
            this.hierarchyElementArchetype = hierarchyElementArchetype;
            bufferedHierarchyObjectElements = new BufferedArray<BufferedHierarchyObjectElement>(InstantiateBufferedObjectElement, BufferBufferedObjectElement);
        }
    }
}

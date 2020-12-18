using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements.Hierarchy
{
    public class HierarchyElementArchetypeBufferV2
    {
        private HierarchyBaseElementV2 hierarchyBaseElement;
        private HierarchyElementArchetypeV2<BufferedHierarchyObjectElementV2> hierarchyElementArchetype;
        public BufferedArray<BufferedHierarchyObjectElementV2> bufferedHierarchyObjectElements;

        private BufferedHierarchyObjectElementV2 InstantiateBufferedObjectElement()
        {
            return hierarchyElementArchetype.elementConstructor.Invoke(GameObject.Instantiate(hierarchyElementArchetype.elementPrefab, hierarchyBaseElement.contentTransform));
        }
        private void BufferBufferedObjectElement(BufferedHierarchyObjectElementV2 bufferedObjectElement, bool value)
        {
            hierarchyElementArchetype.elementBuffer.Invoke(bufferedObjectElement, value);
        }

        public HierarchyElementArchetypeBufferV2(HierarchyBaseElementV2 hierarchyBaseElement, HierarchyElementArchetypeV2<BufferedHierarchyObjectElementV2> hierarchyElementArchetype)
        {
            this.hierarchyBaseElement = hierarchyBaseElement;
            this.hierarchyElementArchetype = hierarchyElementArchetype;
            bufferedHierarchyObjectElements = new BufferedArray<BufferedHierarchyObjectElementV2>(InstantiateBufferedObjectElement, BufferBufferedObjectElement);
        }
    }
}

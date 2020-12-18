using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Toggle Group", 32)]
    [DisallowMultipleComponent]
    /// <summary>
    /// A component that represents a group of UI.Toggles.
    /// </summary>
    /// <remarks>
    /// When using a group reference the group from a UI.Toggle. Only one member of a group can be active at a time.
    /// </remarks>
    public class CustomToggleGroup : ToggleGroup
    {
        
        public void ClearToggles()
        {
            m_Toggles.Clear();
        }
    }
}
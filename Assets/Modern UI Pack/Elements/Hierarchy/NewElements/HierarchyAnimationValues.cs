using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HierarchyAnimationValues", menuName = "ScriptableObjects/HierarchyAnimationValues", order = 2)]
public class HierarchyAnimationValues : ScriptableObject
{
    public float hierarchyListObjectEnableDelay = 0.05f;

    public float hierarchyListObjectDisableDelay = 0.05f;

    public float hierarchyListOpenSpeed = 0.3f;

    public float hierarchyListCloseSpeed = 0.3f;

    public float hierarchyElementsHeight = 25;
}

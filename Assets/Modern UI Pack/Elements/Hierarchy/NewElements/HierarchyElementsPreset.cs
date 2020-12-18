using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HierarchyElementsPreset", menuName = "ScriptableObjects/HierarchyElementsPreset", order = 4)]
public class HierarchyElementsPreset : ScriptableObject
{

    public GameObject tabElementPrefab;

    public List<GameObject> hierarchyElementPrefabs = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < hierarchyElementPrefabs.Count; i++)
        {
            if(hierarchyElementPrefabs[i].GetComponent<HierarchyElement>() == null)
            {
                hierarchyElementPrefabs.RemoveAt(i);
            }
        }
    }
}

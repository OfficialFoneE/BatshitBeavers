using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReferences : MonoBehaviour
{
    [SerializeField] private GameObject _rangeTick;
    public static GameObject rangeTick;

    public static GameObject multiselectDropdownItem;
    [SerializeField] private GameObject _multiselectDropdownItem;

    public static GameObject standardDropdownItem;
    [SerializeField] private GameObject _standardDropdownItem;

    [Header("Hierarchy")]
    //[SerializeField] private GameObject _hierarchyListElement;
    //public static GameObject hierarchyListElement;

    [SerializeField] private GameObject _hierarchyObjectElement;
    public static GameObject hierarchyObjectElement;

    //[SerializeField] private GameObject _hierarchyArrowElement;
    //public static GameObject hierarchyArrowElement;

    [SerializeField] private GameObject _hierarchyTabElement;
    public static GameObject hierarchyTabElement;


    [Header("Scale Bar")]
    [SerializeField] private GameObject _scaleBarTextElement;
    public static GameObject scaleBarTextElement;

    [SerializeField] private GameObject _scaleBarImageElement;
    public static GameObject scaleBarImageElement;

    private void Awake()
    {
        Debug.LogWarning("NEED TO STREAMLINE ASSETS");

        rangeTick = _rangeTick;

        multiselectDropdownItem = _multiselectDropdownItem;

        standardDropdownItem = _standardDropdownItem;

        //hierarchyListElement = _hierarchyListElement;
        hierarchyObjectElement = _hierarchyObjectElement;
        //hierarchyArrowElement = _hierarchyArrowElement;
        hierarchyTabElement = _hierarchyTabElement;

        scaleBarTextElement = _scaleBarTextElement;
        scaleBarImageElement = _scaleBarImageElement;
    }

}

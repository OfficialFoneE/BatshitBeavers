using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyLayoutElement : MonoBehaviour
{

    private Canvas parentCanvas;

    private RectTransform rectTransform;
    private List<RectTransform> rectTransforms = new List<RectTransform>();

    private HierarchyLayoutElement rootElement;
    private List<HierarchyLayoutElement> hierarchyLayoutElements = new List<HierarchyLayoutElement>();

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {

    }

    public void UpdateTransformElements()
    {

        if (rectTransforms.Count > 0)
        {
            Rect rect = rectTransform.rect;//RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

            var vector2Zero = new Vector2(0, 1);
            for (int i = 0; i < rectTransforms.Count; i++)
            {
                rectTransforms[i].anchorMax = vector2Zero;
                rectTransforms[i].anchorMin = vector2Zero;
            }

            Debug.Log(gameObject.name + " has width of " + rect.width + " with " + rectTransforms.Count + " children.");

            float centerOffset = rect.width / 2.0f;
            float heightOffset = rectTransforms[0].rect.width / 2;
            for (int i = 0; i < rectTransforms.Count; i++)
            {
                rectTransforms[i].localPosition = new Vector3(centerOffset, heightOffset /*+ rectTransforms[i].rect.width / 2.0f*/);
                heightOffset -= rectTransforms[i].sizeDelta.y;
            }

            for (int i = 0; i < rectTransforms.Count; i++)
            {
                rectTransforms[i].sizeDelta = new Vector2(rect.width, rectTransforms[i].sizeDelta.y);
            }

            //for (int i = 0; i < rectTransforms.Count; i++)
            //{
            //    LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            //}
        }
    }

    public void UpdateChildList()
    {
        rectTransforms.Clear();

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            rectTransforms.Add((RectTransform)rectTransform.GetChild(i));
        }
    }

    public void OnTransformChildrenChanged()
    {

    }
}

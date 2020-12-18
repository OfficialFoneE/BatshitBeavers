using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalLayoutElement : MonoBehaviour
{

    private Canvas parentCanvas;

    private RectTransform rectTransform;

    private Rect rect;

    private List<RectTransform> rectTransforms = new List<RectTransform>();

    [SerializeField] private bool forceChildWidth = true;
    [SerializeField] private bool forceChildHeight = false;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

        Invoke("OnTransformChildrenChanged", 0.5f);
    }

    public void UpdateTransformElements()
    {
        rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

        var vector2Zero = new Vector2(0, 1);
        for (int i = 0; i < rectTransforms.Count; i++)
        {
            rectTransforms[i].anchorMax = vector2Zero;
            rectTransforms[i].anchorMin = vector2Zero;
        }

        Debug.Log(rect.width, this);

        float centerOffset = rect.width / 2.0f;
        float heightOffset = 0;
        for (int i = 0; i < rectTransforms.Count; i++)
        {
            rectTransforms[i].localPosition = new Vector3(0, heightOffset /*+ rectTransforms[i].rect.width / 2.0f*/);
            heightOffset -= rectTransforms[i].sizeDelta.y;
        }


        for (int i = 0; i < rectTransforms.Count; i++)
        {
            rectTransforms[i].sizeDelta = new Vector2(rect.width, rectTransforms[i].sizeDelta.y);
        }

        for (int i = 0; i < rectTransforms.Count; i++)
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }


    public void OnTransformChildrenChanged()
    {
        rectTransforms.Clear();

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            rectTransforms.Add((RectTransform)rectTransform.GetChild(i));
        }

        UpdateTransformElements();
    }

    //private void OnRectTransformDimensionsChange()
    //{
    //    //rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);
    //    //UpdateTransformElements();
    //}
}

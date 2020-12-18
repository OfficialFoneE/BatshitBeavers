using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurleyBracket : MonoBehaviour
{

    private Canvas canvas;
    private RectTransform rectTransform;

    //private Texture left;
    //private Texture middle;
    //private Texture right;
    //private Texture lines;

    private Image leftImage;
    private Image middleLeftImage;
    private Image middleImage;
    private Image middleRightImage;
    private Image rightImage;


    private RectTransform leftImageTransform;
    private RectTransform middleLeftImageTransform;
    private RectTransform middleImageTransform;
    private RectTransform middleRightImageTransform;
    private RectTransform rightImageTransform;

    private Rect rect;

    private bool hasInitiated = false;
    private bool isEditing = false;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        rectTransform = GetComponent<RectTransform>();

        leftImage = rectTransform.Find("leftImage").GetComponent<Image>();
        leftImageTransform = leftImage.GetComponent<RectTransform>();
        middleLeftImage = rectTransform.Find("middleLeftImage").GetComponent<Image>();
        middleLeftImageTransform = middleLeftImage.GetComponent<RectTransform>();
        middleLeftImage = rectTransform.Find("middleImage").GetComponent<Image>();
        middleImageTransform = middleLeftImage.GetComponent<RectTransform>();
        middleRightImage = rectTransform.Find("middleRightImage").GetComponent<Image>();
        middleRightImageTransform = middleRightImage.GetComponent<RectTransform>();
        rightImage = rectTransform.Find("rightImage").GetComponent<Image>();
        rightImageTransform = rightImage.GetComponent<RectTransform>();

        hasInitiated = true;
    }

    private void Start()
    {
        UpdateBracket();
    }

    private void OnRectTransformDimensionsChange()
    {
        //if (hasInitiated && !isEditing)
            UpdateBracket();
    }

    private void UpdateBracket()
    {
        isEditing = true;

        GetDimensions();

        float width = rect.width;
        float height = rect.height;

        if (width <= 0.01f && height <= 0.01f) { return;  };
        //var middleWidth = middleImageTransform.rect.width;

        //var middleWidthNormalized = 1.0f / (width / middleWidth);

        Vector2 zero = Vector2.zero;

        float heightWidthRatio = 120.0f / 64.0f;
        //the width = heightWidthRatio * height
        float widthOfMiddle = height * heightWidthRatio;
        float widthOfMiddleNormalized = 1.0f / (width / widthOfMiddle);

        middleImageTransform.anchorMin = new Vector2(0.5f - widthOfMiddleNormalized / 2.0f, 0);
        middleImageTransform.anchorMax = new Vector2(0.5f + widthOfMiddleNormalized / 2.0f, 1);
        middleImageTransform.offsetMax = zero;
        middleImageTransform.offsetMin = zero;

        float sideHeightWidthRatio = 60.0f / 64.0f;
        //the width = heightWidthRatio * height
        float widthOfSide = height * sideHeightWidthRatio;
        float widthOfSideNormalized = 1.0f / (width / widthOfSide);

        leftImageTransform.anchorMin = zero;
        leftImageTransform.anchorMax = new Vector2(widthOfSideNormalized, 1);
        leftImageTransform.offsetMax = zero;
        leftImageTransform.offsetMin = zero;

        rightImageTransform.anchorMin = new Vector2(1 - widthOfSideNormalized, 0);
        rightImageTransform.anchorMax = new Vector2(1, 1);
        rightImageTransform.offsetMax = zero;
        rightImageTransform.offsetMin = zero;

        middleLeftImageTransform.anchorMin = new Vector2(widthOfSideNormalized, 0);
        middleLeftImageTransform.anchorMax = new Vector2(0.5f - widthOfMiddleNormalized / 2.0f, 1);
        middleLeftImageTransform.offsetMax = zero;
        middleLeftImageTransform.offsetMin = zero;

        middleRightImageTransform.anchorMin = new Vector2(0.5f + widthOfMiddleNormalized / 2.0f, 0);
        middleRightImageTransform.anchorMax = new Vector2(1 - widthOfSideNormalized, 1);
        middleRightImageTransform.offsetMax = zero;
        middleRightImageTransform.offsetMin = zero;

        isEditing = false;
    }

    private void GetDimensions()
    {
        rect = RectTransformUtility.PixelAdjustRect(rectTransform, canvas);
    }
}
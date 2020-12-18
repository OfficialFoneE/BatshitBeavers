using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollElement : MonoBehaviour
{
    private Canvas parentCanvas;

    private RectTransform rectTransform;

    private RectTransform contentTransform;

    private Rect contentRect;

    private Scrollbar verticalScrollBar;

    private List<RectTransform> childObjects;

    private Rect prefabRect;


    private GameObject prefabObject;
    //private int visible 

    private Rect rect;

    private bool isInitiated = false;

    private void OnScroll()
    {



        for (int i = 0; i < childObjects.Count; i++)
        {




            //verticalScrollBar.value;


        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if(isInitiated)
        {
            rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);
        }
    }



}

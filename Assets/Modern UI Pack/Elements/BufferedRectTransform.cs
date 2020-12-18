using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferedRectTransform : BufferedObject
{
    public RectTransform rectTransform;

    public BufferedRectTransform(GameObject go) : base(go)
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class HierarchyElement : MonoBehaviour
{

    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            return _rectTransform;
        }
        protected set
        {
            _rectTransform = value;
        }
    }

    protected RectTransform _labelRectTransform;
    public RectTransform labelRectTransform
    {
        get
        {
            return _labelRectTransform;
        }
    }

    protected TextMeshProUGUI elementTitle;
    public string elementName
    {
        set
        {
            elementTitle.text = value;
        }
    }

    protected List<BufferedRectTransform> bufferedTabElements = new List<BufferedRectTransform>(); 

    protected abstract void Awake();

    protected abstract void Start();











    public void AddTabElement(BufferedRectTransform bufferedRectTransform)
    {
        bufferedRectTransform.rectTransform.SetParent(labelRectTransform);
        bufferedRectTransform.rectTransform.SetAsFirstSibling();
        bufferedTabElements.Add(bufferedRectTransform);
    }

    public void AddTabElements(List<BufferedRectTransform> bufferedRectTransforms)
    {
        for (int i = 0; i < bufferedRectTransforms.Count; i++)
        {
            bufferedRectTransforms[i].rectTransform.SetParent(labelRectTransform);
            bufferedRectTransforms[i].rectTransform.SetAsFirstSibling();
        }
        bufferedTabElements.AddRange(bufferedRectTransforms);
    }

    public BufferedRectTransform RemoveTabElement()
    {
        var bufferedTabElement = bufferedTabElements[0];
        bufferedTabElements.RemoveAt(0);
        return bufferedTabElement;
    }

    public BufferedRectTransform[] ClearTabElements()
    {
        BufferedRectTransform[] outputBufferedRectTransforms = new BufferedRectTransform[bufferedTabElements.Count];
        bufferedTabElements.CopyTo(outputBufferedRectTransforms);
        bufferedTabElements.Clear();
        return outputBufferedRectTransforms;
    }

    public abstract void EnableHeirarchyElement(bool value);
}

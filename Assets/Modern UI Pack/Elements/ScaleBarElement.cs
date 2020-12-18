using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleBarElement : MonoBehaviour
{

    private Canvas parentCanvas;

    private RectTransform rectTransform;

    private BufferedArray<BufferedTextObject> bufferedTextObjects;
    private class BufferedTextObject : BufferedRectTransform
    {
        public TextMeshProUGUI text;
        public LayoutElement layoutElement;
        public BufferedTextObject(GameObject gameObject) : base(gameObject)
        {
            text = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            layoutElement = gameObject.GetComponent<LayoutElement>();
        }
    }
    private BufferedTextObject InstantiateBufferedTextObject()
    {
        return new BufferedTextObject(Instantiate(UIReferences.scaleBarTextElement, textGroupTransform));
    }
    private void BufferTextObject(BufferedTextObject bufferedTextObject, bool value)
    {
        bufferedTextObject.gameObject.SetActive(value);
    }

    private BufferedArray<BufferedImageObject> bufferedImageObjects;
    private class BufferedImageObject : BufferedRectTransform
    {
        public Image image;
        public LayoutElement layoutElement;
        public BufferedImageObject(GameObject gameObject) : base(gameObject)
        {
            image = gameObject.GetComponentInChildren<Image>();
            layoutElement = gameObject.GetComponent<LayoutElement>();
        }
    }
    private BufferedImageObject InstantiateBufferedImageObject()
    {
        return new BufferedImageObject(Instantiate(UIReferences.scaleBarImageElement, imageGroupTransform));
    }
    private void BufferImageObject(BufferedImageObject bufferedImageObject, bool value)
    {
        bufferedImageObject.gameObject.SetActive(value);
    }

    private RectTransform textGroupTransform;
    private HorizontalLayoutGroup textLayoutGroup;

    private RectTransform imageGroupTransform;
    private HorizontalLayoutGroup imageLayoutGroup;

    private Rect rect;

    [SerializeField] private Color barPrimaryColor = new Color(147, 215, 240, 255);
    [SerializeField] private Color barSecondaryColor;

    private int intervals = 0;

    private string measurementUnit = "Kilometers";

    private float maximumUnit = 2;

    private bool isInitialised = false;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        rectTransform = GetComponent<RectTransform>();

        bufferedTextObjects = new BufferedArray<BufferedTextObject>(InstantiateBufferedTextObject, BufferTextObject);
        bufferedImageObjects = new BufferedArray<BufferedImageObject>(InstantiateBufferedImageObject, BufferImageObject);

        textGroupTransform = transform.GetChild(0).GetComponent<RectTransform>();
        textLayoutGroup = textGroupTransform.GetComponent<HorizontalLayoutGroup>();

        imageGroupTransform = transform.GetChild(1).GetComponent<RectTransform>();
        imageLayoutGroup = imageGroupTransform.GetComponent<HorizontalLayoutGroup>();

        isInitialised = true;
    }

    private void Start()
    {
        intervals = 5;
        OnRectTransformDimensionsChange();
    }

    public void UpdateScaleBarElement()
    {
        bufferedTextObjects.UpdatePooledObjects(intervals + 1);

        //bufferedTextObjects[0].text.alignment = TextAlignmentOptions.MidlineLeft;
        bufferedTextObjects[0].text.text = maximumUnit.ToString() + " " + measurementUnit;

        //bufferedTextObjects[bufferedTextObjects.bufferedCount - 1].text.alignment = TextAlignmentOptions.MidlineRight;

        float interval = maximumUnit / intervals;

        for (int i = 1; i < bufferedTextObjects.bufferedCount - 1; i++)
        {
            bufferedTextObjects[i].text.text = (maximumUnit - (interval * i)).ToString();
        }

        bufferedImageObjects.UpdatePooledObjects(intervals + 2);

        float cornerWidth = (rect.width / (intervals + 1)) / 2.0f;

        bufferedImageObjects[0].layoutElement.minWidth = cornerWidth;
        bufferedImageObjects[0].layoutElement.preferredWidth = cornerWidth;
        bufferedImageObjects[0].image.color = Color.clear;

        bufferedImageObjects[bufferedImageObjects.bufferedCount - 1].layoutElement.minWidth = cornerWidth;
        bufferedImageObjects[bufferedImageObjects.bufferedCount - 1].layoutElement.preferredWidth = cornerWidth;
        bufferedImageObjects[bufferedImageObjects.bufferedCount - 1].image.color = Color.clear;

        for (int i = 1; i < bufferedImageObjects.bufferedCount - 1; i+=2)
        {
            bufferedImageObjects[i].layoutElement.minWidth = 1.0f;
            bufferedImageObjects[i].layoutElement.preferredWidth = 100.0f;
            bufferedImageObjects[i].image.color = barPrimaryColor;
        }

        for (int i = 2; i < bufferedImageObjects.bufferedCount - 1; i += 2)
        {
            bufferedImageObjects[i].layoutElement.minWidth = 1.0f;
            bufferedImageObjects[i].layoutElement.preferredWidth = 100.0f;
            bufferedImageObjects[i].image.color = barSecondaryColor;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if(isInitialised)
        {
            rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);
            UpdateScaleBarElement();
        }
    }
}

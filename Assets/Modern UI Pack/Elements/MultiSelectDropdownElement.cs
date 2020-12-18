using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Text;

public class MultiSelectDropdownElement : MonoBehaviour
{

    private Canvas parentCanvas;

    private Canvas canvas;
    private RectTransform rectTransform;
    private Animator dropdownAnimator;
    private Button dropDownButton;

    private ScrollRect scrollRect;
    private RectTransform contentTransform;
    private VerticalLayoutGroup itemListLayoutGroup;
    private RectTransform scrollBarRectTransform;

    private TextMeshProUGUI dropdownNameText;
    public string dropdownName
    {
        get
        {
            return dropdownNameText.text;
        }
    }

    private Rect rect;

    [SerializeField] private bool defaultToggleValue = true;

    public AnimationType animationType;

    private List<MultiselectItem> dropdownItems = new List<MultiselectItem>();

    public int selectedValues = 0;

    private bool isOn;

    public class OnValueChangedEvent : UnityEvent<int> { }
    public OnValueChangedEvent OnValueChanged = new OnValueChangedEvent();

    public enum AnimationType
    {
        FADING,
        SLIDING,
        STYLISH
    }

    private class MultiselectItem
    {
        public RectTransform rectTransform;

        public string itemName
        {
            get
            {
                return itemNameText.text;
            }
            set
            {
                itemNameText.text = value;
            }
        }

        public TextMeshProUGUI itemNameText;

        public Toggle toggle;

        public bool isOn
        {
            get
            {
                return toggle.isOn;
            }
        }

        public MultiselectItem(GameObject go)
        {
            rectTransform = go.GetComponent<RectTransform>();
            itemNameText = go.GetComponentInChildren<TextMeshProUGUI>();
            toggle = go.GetComponent<Toggle>();
        }
    }

    private bool isInitialized = false;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        canvas = GetComponentInChildren<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        dropdownAnimator = GetComponent<Animator>();
        dropDownButton = GetComponent<Button>();

        scrollRect = GetComponentInChildren<ScrollRect>();
        contentTransform = scrollRect.content;
        itemListLayoutGroup = scrollRect.content.GetComponent<VerticalLayoutGroup>();
        scrollBarRectTransform = scrollRect.verticalScrollbar.GetComponent<RectTransform>();

        dropdownNameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {

        dropDownButton.onClick.AddListener(Animate);

        //for (int i = 0; i < dropdownItems.Count; ++i)
        //{
        //    GameObject go = Instantiate(UIReferences.multiselectDropdownItem, contentTransform);

        //    go.GetComponentInChildren<TextMeshProUGUI>().text = dropdownItems[i].itemName;

        //    dropdownItems[i].rectTransform = go.GetComponent<RectTransform>();

        //    dropdownItems[i].toggle = go.GetComponent<Toggle>();

        //    //Notice: Toggle Invoke must come befor the ChangeDropdownInfo Invoke
        //    //dropdownItems[i].toggle.onValueChanged.AddListener(dropdownItems[i].toggleEvents.Invoke);
        //    var dropDownItem = dropdownItems[i];
        //    dropdownItems[i].toggle.onValueChanged.AddListener(delegate { ChangeDropdownInfo(dropdownItems.IndexOf(dropDownItem)); });
        //}

        //UpdateDropdownElements();

        canvas.sortingOrder = (int)rectTransform.position.y;

        isInitialized = true;

        Invoke("UpdateSortingOrder", 1f);
    }

    private void UpdateSortingOrder()
    {

        canvas.sortingOrder = (int)transform.position.y;
    }

    public void AddItemElement(string itemName)
    {
        var multiselectItem = new MultiselectItem(Instantiate(UIReferences.multiselectDropdownItem, contentTransform));
        multiselectItem.itemName = itemName;

        dropdownItems.Add(multiselectItem);

        var dropDownItem = multiselectItem;
        multiselectItem.toggle.onValueChanged.AddListener(delegate { ChangeDropdownInfo(dropdownItems.IndexOf(dropDownItem)); });

        multiselectItem.toggle.isOn = defaultToggleValue;

        ChangeDropdownInfo(dropdownItems.IndexOf(dropDownItem));
    }

    public void ChangeDropdownInfo(int itemIndex)
    {

        if (dropdownItems[itemIndex].isOn)
        {
            selectedValues |= 1 << itemIndex;
        }
        else
        {
            selectedValues &= ~(1 << itemIndex);
        }

        UpdateDropdownName();

        OnValueChanged.Invoke(selectedValues);
    }

    private void UpdateDropdownName()
    {
        StringBuilder stringBuilder = new StringBuilder();

        int selectedCount = 0;

        for (int i = 0; i < dropdownItems.Count; i++)
        {
            if (dropdownItems[i].isOn)
            {
                if (selectedCount == 0)
                {
                    stringBuilder.Append(dropdownItems[i].itemName);
                }
                else
                {
                    stringBuilder.Append(", ");
                    stringBuilder.Append(dropdownItems[i].itemName);
                }
                selectedCount++;
            }
        }

        if (selectedCount == dropdownItems.Count)
        {
            dropdownNameText.text = "All Selected";
        }
        else if (selectedCount == 0)
        {
            dropdownNameText.text = "None Selected";
        }
        else
        {
            dropdownNameText.text = stringBuilder.ToString();
        }

    }

    public void Animate()
    {
        if (isOn == false && animationType == AnimationType.FADING)
        {
            dropdownAnimator.Play("Fading In");
            isOn = true;
        }

        else if (isOn == true && animationType == AnimationType.FADING)
        {
            dropdownAnimator.Play("Fading Out");
            isOn = false;
        }

        else if (isOn == false && animationType == AnimationType.SLIDING)
        {
            //dropdownAnimator.Play("Sliding In");
            dropdownAnimator.Play("Sliding In Small");
            isOn = true;
        }

        else if (isOn == true && animationType == AnimationType.SLIDING)
        {
            //dropdownAnimator.Play("Sliding Out");
            dropdownAnimator.Play("Sliding Out Small");
            isOn = false;
        }

        else if (isOn == false && animationType == AnimationType.STYLISH)
        {
            dropdownAnimator.Play("Stylish In");
            isOn = true;
        }

        else if (isOn == true && animationType == AnimationType.STYLISH)
        {
            dropdownAnimator.Play("Stylish Out");
            isOn = false;
        }
    }

    private void UpdateDropdownElements()
    {

        rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

        for (int i = 0; i < dropdownItems.Count; i++)
        {
            dropdownItems[i].rectTransform.sizeDelta = new Vector2(dropdownItems[i].rectTransform.sizeDelta.x, rect.height);
        }

        var scrollBarRect = RectTransformUtility.PixelAdjustRect(scrollBarRectTransform, parentCanvas);

        itemListLayoutGroup.padding.right = (int)scrollBarRect.width;

        itemListLayoutGroup.padding.top = (int)(rect.height * 0.35f);
    }

    private void OnRectTransformDimensionsChange()
    {
        if(isInitialized)
        {
            UpdateDropdownElements();
        }
    }
}

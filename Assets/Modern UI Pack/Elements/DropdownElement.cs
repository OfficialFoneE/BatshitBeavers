using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class DropdownElement : MonoBehaviour
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

    private TextMeshProUGUI dropDownSelectedItemText;
    private Image dropDownSelectedItemImage;

    private GameObject triggerObject;

    private Rect rect;

    public class OnValueChangedEvent : UnityEvent<int> { }
    public OnValueChangedEvent OnValueChanged = new OnValueChangedEvent();


    [Header("SETTINGS")]
    public bool enableIcon = true;
    public bool enableTrigger = true;
    public bool enableScrollbar = true;
    public bool invokeAtStart = false;
    public AnimationType animationType;

    [SerializeField] private int _selectedValue = 0;
    public int selectedValue
    {
        get
        {
            return _selectedValue;
        }

        private set
        {
            _selectedValue = value;
        }
    }

    [SerializeField] private int startingBuffer = 0;

    private BufferedArray<BufferedDropdownElement> bufferedDropdownElements;
    private class BufferedDropdownElement : BufferedRectTransform
    {
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
        public Button button;
        public CanvasGroup canvasGroup;
        public LayoutElement layoutElement;
        public BufferedDropdownElement(GameObject gameObject) : base(gameObject)
        {
            itemNameText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            button = gameObject.GetComponentInChildren<Button>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            layoutElement = gameObject.GetComponent<LayoutElement>();
        }
    }
    private BufferedDropdownElement InstantiateBufferedDropdownElement()
    {
        var bufferedDropdownElement = new BufferedDropdownElement(Instantiate(UIReferences.standardDropdownItem, contentTransform));
        bufferedDropdownElement.rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
        bufferedDropdownElement.button.onClick.AddListener(Animate);
        bufferedDropdownElement.button.onClick.AddListener(delegate { ChangeDropdownInfo(bufferedDropdownElements.IndexOf(bufferedDropdownElement)); });
        return bufferedDropdownElement;
    }
    private void BufferDropdownElement (BufferedDropdownElement bufferedDropdownElement, bool state)
    {
        //bufferedDropdownElement.gameObject.SetActive(state);
        bufferedDropdownElement.canvasGroup.alpha = System.Convert.ToInt32(state);
        bufferedDropdownElement.layoutElement.ignoreLayout = !state;
    }

    public enum AnimationType
    {
        FADING,
        SLIDING,
        STYLISH
    }

    private bool isOn;
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

        dropDownSelectedItemText = rectTransform.Find("Content").Find("Main").GetComponentInChildren<TextMeshProUGUI>();
        dropDownSelectedItemImage = rectTransform.Find("Content").Find("Main").Find("Selected Image").GetComponent<Image>();

        rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);
    }

    private IEnumerator Start()
    {

        bufferedDropdownElements = new BufferedArray<BufferedDropdownElement>(InstantiateBufferedDropdownElement, BufferDropdownElement);

        dropDownButton.onClick.AddListener(Animate);


        bufferedDropdownElements.UpdatePooledObjects(startingBuffer);
        bufferedDropdownElements.UpdatePooledObjects(0);

        canvas.overrideSorting = true;
        canvas.sortingOrder = (int)rectTransform.position.y;

        isInitialized = true;


        yield return new WaitForEndOfFrame();

        //OnRectTransformDimensionsChange();

        Invoke("UpdateSortingOrder", 1f);

        //if (enableScrollbar == true)
        //{
        //    itemList.padding.right = 25;
        //    scrollbar.SetActive(true);
        //}

        //else
        //{
        //    itemList.padding.right = 8;
        //    Destroy(scrollbar);
        //}

        //if (enableIcon == false)
        //{
        //    selectedImage.enabled = false;
        //}
    }

    public void ClearDropdown()
    {
        bufferedDropdownElements.UpdatePooledObjects(0);
        
    }

    private void UpdateSortingOrder()
    {
        canvas.sortingOrder = (int)transform.position.y;
    }

    public void ChangeDropdownInfo(int itemIndex)
    {
        dropDownSelectedItemText.text = bufferedDropdownElements[itemIndex].itemName;

        selectedValue = itemIndex;

        OnValueChanged.Invoke(selectedValue);
    }

    public void ChangeDropdownInfoWithoutInvoke(int itemIndex)
    {
        dropDownSelectedItemText.text = bufferedDropdownElements[itemIndex].itemName;

        selectedValue = itemIndex;
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
            //dropdownAnimator.Play("Sliding In Small");

            dropdownAnimator.SetBool("showDropdown", true);

            isOn = true;
        }

        else if (isOn == true && animationType == AnimationType.SLIDING)
        {
            //dropdownAnimator.Play("Sliding Out");
            //dropdownAnimator.Play("Sliding Out Small");

            dropdownAnimator.SetBool("showDropdown", false);

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

        //if (enableTrigger == true && isOn == false)
        //{
        //    triggerObject.SetActive(false);
        //}

        //else if (enableTrigger == true && isOn == true)
        //{
        //    triggerObject.SetActive(true);
        //}
    }

    public void AddItemElement(string itemName)
    {
        var bufferedDropdownElement = bufferedDropdownElements.GetUnusedPooledObjects(1)[0];

        bufferedDropdownElement.itemName = itemName;

        //UpdateDropdownElements();
    }

    public void AddItemElements(string[] itemNames)
    {
        var newBufferedDropdownElements = bufferedDropdownElements.GetUnusedPooledObjects(itemNames.Length);

        for (int i = 0; i < itemNames.Length; i++)
        {
            newBufferedDropdownElements[i].itemName = itemNames[i];
        }

        //ChangeDropdownInfo(bufferedDropdownElements.IndexOf(bufferedDropdownElement));
        //UpdateDropdownElements();
    }

    public void SetItemElements(string[] itemNames)
    {
        bufferedDropdownElements.UpdatePooledObjects(itemNames.Length);

        for (int i = 0; i < bufferedDropdownElements.bufferedCount; i++)
        {
            bufferedDropdownElements[i].itemName = itemNames[i];
        }
        //Need to update heights when instantiated
        ChangeDropdownInfo(0);
        //UpdateDropdownElements();
    }

    private void UpdateDropdownElements()
    {
        for (int i = 0; i < bufferedDropdownElements.bufferedCount; i++)
        {
            bufferedDropdownElements[i].rectTransform.sizeDelta = new Vector2(bufferedDropdownElements[i].rectTransform.sizeDelta.x, rect.height);
        }
    }

    private void UpdateLayoutGroup()
    {
        var scrollBarRect = RectTransformUtility.PixelAdjustRect(scrollBarRectTransform, parentCanvas);

        itemListLayoutGroup.padding.right = (int)scrollBarRect.width;

        itemListLayoutGroup.padding.top = (int)(rect.height * 0.35f);

        parentCanvas.sortingOrder = (int)rectTransform.position.y;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (isInitialized)
        {
            rect = RectTransformUtility.PixelAdjustRect(rectTransform, parentCanvas);

            UpdateDropdownElements();

            UpdateLayoutGroup();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;
using System.Linq;
using UnityEngine.Events;

public class RangeSliderElement : MonoBehaviour
{
    private RectTransform rectTransform;

    //[Header("SETTINGS")]
    //[Range(0, 3)] public int DecimalPlaces = 0;
    //public bool useWholeNumbers = false;
    //public bool showLabels = true;

    public UnityEvent OnValueChanged;

    [SerializeField] private int _minValue = 0;
    public int minValue
    {
        get
        {
            return _minValue;
        }
        set
        {
            minSliderLabel.text = value.ToString();
            minSlider.minValue = value;
            minSlider.value = value;
            maxSlider.minValue = value;
            maxSlider.value = value;
            _minValue = value;
            UpdateTicks();
        }
    }
    [SerializeField] private int _maxValue = 5;
    public int maxValue
    {
        get
        {
            return _maxValue;
        }
        set
        {
            maxSliderLabel.text = value.ToString();
            maxSlider.maxValue = value;
            maxSlider.realValue = value;
            minSlider.maxValue = value;
            _maxValue = value;

            UpdateTicks();
        }
    }

    [Header("MIN SLIDER")]
    public RangeMinSlider minSlider;
    public TextMeshProUGUI minSliderLabel;

    [Header("MAX SLIDER")]
    public RangeMaxSlider maxSlider;
    public TextMeshProUGUI maxSliderLabel;

    private GameObject rangeTickGameObject;
    private RectTransform rangeTickRectTransform;
    private HorizontalLayoutGroup rangeTickLayoutGroup;
    private BufferedArray<BufferedRangeTick> bufferedRangeTicks;

    private class BufferedRangeTick : BufferedRectTransform
    {
        public BufferedRangeTick(GameObject gameObject) : base(gameObject)
        {

        }
    }
    private BufferedRangeTick InstantiateRangeTickBuffer()
    {
        return new BufferedRangeTick(Instantiate(UIReferences.rangeTick, rangeTickRectTransform));
    }
    private void BufferRangeTick(BufferedRangeTick bufferedRangeTick, bool value)
    {
        bufferedRangeTick.gameObject.SetActive(value);
    }

    [SerializeField] private bool _showTicks = true;
    public bool showTicks
    {
        get
        {
            return _showTicks;
        }
        set
        {
            if (_showTicks != value)
            {
                rangeTickGameObject.SetActive(value);
                UpdateTicks();
                _showTicks = value;
            }
        }
    }

    [SerializeField] private float _tickInterval = 1;
    public float tickInterval
    {
        get
        {
            return _tickInterval;
        }
        set
        {
            if(value == 0)
            {
                _tickInterval = 1;
                UpdateTicks();
                return;
            }
            _tickInterval = value;
            UpdateTicks();

        }
    }

    // Properties
    public float CurrentLowerValue
    {
        get { return minSlider.value; }
    }
    public float CurrentUpperValue
    {
        get { return maxSlider.realValue; }
    }

    private void Awake()
    {
        SetInitialReferences();

        UpdateLabels();
        UpdateSliders();
        UpdateTicks();
    }

    private void Start()
    {
        minSlider.onValueChanged.AddListener(delegate { OnValueChanged.Invoke(); });
        maxSlider.onValueChanged.AddListener(delegate { OnValueChanged.Invoke(); });
        //UpdateLabels();
        //UpdateSliders();
        //UpdateTicks();
    }

    private void UpdateLabels()
    {
        minSliderLabel.text = minValue.ToString();
        maxSliderLabel.text = maxValue.ToString();
    }

    private void UpdateSliders()
    {
        minSlider.minValue = minValue;
        minSlider.maxValue = maxValue;
        minSlider.wholeNumbers = true; //useWholeNumbers;
        minSlider.value = minValue;

        maxSlider.minValue = minValue;
        maxSlider.maxValue = maxValue;
        maxSlider.wholeNumbers = true; //useWholeNumbers;
        maxSlider.value = minValue;
        maxSlider.realValue = maxValue;
    }

    private void UpdateTicks()
    {
        if (showTicks)
        {
            rangeTickGameObject.SetActive(true);
            int tickCount = maxValue - minValue;

            bufferedRangeTicks.UpdatePooledObjects((int)(tickCount / tickInterval));
        }
        else
        {
            rangeTickGameObject.SetActive(false);
        }
    }

    private void SetInitialReferences()
    {
        rectTransform = GetComponent<RectTransform>();

        //Debug.Log("THIS SLIDER IS BAD", this);

        minSlider = rectTransform.Find("Min Slider").GetComponent<RangeMinSlider>();
        maxSlider = rectTransform.Find("Max Slider").GetComponent<RangeMaxSlider>();
        minSliderLabel = rectTransform.Find("FirstLabel").GetComponent<TextMeshProUGUI>();
        maxSliderLabel = rectTransform.Find("LastLabel").GetComponent<TextMeshProUGUI>();

        rangeTickLayoutGroup = rectTransform.Find("RangeTicks").GetComponent<HorizontalLayoutGroup>();
        rangeTickRectTransform = rangeTickLayoutGroup.GetComponent<RectTransform>();
        rangeTickGameObject = rangeTickLayoutGroup.gameObject;

        bufferedRangeTicks = new BufferedArray<BufferedRangeTick>(InstantiateRangeTickBuffer, BufferRangeTick);

        List<BufferedRangeTick> bufferedRangeTicksInChildren = new List<BufferedRangeTick>();

        foreach (Transform obj in rangeTickRectTransform)
        {
            bufferedRangeTicksInChildren.Add(new BufferedRangeTick(obj.gameObject));
        }

        bufferedRangeTicks.AddRange(bufferedRangeTicksInChildren);

        bufferedRangeTicks.bufferedCount = bufferedRangeTicksInChildren.Count;

        bufferedRangeTicks.UpdatePooledObjects(0);

        minSlider.label = minSliderLabel;
        minSlider.numberFormat = "n" + 0; //DecimalPlaces;
        maxSlider.label = maxSliderLabel;
        maxSlider.numberFormat = "n" + 0; //DecimalPlaces;

    }

#if (UNITY_EDITOR)
    private void OnValidate()
    {
        SetInitialReferences();
        UpdateLabels();
        UpdateSliders();
        UpdateTicks();
    }
#endif
}

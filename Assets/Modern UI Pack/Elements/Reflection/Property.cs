using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum PropertyType
{
    Int, Float, String, Enum, Float3, Slider, Color, Dropdown
}

public abstract class Property : MonoBehaviour
{

    protected RectTransform rectTransform;

    private object _component;
    protected object component
    {
        get
        {
            return _component;
        }

        set
        {
            _component = value;

        }
    }

    private System.Reflection.PropertyInfo _propertyInfo;
    protected System.Reflection.PropertyInfo propertyInfo
    {
        get
        {
            return _propertyInfo;
        }

        set
        {
            _propertyInfo = value;
            UpdateUI();
        }
    }

    public abstract bool isInteractable
    {
        get;
        set;
    }

    protected TMPro.TextMeshProUGUI propertyNameText;

    protected abstract void Initiate();

    protected abstract void UpdateUI();

    public abstract void UpdateValue();

    public abstract bool isEqual(object newValue);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        propertyNameText = rectTransform.Find("Mask").Find("PropertyName").GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void Initiate(System.Reflection.PropertyInfo propertyInfo, object component)
    {
        _component = component;
        _propertyInfo = propertyInfo;
        propertyNameText.text = StringFormatUtil.VaraibleNameToString(propertyInfo.Name);
        Initiate();
    }


    protected object GetValue()
    {
        return propertyInfo.GetValue(_component);
    }

    protected void SetValue(object value)
    {
        propertyInfo.SetValue(_component, value);
    }
}

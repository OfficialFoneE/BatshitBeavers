using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatProperty : Property
{
    private TMPro.TMP_InputField valueInputField;

    protected override void Initiate()
    {
        valueInputField = GetComponentInChildren<TMPro.TMP_InputField>();
        valueInputField.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;

        valueInputField.onEndEdit.AddListener(delegate { OnEndEdit(valueInputField.text); });

        UpdateUI();
    }

    public override bool isInteractable
    {
        get
        {
            return valueInputField.interactable;
        }
        set
        {
            valueInputField.interactable = value;
        }
    }

    protected override void UpdateUI()
    {
        UpdateValue();
    }

    protected void OnEndEdit(string value)
    {
        float floatValue;
        if (float.TryParse(value, out floatValue))
        {
            SetValue(floatValue);
        }
    }

    public override void UpdateValue()
    {
        valueInputField.text = GetValue().ToString();
    }

    public void SetValue(float value)
    {
        base.SetValue(value);
        UpdateValue();
    }

    public new float GetValue()
    {
        return (float)base.GetValue();
    }

    public override bool isEqual(object newValue)
    {
        return GetValue() == (float)newValue;
    }
}

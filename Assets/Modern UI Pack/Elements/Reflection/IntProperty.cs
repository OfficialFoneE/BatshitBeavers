using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntProperty : Property
{

    private TMPro.TMP_InputField valueInputField;

    protected override void Initiate()
    {
        valueInputField = GetComponentInChildren<TMPro.TMP_InputField>();
        valueInputField.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;

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
        int intValue;
        if(int.TryParse(value, out intValue))
        {
            SetValue(intValue);
        }
    }

    public override void UpdateValue()
    {
        valueInputField.text = GetValue().ToString();
    }

    public void SetValue(int value)
    {
        base.SetValue(value);
        UpdateValue();
    }

    public new int GetValue()
    {
        return (int)base.GetValue();
    }

    public override bool isEqual(object newValue)
    {
        return GetValue() == (int)newValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StringProperty : Property
{
    private TMPro.TMP_InputField valueInputField;

    //private new void Awake()
    //{
    //    valueInputField = GetComponentInChildren<TMPro.TMP_InputField>();
    //    valueInputField.contentType = TMPro.TMP_InputField.ContentType.Standard;

    //    valueInputField.onEndEdit.AddListener(delegate { OnEndEdit(valueInputField.text); });
    //}

    protected override void Initiate()
    {

        valueInputField = GetComponentInChildren<TMPro.TMP_InputField>();
        valueInputField.contentType = TMPro.TMP_InputField.ContentType.Standard;

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

    private void OnEndEdit(string value)
    {
        SetValue(value);
    }
       
    public override void UpdateValue()
    {
        valueInputField.text = GetValue();
    }

    public void SetValue(string value)
    {
        base.SetValue(value);
        UpdateValue();
    }

    public new string GetValue()
    {
        return (string)base.GetValue();
    }

    public override bool isEqual(object newValue)
    {
        return GetValue() == (string)newValue;
    }
}

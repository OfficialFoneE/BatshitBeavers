using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float3Property : Property
{
    private TMPro.TMP_InputField xInputField;
    private TMPro.TMP_InputField yInputField;
    private TMPro.TMP_InputField zInputField;

    protected override void Initiate()
    {

        xInputField = rectTransform.Find("Mask").Find("XInputField").GetComponent<TMPro.TMP_InputField>();
        yInputField = rectTransform.Find("Mask").Find("YInputField").GetComponent<TMPro.TMP_InputField>();
        zInputField = rectTransform.Find("Mask").Find("ZInputField").GetComponent<TMPro.TMP_InputField>();

        xInputField.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        yInputField.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        zInputField.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;

        xInputField.onEndEdit.AddListener(OnEndEdit);
        yInputField.onEndEdit.AddListener(OnEndEdit);
        zInputField.onEndEdit.AddListener(OnEndEdit);

        UpdateUI();
    }

    public override bool isInteractable
    {
        get
        {
            return xInputField.interactable && xInputField.interactable && zInputField.interactable;
        }
        set
        {
            xInputField.interactable = value;
            yInputField.interactable = value;
            zInputField.interactable = value;
        }
    }

    protected override void UpdateUI()
    {
        UpdateValue();
    }

    protected void OnEndEdit(string value)
    {

        float xValue;
        float yValue;
        float zValue;

        if (float.TryParse(xInputField.text, out xValue) && float.TryParse(yInputField.text, out yValue) && float.TryParse(zInputField.text, out zValue))
        {
            SetValue (new Vector3(xValue, yValue, zValue));
        }
    }


    public override void UpdateValue()
    {
        var value = GetValue();
        xInputField.text = value.x.ToString();
        yInputField.text = value.y.ToString();
        zInputField.text = value.z.ToString();
    }

    public void SetValue(Vector3 value)
    {
        base.SetValue(value);
        UpdateValue();
    }

    public new Vector3 GetValue()
    {
        return (Vector3)base.GetValue();
    }

    public override bool isEqual(object newValue)
    {
        return GetValue() == (Vector3)newValue;
    }
}

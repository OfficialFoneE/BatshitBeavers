using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class EnumProperty : Property
{

    private TMPro.TMP_InputField valueInputField;

    private CustomDropdown customDropdown;

    protected override void Initiate()
    {
        customDropdown = GetComponentInChildren<CustomDropdown>();

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
            //valueInputField.interactable = value;
        }
    }

    protected override void UpdateUI()
    {

        var enumValues = System.Enum.GetNames(propertyInfo.PropertyType);

        for (int i = 0; i < enumValues.Length; i++)
        {

            CustomDropdown.Item item = new CustomDropdown.Item();
            item.itemName = enumValues[i];
            item.OnItemSelection = new UnityEngine.Events.UnityEvent();
            item.OnItemSelection.AddListener( delegate { OnEndEdit(i); });

            customDropdown.InstantiateDropdownItem(item);
        }

        UpdateValue();
    }

    private void OnEndEdit(int value)
    {
        SetValue(value);
    }

    public override void UpdateValue()
    {
        customDropdown.ChangeDropdownInfo(GetValue());
    }

    public void SetValue(string value)
    {
        base.SetValue(value);
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

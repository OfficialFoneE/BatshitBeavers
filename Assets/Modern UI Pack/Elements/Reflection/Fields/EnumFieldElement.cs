using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UI.Elements.Reflection
{
    public class EnumFieldElement : FieldElement
    {
        private DropdownElement dropdownElement;

        public override bool isInteractable
        {
            get
            {
                throw new System.NotImplementedException();
                // return dropdownElement .interactable;
            }
            set
            {
                throw new System.NotImplementedException();
                //valueInputField.interactable = value;
            }
        }

        private void Awake()
        {
            dropdownElement = GetComponentInChildren<DropdownElement>();
            dropdownElement.OnValueChanged.AddListener(delegate { CallEventOnValueChanged(dropdownElement.selectedValue); });
        }

        protected override void InitaliseElement(object value)
        {
            dropdownElement.ClearDropdown();
            dropdownElement.AddItemElements(System.Enum.GetNames(fieldInfo.FieldType));
            dropdownElement.ChangeDropdownInfoWithoutInvoke((int)value);
        }

        protected override void UpdateElement(object value)
        {
            dropdownElement.ChangeDropdownInfoWithoutInvoke((int)value);
        }
        
        public void SetValue(int value)
        {
            base.SetValue(value);
        }

        public new int GetValue()
        {
            return (int)base.GetValue();
        }

        public override bool isEqual(object value)
        {
            return GetValue() == (int)value;
        }
    }
}

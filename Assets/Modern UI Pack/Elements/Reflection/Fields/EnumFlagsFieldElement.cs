using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UI.Elements.Reflection
{
    public class EnumFlagsFieldElement : FieldElement
    {
        private MultiSelectDropdownElement multiSelectDropdownElement;

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
            multiSelectDropdownElement = GetComponentInChildren<MultiSelectDropdownElement>();
            multiSelectDropdownElement.OnValueChanged.AddListener(delegate { CallEventOnValueChanged(multiSelectDropdownElement.selectedValues); });
        }



        protected override void InitaliseElement(object value)
        {
            throw new System.NotImplementedException();
            //multiSelectDropdownElement.ClearDropdown();
            //multiSelectDropdownElement.AddItemElements(System.Enum.GetNames(fieldInfo.FieldType));
            //multiSelectDropdownElement.ChangeDropdownInfoWithoutInvoke((int)value);
        }

        protected override void UpdateElement(object value)
        {
            throw new System.NotImplementedException();
            //multiSelectDropdownElement.ChangeDropdownInfoWithoutInvoke((int)value);
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

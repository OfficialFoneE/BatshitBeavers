using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements.Reflection
{
    public class StringFieldElement : FieldElement
    {

        private TMPro.TMP_InputField valueInputField;

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

        private void Awake()
        {
            valueInputField = GetComponentInChildren<TMPro.TMP_InputField>();
            valueInputField.contentType = TMPro.TMP_InputField.ContentType.Standard;
            valueInputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                CallEventOnValueChanged(intValue);
            }
        }

        protected override void InitaliseElement(object value)
        {
            UpdateElement(value);
        }

        protected override void UpdateElement(object value)
        {
            valueInputField.text = ((string)value).ToString();
            valueInputField.textComponent.text = ((string)value).ToString();
        }

        public void SetValue(int value)
        {
            base.SetValue(value);
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
}


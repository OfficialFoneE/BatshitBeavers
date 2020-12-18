using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UI.Elements.Reflection
{
    public class FloatFieldElement : FieldElement
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
            valueInputField.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
            valueInputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            float floatValue;
            if (float.TryParse(value, out floatValue))
            {
                CallEventOnValueChanged(floatValue);
            }
        }

        protected override void InitaliseElement(object value)
        {
            UpdateElement(value);
        }

        protected override void UpdateElement(object value)
        {
            valueInputField.text = ((float)value).ToString();
            valueInputField.textComponent.text = ((float)value).ToString();
        }

        public void SetValue(float value)
        {
            base.SetValue(value);
        }

        public new float GetValue()
        {
            return (int)base.GetValue();
        }

        public override bool isEqual(object value)
        {
            return GetValue() == (float)value;
        }
    }
}

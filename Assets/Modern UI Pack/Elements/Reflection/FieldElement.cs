using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UI.Elements.Reflection
{
    public abstract class FieldElement : MonoBehaviour
    {

        protected object baseComponent;

        public delegate void OnValueChanged(object value);
        public event OnValueChanged onValueChanged;

        protected string fieldName;
        protected FieldInfo fieldInfo;

        //protected TMPro.TextMeshProUGUI fieldNameText;

        public abstract bool isInteractable
        {
            get;
            set;
        }

        protected abstract void UpdateElement(object value);
        protected abstract void InitaliseElement(object value);

        public abstract bool isEqual(object value);

        //protected virtual void Awake()
        //{
        //    fieldNameText = transform.Find("Mask").Find("FieldName").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        //}

        protected void Start()
        {
            onValueChanged += UpdateElement;
        }

        public void SetFieldInfo(FieldInfo fieldInfo, object baseComponent)
        {
            this.fieldInfo = fieldInfo;
            this.baseComponent = baseComponent;

            //fieldNameText.text = StringFormatUtil.VaraibleNameToString(fieldInfo.Name);

            InitaliseElement(GetValue());
        }

        public void SetComponentBase(Component baseComponent)
        {
            this.baseComponent = baseComponent;

            UpdateElement(GetValue());
        }

        public void CallEventOnValueChanged(object value)
        {
            if(onValueChanged != null)
                {
                onValueChanged.Invoke(value);
            }
        }

        protected object GetValue()
        {
            return fieldInfo.GetValue(baseComponent);
        }

        protected void SetValue(object value)
        {
            fieldInfo.SetValue(baseComponent, value);
            CallEventOnValueChanged(value);
        }
    }
}

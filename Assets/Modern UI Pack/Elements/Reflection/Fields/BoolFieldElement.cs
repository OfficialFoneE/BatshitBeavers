using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

namespace UI.Elements.Reflection
{
    public class BoolFieldElement : FieldElement
    {

        private SwitchElement switchElement;

        public override bool isInteractable
        {
            get
            {
                throw new System.NotImplementedException();
                //return switchElement.interactable;
            }
            set
            {
                throw new System.NotImplementedException();
                //switchElement.interactable = value;
            }
        }


        private void Awake()
        {
            switchElement = GetComponentInChildren<SwitchElement>();
            switchElement.OnValueChanged.AddListener(delegate { CallEventOnValueChanged(switchElement.isOn); });
        }

        protected override void InitaliseElement(object value)
        {
            UpdateElement(value);
        }
        protected override void UpdateElement(object value)
        {
            switchElement.isOn = (bool)value;
        }

        public void SetValue(int value)
        {
            base.SetValue(value);
        }

        public new bool GetValue()
        {
            return (bool)base.GetValue();
        }

        public override bool isEqual(object value)
        {
            return GetValue() == (bool)value;
        }

    }
}

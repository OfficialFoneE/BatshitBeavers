using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchElement : MonoBehaviour
{

    [System.Serializable]
    public class OnValueChangedEvent : UnityEvent<bool>
    {

    }

    private Michsky.UI.ModernUIPack.SwitchAnim switchAnim;

    public bool isOn
    {
        get
        {
            return switchAnim.isOn;
        }
        set
        {
            if(value != switchAnim.isOn)
            {
                switchAnim.AnimateSwitch();
            }
            //switchAnim.isOn = value;
            //switchAnim.AnimateSwitch();
        }
    }

    public OnValueChangedEvent OnValueChanged = new OnValueChangedEvent();

    private void Awake()
    {
        switchAnim = GetComponent<Michsky.UI.ModernUIPack.SwitchAnim>();
    }

    private void Start()
    {
        switchAnim.OnEvents.AddListener(delegate { CallOnValueChanged(true); } );
        switchAnim.OffEvents.AddListener(delegate { CallOnValueChanged(false); });
    }

    private void CallOnValueChanged(bool value)
    {
        OnValueChanged.Invoke(value);
    }


}

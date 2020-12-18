using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UISquareButtonToggle : MonoBehaviour
{

    private Toggle _toggle;
    public Toggle toggle
    {
        get
        {
            return _toggle;
        }
    }

    private Animator animator;

    public Sprite sprite
    {
        set
        {
            centerImage.overrideSprite = value;
        }
    }

    private Image centerImage;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        animator = GetComponent<Animator>();
        centerImage = transform.Find("Icon").Find("CenterIcon").GetComponent<Image>();
    }

    public void Start()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(bool value)
    {
        animator.SetBool("isOn", value);
    }

    
}

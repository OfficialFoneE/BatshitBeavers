using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HightlightButtonElement : Button
{

    private TextMeshProUGUI mainText;
    private TextMeshProUGUI blurredText;

    //private Animator animator;

    private new void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        blurredText = rectTransform.Find("Content").Find("TextComponents").Find("BlurredText").GetComponent<TextMeshProUGUI>();
        mainText = blurredText.transform.Find("MainText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        this.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public string text
    {
        get
        {
            return mainText.text;
        }
        set
        {
            mainText.text = value;
            blurredText.text = value;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdvancedButton : MonoBehaviour
{

    private Button button;

    private TextMeshProUGUI buttonNameText;

    private void Awake()
    {
        button = GetComponent<Button>();

    }

    public string buttonName
    {
        set
        {
            buttonNameText.text = value;
        }
    }

}

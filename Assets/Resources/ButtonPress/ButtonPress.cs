using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    private TMPro.TextMeshProUGUI text;

    public KeyCode keyCode
    {
        set
        {
            if(text != null)
                text.text = value.ToString().ToUpper();
        }
    }

    private void Awake()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

}

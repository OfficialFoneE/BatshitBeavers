using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TImer : MonoBehaviour
{

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        var ts = TimeSpan.FromSeconds(MinigameManager.gameTime);
        text.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
}

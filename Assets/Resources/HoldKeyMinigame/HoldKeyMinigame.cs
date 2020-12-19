using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldKeyMinigame : MonoBehaviour
{

    private ProgressBar progressBar;

    public KeyCode keyCode;

    public float holdTime = 30f;
    public float decayTime = 1f;
    private float currentHoldTime = 0;

    public delegate void OnFinished();
    public OnFinished onFinished;

    private void Awake()
    {
        progressBar = GetComponent<ProgressBar>();
    }

    private void OnEnable()
    {
        currentHoldTime = 0;
    }

    private void Update()
    {
        if(Input.GetKey(keyCode))
        {
            currentHoldTime += Time.deltaTime;
        }
        else
        {
            currentHoldTime -= decayTime * Time.deltaTime;
        }

        currentHoldTime = Mathf.Clamp(currentHoldTime, 0, holdTime);

        progressBar.currentPercent = currentHoldTime / holdTime * 100;

        if (currentHoldTime >= holdTime)
        {
            if(onFinished != null)
            {
                onFinished.Invoke();
            }
        }
    }

}

using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpamKeyMinigame : MonoBehaviour
{
    private static Color player1Color = new Color(173 / 255.0f, 47 / 255.0f, 69 / 255.0f, 255 / 255.0f);
    private static Color player2Color = new Color(240 / 255.0f, 181 / 255.0f, 65 / 255.0f, 255 / 255.0f);

    private ProgressBar progressBar;

    private TextMeshProUGUI sliderText;
    private Image loadingBarImage;

    public bool isPlayer1;

    public KeyCode keyCode;

    public float valuePerKeyPress = 0.1f;

    public bool hasDecay = false;
    public float decayAmount = 0.05f;

    private float currentValue = 0;

    public delegate void OnFinished();
    public OnFinished onFinished;

    private void Awake()
    {
        progressBar = GetComponent<ProgressBar>();

        sliderText = GetComponentInChildren<TextMeshProUGUI>();
        loadingBarImage = transform.Find("Background").Find("Loading Bar").GetComponent<Image>();
    }

    private void Start()
    {
        if(isPlayer1)
        {
            sliderText.color = player1Color;
            loadingBarImage.color = player1Color;
        } else
        {
            sliderText.color = player2Color;
            loadingBarImage.color = player2Color;
        }
    }

    private void OnEnable()
    {
        currentValue = 0;
    }

    private void Update()
    {
        if(hasDecay)
        {
            currentValue -= decayAmount * Time.deltaTime;
        }

        currentValue = Mathf.Clamp(currentValue, 0, 1f);

        if (Input.GetKeyDown(keyCode))
        {
            currentValue += valuePerKeyPress;
        }

        progressBar.currentPercent = currentValue * 100;

        if (currentValue >= 1f)
        {
            if (onFinished != null)
            {
                onFinished.Invoke();
            }
        }
    }
}

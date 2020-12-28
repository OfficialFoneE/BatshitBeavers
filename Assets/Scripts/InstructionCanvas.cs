using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionCanvas : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(GameManager.EnableMainCanvas);
        gameObject.SetActive(false);
    }
}

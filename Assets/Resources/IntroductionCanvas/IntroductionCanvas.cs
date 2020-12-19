using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionCanvas : MonoBehaviour
{

    private Animator animator;

    private static Button findGameButton;
    private static Button localGameButton;
    private static Button createGameButton;
    private static Button joinGameButton;

    private void Awake()
    {
        ConnectedToServer = false;

        animator = GetComponent<Animator>();

        var buttons = GetComponentsInChildren<Button>();

        findGameButton = buttons[0];
        localGameButton = buttons[1];
        createGameButton = buttons[2];
        joinGameButton = buttons[3];
    }

    public static bool ConnectedToServer
    {
        set
        {
            findGameButton.interactable = value;
            createGameButton.interactable = value;
            joinGameButton.interactable = value;
        }
    }

    private void Start()
    {
        findGameButton.onClick.AddListener(NetworkManager.QueueForRandomMM);

        localGameButton.onClick.AddListener(NetworkManager.QueueForRandomMM);
        createGameButton.onClick.AddListener(NetworkManager.QueueForRandomMM);
        joinGameButton.onClick.AddListener(NetworkManager.QueueForRandomMM);
    }

    public new bool enabled
    {
        set
        {
            animator.SetBool("Enable", value);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    private Animator animator;

    private static Button findGameButton;
    private static Button instructionsButton;

    //private static Button localGameButton;
    //private static Button createGameButton;
    //private static Button joinGameButton;

    private void Awake()
    {

        animator = GetComponent<Animator>();

        var buttons = GetComponentsInChildren<Button>(true);

        findGameButton = buttons[0];
        instructionsButton = buttons[1];
        //localGameButton = buttons[1];
        //createGameButton = buttons[2];
        //joinGameButton = buttons[3];

        ConnectedToServer = false;
    }

    public static bool ConnectedToServer
    {
        set
        {
            findGameButton.interactable = value;
            //createGameButton.interactable = value;
            //joinGameButton.interactable = value;
        }
    }

    private void Start()
    {
        findGameButton.onClick.AddListener(NetworkManager.QueueForRandomMM);
        instructionsButton.onClick.AddListener(GameManager.EnableInstructionsCanvas);
        //localGameButton.onClick.AddListener(NetworkManager.QueueForRandomMM);
        //createGameButton.onClick.AddListener(NetworkManager.CreateLobby);
        //joinGameButton.onClick.AddListener(NetworkManager.JoinLobby);
    }

    public new bool enabled
    {
        set
        {
            animator.SetBool("Enable", value);
        }
    }
}

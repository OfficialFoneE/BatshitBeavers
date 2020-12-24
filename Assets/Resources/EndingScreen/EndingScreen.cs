using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{

    private static Animator animator;

    private static List<Button> buttons = new List<Button>();

    private static PhotonView photonView;

    public static bool finished = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        buttons.AddRange(GetComponentsInChildren<Button>());
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].onClick.AddListener(Quit);
        }
    }

    public static void Player1Wins()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Win");
            AudioSFXReferences.PlayWin();
        }
        else
        {
            animator.SetTrigger("Lose");
            AudioSFXReferences.PlayLose();
        }
        photonView.RPC("Player1WinsNet", RpcTarget.Others);

        finished = true;
    }

    public static void Player2Wins()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Lose");
            AudioSFXReferences.PlayLose();
        }
        else
        {
            animator.SetTrigger("Win");
            AudioSFXReferences.PlayWin();
        }
        photonView.RPC("Player2WinsNet", RpcTarget.Others);

        finished = true;
    }

    [PunRPC]
    public void Player1WinsNet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Win");
            AudioSFXReferences.PlayWin();
        }
        else
        {
            animator.SetTrigger("Lose");
            AudioSFXReferences.PlayLose();
        }

        finished = true;
    }

    [PunRPC]
    public void Player2WinsNet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Lose");
            AudioSFXReferences.PlayLose();
        }
        else
        {
            animator.SetTrigger("Win");
            AudioSFXReferences.PlayWin();
        }

        finished = true;
    }

    public static void Win()
    {
        animator.SetTrigger("Win");
    }

    public void Quit()
    {
        Application.Quit();
        //SceneManager.LoadScene(0);
    }

}

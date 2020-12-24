using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{

    private static Animator animator;

    private static List<Button> buttons = new List<Button>();

    private static PhotonView photonView;

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
        }
        else
        {
            animator.SetTrigger("Lose");
        }
        photonView.RPC("Player1WinsNet", RpcTarget.Others);
    }

    public static void Player2Wins()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Lose");
        }
        else
        {
            animator.SetTrigger("Win");
        }
        photonView.RPC("Player2WinsNet", RpcTarget.Others);
    }

    [PunRPC]
    public void Player1WinsNet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Win");
        }
        else
        {
            animator.SetTrigger("Lose");
        }
    }
    [PunRPC]
    public void Player2WinsNet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            animator.SetTrigger("Lose");
        }
        else
        {
            animator.SetTrigger("Win");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }

}

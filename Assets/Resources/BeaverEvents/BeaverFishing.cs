using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverFishing : MonoBehaviour
{
    private GameObject myGameObject;

    private Animator animator;

    private PhotonView photonView;

    public KeyCode keyCode
    {
        set
        {
            holdKeyMinigame.keyCode = value;
        }
    }

    public bool isPlayer1;

    private HoldKeyMinigame holdKeyMinigame;

    public delegate void OnCompleted();
    public OnCompleted onCompleted;

    private void Awake()
    {
        myGameObject = gameObject;
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        holdKeyMinigame = GetComponentInChildren<HoldKeyMinigame>();
    }

    private void OnEnable()
    {

    }

    private void Start()
    {
        //holdKeyMinigame.onClicked += OnClick;
        holdKeyMinigame.onFinished += OnFinished;

        holdKeyMinigame.isPlayer1 = isPlayer1;
    }

    private void OnClick()
    {
        animator.SetTrigger("Hit");
    }

    private void OnFinished()
    {
        if (onCompleted != null)
        {
            onCompleted.Invoke();
        }

        photonView.RPC("FinishedCall", RpcTarget.AllViaServer);

        //if (isPlayer1)
        //{
        //    photonView.RPC("FinishedCall", RpcTarget.AllViaServer);
        //}
        //else
        //{

        //}

        myGameObject.SetActive(false);
    }

    [PunRPC]
    public void FinishedCall()
    {



        if (isPlayer1)
        {

        }
        else
        {

        }

        myGameObject.SetActive(false);
    }
}

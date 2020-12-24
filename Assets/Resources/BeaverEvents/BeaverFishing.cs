using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverFishing : MonoBehaviour
{
    private GameObject myGameObject;

    private Animator animator;

    private PhotonView photonView;

    [SerializeField] private KeyCode _keyCode;
    public KeyCode keyCode
    {
        get
        {
            return _keyCode;
        }
        set
        {
            holdKeyMinigame.keyCode = value;
            buttonPress.keyCode = value;
            _keyCode = value;
        }
    }

    public bool isPlayer1;

    [SerializeField] private int _buildCost;
    public int buildCost
    {
        get
        {
            return _buildCost;
        }
        set
        {
            _buildCost = value;
            buildCostText.text = _buildCost.ToString();
        }
    }

    private GameObject buildCostObject;
    private TMPro.TextMeshProUGUI buildCostText;

    private GameObject buttonPressObject;
    private ButtonPress buttonPress;

    private GameObject holdKeyBold;
    private HoldKeyMinigame holdKeyMinigame;

    public delegate void OnCompleted();
    public OnCompleted onCompleted;

    [SerializeField] private bool _isBought = false;

    private void Awake()
    {
        myGameObject = gameObject;
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        holdKeyMinigame = GetComponentInChildren<HoldKeyMinigame>();

        buildCostText = transform.Find("Canvas").GetChild(1).GetChild(1).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        buildCostObject = buildCostText.transform.parent.gameObject;

        buttonPress = transform.Find("Canvas").GetChild(1).GetChild(0).GetComponent<ButtonPress>();
        buttonPressObject = buttonPress.gameObject;

        holdKeyMinigame = GetComponentInChildren<HoldKeyMinigame>();
        holdKeyBold = holdKeyMinigame.gameObject;

        NetworkManager.OnGameStart += OnGameStart;
    }

    private void Start()
    {
        holdKeyMinigame.onFinished += OnFinished;

        holdKeyMinigame.keyCode = _keyCode;
        holdKeyMinigame.isPlayer1 = isPlayer1;
        buttonPress.keyCode = _keyCode;
        buildCostText.text = buildCost.ToString();
    }

    private void OnEnable()
    {
        buttonPress.keyCode = _keyCode;
        holdKeyMinigame.keyCode = _keyCode;
        holdKeyMinigame.isPlayer1 = isPlayer1;
    }

    public void EnableOnNetwork()
    {
        buttonPressObject.SetActive(false);
        holdKeyBold.SetActive(false);
        buildCostObject.SetActive(true);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(keyCode) && !_isBought)
            {
                int fishs = isPlayer1 ? GameCanvas.GetPlayer1Fish() : GameCanvas.GetPlayer2Fish();
                int beavers = isPlayer1 ? GameCanvas.GetPlayer1Beavers() : GameCanvas.GetPlayer2Beavers();

                if (Input.GetKeyDown(keyCode) && (fishs >= buildCost) && (beavers >= 1))
                {

                    if (isPlayer1)
                    {
                        GameCanvas.SetPlayer1Fish(fishs - buildCost);
                        GameCanvas.SetPlayer1Beavers(beavers - 1);

                    }
                    else
                    {
                        GameCanvas.SetPlayer2Fish(fishs - buildCost);
                        GameCanvas.SetPlayer2Beavers(beavers - 1);
                    }
                
                    holdKeyMinigame.currentHoldTime = 0.0f;
                    holdKeyBold.SetActive(true);
                    buildCostObject.SetActive(false);

                    photonView.RPC("OnBought", RpcTarget.Others);

                    _isBought = true;

                    if (isPlayer1)
                    {
                        LeftSide.AddNewFishBeaver();
                    }
                    else
                    {
                        RightSide.AddNewFishBeaver();
                    }
                }
            }
        }
    }

    public void Buy()
    {
        if (photonView.IsMine)
        {
            holdKeyMinigame.currentHoldTime = 0.0f;
            holdKeyBold.SetActive(true);
            buildCostObject.SetActive(false);
            _isBought = true;
            photonView.RPC("OnBought", RpcTarget.Others);
        }
    }

    private void OnGameStart()
    {
        if (((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        buttonPressObject.SetActive(((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)));

        holdKeyBold.SetActive(false);
        buildCostObject.SetActive(true);

        _isBought = false;
    }

    [PunRPC]
    public void OnBought()
    {
        buttonPressObject.SetActive(false);
        holdKeyBold.SetActive(true);
        buildCostObject.SetActive(false);
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

        holdKeyMinigame.currentHoldTime = 0.0f;

        if (isPlayer1)
        {
            GameCanvas.SetPlayer1Fish(GameCanvas.GetPlayer1Fish() + 1);
        }
        else
        {
            GameCanvas.SetPlayer2Fish(GameCanvas.GetPlayer2Fish() + 1);
        }

        //photonView.RPC("FinishedCall", RpcTarget.AllViaServer);

        //if (isPlayer1)
        //{
        //    photonView.RPC("FinishedCall", RpcTarget.AllViaServer);
        //}
        //else
        //{

        //}

        //myGameObject.SetActive(false);
    }

    //[PunRPC]
    //public void FinishedCall()
    //{



    //    if (isPlayer1)
    //    {

    //    }
    //    else
    //    {

    //    }

    //    myGameObject.SetActive(false);
    //}
}

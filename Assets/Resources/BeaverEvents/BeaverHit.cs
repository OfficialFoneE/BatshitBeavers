using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverHit : MonoBehaviour
{
    private GameObject myGameObject;

    private Animator animator;

    [SerializeField] private KeyCode _keyCode;
    public KeyCode keyCode
    {
        get
        {
            return _keyCode;
        }
        set
        {
            spamKeyMinigame.keyCode = value;
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

    private GameObject spamKeyBold;
    private SpamKeyMinigame spamKeyMinigame;

    public delegate void OnCompleted();
    public OnCompleted onCompleted;

    private PhotonView photonView;

    [SerializeField] private bool _isBought = false;

    private void Awake()
    {
        myGameObject = gameObject;

        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();

        buildCostText = transform.Find("Canvas").GetChild(0).GetChild(2).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        buildCostObject = buildCostText.transform.parent.gameObject;

        buttonPress = transform.Find("Canvas").GetChild(0).GetChild(0).GetComponent<ButtonPress>();
        buttonPressObject = buttonPress.gameObject;

        spamKeyMinigame = transform.Find("Canvas").GetChild(0).GetChild(1).GetComponent<SpamKeyMinigame>();
        spamKeyBold = spamKeyMinigame.gameObject;
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
                    Debug.Log("This was called");
                    spamKeyMinigame.currentValue = 0.0f;

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

                    spamKeyBold.SetActive(true);
                    buildCostObject.SetActive(false);

                    photonView.RPC("OnBought", RpcTarget.Others);

                    _isBought = true;

                }
            }
        }
    }

    [PunRPC]
    public void OnBought()
    {
        spamKeyBold.SetActive(true);
        buildCostObject.SetActive(false);
    }

    private void Start()
    {
        spamKeyMinigame.onClicked += OnClick;
        spamKeyMinigame.onClicked += OnFinished;
        spamKeyMinigame.isPlayer1 = isPlayer1;

        NetworkManager.OnGameStart += OnGameStart;

        buttonPress.keyCode = _keyCode;
        spamKeyMinigame.keyCode = _keyCode;
        buildCostText.text = buildCost.ToString();
    }

    private void OnGameStart()
    {
        if (((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        spamKeyBold.SetActive(false);
        buildCostObject.SetActive(true);

        _isBought = false;
    }

    private void OnClick()
    {
        animator.SetTrigger("Hit");
    }

    private void OnFinished()
    {
        if(onCompleted != null)
        {
            onCompleted.Invoke();

        }

        spamKeyMinigame.currentValue = 0.0f;

        //pun rpc to adding a tree
    }
}

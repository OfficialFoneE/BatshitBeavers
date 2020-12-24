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

        NetworkManager.OnGameStart += OnGameStart;
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

                    spamKeyMinigame.currentValue = 0.0f;
                    spamKeyBold.SetActive(true);
                    buildCostObject.SetActive(false);

                    photonView.RPC("OnBought", RpcTarget.Others);

                    _isBought = true;

                    if (isPlayer1)
                    {
                        LeftSide.AddNewWoodBeaver();
                    }
                    else
                    {
                        RightSide.AddNewWoodBeaver();
                    }

                }
            }
        }
    }

    public void Buy()
    {
            _isBought = true;
            spamKeyMinigame.currentValue = 0.0f;
            spamKeyBold.SetActive(true);
            buildCostObject.SetActive(false);
            buttonPressObject.SetActive(true);
    }

    [PunRPC]
    public void OnBought()
    {
        buttonPressObject.SetActive(false);
        spamKeyBold.SetActive(true);
        buildCostObject.SetActive(false);
    }

    public void EnableOnNetwork()
    {
        buttonPressObject.SetActive(false);
        spamKeyBold.SetActive(false);
        buildCostObject.SetActive(true);
    }

    private void Start()
    {
        spamKeyMinigame.onClicked += OnClick;
        spamKeyMinigame.onFinished += OnFinished;
        spamKeyMinigame.isPlayer1 = isPlayer1;

        NetworkManager.OnGameStart += OnGameStart;

        buttonPress.keyCode = _keyCode;
        spamKeyMinigame.keyCode = _keyCode;
        spamKeyMinigame.isPlayer1 = isPlayer1;
        buildCostText.text = buildCost.ToString();
    }

    private void OnEnable()
    {
        buttonPress.keyCode = _keyCode;
        spamKeyMinigame.keyCode = _keyCode;
        spamKeyMinigame.isPlayer1 = isPlayer1;
    }

    private void OnGameStart()
    {
        if (((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }


        buttonPressObject.SetActive(((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)));

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

        if(isPlayer1)
        {
            GameCanvas.SetPlayer1Logs(GameCanvas.GetPlayer1Logs() + 1);
        }
        else
        {
            GameCanvas.SetPlayer2Logs(GameCanvas.GetPlayer2Logs() + 1);
        }

        spamKeyMinigame.currentValue = 0.0f;

        //pun rpc to adding a tree
    }
}

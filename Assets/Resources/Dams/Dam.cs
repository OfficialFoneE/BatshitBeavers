using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dam : MonoBehaviour
{

    private Animator animator;

    [SerializeField] private int[] buildCosts = { 3, 5, 7, 9 };

    public int state
    {
        get
        {
           return animator.GetInteger("State");
        }
        set
        {
            animator.SetInteger("State", value);
        }
    }

    public KeyCode keyCode;

    private GameObject buildCostObject;
    private TMPro.TextMeshProUGUI buildCostText;

    private GameObject buttonPressObject;
    private ButtonPress buttonPress;

    private GameObject spamKeyBold;
    private SpamKeyMinigame spamKeyMinigame;

    private PhotonView photonView;

    public bool isPlayer1;

    public delegate void GeneralEventHandler();
    public GeneralEventHandler OnMaximum;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        buildCostText = transform.Find("Canvas").GetChild(0).GetChild(2).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        buildCostObject = buildCostText.transform.parent.gameObject; ;

        buttonPress = transform.Find("Canvas").GetChild(0).GetChild(0).GetComponent<ButtonPress>();
        buttonPressObject = buttonPress.gameObject;

        spamKeyMinigame = transform.Find("Canvas").GetChild(0).GetChild(1).GetComponent<SpamKeyMinigame>();
        spamKeyBold = spamKeyMinigame.gameObject;

        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        NetworkManager.OnGameStart += ResetDam;

        spamKeyMinigame.onClicked += OnClick;
        spamKeyMinigame.onZero += OnZero;
        spamKeyMinigame.onFinished += OnFinish;

        buttonPress.keyCode = keyCode;
        spamKeyMinigame.keyCode = keyCode;
        spamKeyMinigame.isPlayer1 = isPlayer1;
    }

    private void OnEnable()
    {
        buildCostText.text = buildCosts[state].ToString();
    }

    private void ResetDam()
    {
        state = 0;
        spamKeyBold.SetActive(false);

        if (((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        buttonPressObject.SetActive(((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)));
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (state == 0)
            {
                int logs = isPlayer1 ? GameCanvas.GetPlayer1Logs() : GameCanvas.GetPlayer2Logs();

                if (Input.GetKeyDown(keyCode) && (logs >= buildCosts[state]))
                {

                    //cehck for income and start dam
                    spamKeyBold.SetActive(true);
                    spamKeyMinigame.currentValue = 0.5f;
                    if (isPlayer1)
                    {
                        GameCanvas.SetPlayer1Logs(logs - buildCosts[state]);

                    }
                    else
                    {
                        GameCanvas.SetPlayer2Logs(logs - buildCosts[state]);
                    }

                    AudioSFXReferences.PlayPurchase();

                    state++;
                    buildCostText.text = buildCosts[state].ToString();
                    photonView.RPC("EnableSpamKey", RpcTarget.Others, true, buildCosts[state], true);

                }
            }
        }
    }

    public void OnClick()
    {
        AudioSFXReferences.PlayButtonClick();
    }

    public void OnNetworkEnable()
    {
        buttonPressObject.SetActive(false);

        AudioSFXReferences.PlayBadPurchase();
    }

    [PunRPC]
    private void EnableSpamKey(bool value, int costCount, bool enableBuildCost)
    {
        spamKeyBold.SetActive(value);
        buildCostText.text = costCount.ToString();
        buildCostObject.SetActive(enableBuildCost);
    }

    private void OnFinish()
    {
        if(state == 3)
        {
            return;
        }
        else
        {
            int logs = isPlayer1 ? GameCanvas.GetPlayer1Logs() : GameCanvas.GetPlayer2Logs();

            if (logs >= buildCosts[state])
            {
                spamKeyMinigame.currentValue = 0.5f;

                if(isPlayer1)
                {
                    GameCanvas.SetPlayer1Logs(logs - buildCosts[state]);

                } else
                {
                    GameCanvas.SetPlayer2Logs(logs - buildCosts[state]);
                }

                state++;

                if (state == 3)
                {
                    buildCostObject.SetActive(false);
                    photonView.RPC("EnableSpamKey", RpcTarget.Others, true, 0, false);

                    if (OnMaximum != null)
                    {
                        OnMaximum.Invoke();
                    }

                    if(isPlayer1)
                    {
                        LeftSide.AddNewDamBeaver();
                    } else
                    {
                        RightSide.AddNewDamBeaver();
                    }
                }
                else
                {
                    photonView.RPC("EnableSpamKey", RpcTarget.Others, true, buildCosts[state], true);
                }

                buildCostText.text = buildCosts[state].ToString();


                AudioSFXReferences.PlayPurchase();
            }
        }
    }

    private void OnZero()
    {
        if(state > 1)
        {
            state--;
            buildCostText.text = buildCosts[state].ToString();
            spamKeyMinigame.currentValue = 0.5f;
            buildCostObject.SetActive(true);
            photonView.RPC("EnableSpamKey", RpcTarget.Others, true, buildCosts[state], true);
        }
        if (state == 1)
        {
            state--;
            buildCostText.text = buildCosts[state].ToString();
            spamKeyBold.SetActive(false);
            photonView.RPC("EnableSpamKey", RpcTarget.Others, false, buildCosts[state], true);
        }
    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverDen : MonoBehaviour
{
    [SerializeField] private int[] buildCosts = { 1, 3, 6, 10 };

    public KeyCode keyCode;

    private GameObject buildCostObject;
    private TMPro.TextMeshProUGUI buildCostText;

    private GameObject buttonPressObject;
    private ButtonPress buttonPress;

    private GameObject spamKeyBold;
    private SpamKeyMinigame spamKeyMinigame;

    private PhotonView photonView;

    public bool isPlayer1;

    public int maxBeaverCount = 7;
    public int beaverCount = 2;

    private void Awake()
    {
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
        spamKeyMinigame.onFinished += OnFinish;

        buttonPress.keyCode = keyCode;
        spamKeyMinigame.keyCode = keyCode;
        spamKeyMinigame.isPlayer1 = isPlayer1;
    }

    private void OnClick()
    {
        AudioSFXReferences.PlayButtonClick();
    }

    private void ResetDam()
    {
        keyCode = MinigameManager.GetRandomKeyCode();
        beaverCount = 2;
        buttonPress.keyCode = keyCode;
        spamKeyMinigame.keyCode = keyCode;
        spamKeyMinigame.isPlayer1 = isPlayer1;
        buildCostText.text = buildCosts[beaverCount - 2].ToString();

        if (((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        buttonPressObject.SetActive(((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)));
    }

    private void OnFinish()
    {
        if (beaverCount == maxBeaverCount)
        {
            return;
        }
        else
        {
            int fish = isPlayer1 ? GameCanvas.GetPlayer1Fish() : GameCanvas.GetPlayer2Fish();

            if (fish >= buildCosts[beaverCount - 2])
            {
                spamKeyMinigame.currentValue = 0f;

                if (isPlayer1)
                {
                    GameCanvas.SetPlayer1Fish(fish - buildCosts[beaverCount - 2]);
                    GameCanvas.SetPlayer1Beavers(GameCanvas.GetPlayer1Beavers() + 1);
                }
                else
                {
                    GameCanvas.SetPlayer2Fish(fish - buildCosts[beaverCount - 2]);
                    GameCanvas.SetPlayer2Beavers(GameCanvas.GetPlayer2Beavers() + 1);
                }

                beaverCount++;

                AudioSFXReferences.PlayPurchase();

                if (beaverCount == maxBeaverCount)
                {
                    buildCostObject.SetActive(false);

                    photonView.RPC("DsiableCostObject", RpcTarget.Others, false, 0);
                }
                else
                {
                    buildCostText.text = buildCosts[beaverCount - 2].ToString();
                    photonView.RPC("DsiableCostObject", RpcTarget.Others, true, buildCosts[beaverCount - 2]);
                }
            }
        }
    }

    [PunRPC]
    private void DsiableCostObject(bool value, int costCount)
    {
        buildCostObject.SetActive(value);
        buildCostText.text = costCount.ToString();

        AudioSFXReferences.PlayBadPurchase();
    }
}

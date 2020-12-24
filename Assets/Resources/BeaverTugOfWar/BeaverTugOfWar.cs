using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverTugOfWar : MonoBehaviour
{

    private GameObject warningButton;
    private GameObject beaversFighting;
    private GameObject canvas;
    private ButtonPress buttonPress;
    private TugOfWarMinigame tugOfWarMinigame;

    public PhotonView photonView;

    public KeyCode keyCode;
    public float warningTime = 10f;
    public float propabilityToStart = .2f;

    public bool inPlay = false;

    public int logWinAmount = 5;

    private void Awake()
    {
        warningButton = transform.Find("Warning").gameObject;
        beaversFighting = transform.Find("BeaversFighting").gameObject;
        canvas = transform.Find("Canvas").gameObject;
        buttonPress = GetComponentInChildren<ButtonPress>();
        photonView = GetComponent<PhotonView>();
        tugOfWarMinigame = GetComponentInChildren<TugOfWarMinigame>();
    }

    private void Start()
    {
        NetworkManager.OnGameStart += DisableBeaversFighting;
        tugOfWarMinigame.onFinishedStatus += OnWin;
    }

    [PunRPC]
    public void EnableBeaverTugOfWar(int keyCode)
    {
        warningButton.SetActive(true);
        beaversFighting.SetActive(false);
        canvas.SetActive(false);
        this.keyCode = (KeyCode)keyCode;
        Invoke("EnableBeaversFighting", warningTime);
    }

    private void EnableBeaversFighting()
    {
        buttonPress.keyCode = keyCode;
        tugOfWarMinigame.keyCode = keyCode;
        warningButton.SetActive(false);
        beaversFighting.SetActive(true);
        canvas.SetActive(true);
    }

    private void OnWin(TugOfWarMinigame.WinStatus winStatus)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (winStatus)
            {
                case TugOfWarMinigame.WinStatus.Player1:
                    GameCanvas.SetPlayer1Logs(GameCanvas.GetPlayer1Logs() + logWinAmount);
                    break;

                case TugOfWarMinigame.WinStatus.Player2:
                    GameCanvas.SetPlayer2Logs(GameCanvas.GetPlayer2Logs() + logWinAmount);
                    break;
                case TugOfWarMinigame.WinStatus.Tie:
                    break;
                case TugOfWarMinigame.WinStatus.None:
                    break;
                default:
                    break;
            }
        }

        DisableBeaversFighting();
        PlaySound((int)winStatus);

        photonView.RPC("DisableBeaversFighting", RpcTarget.Others);
        photonView.RPC("PlaySound", RpcTarget.Others, (int)winStatus);

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("RandomWaitTime", Random.Range(10, 90));
        }
    }

    [PunRPC]
    private void DisableBeaversFighting()
    {
        warningButton.SetActive(false);
        beaversFighting.SetActive(false);
        canvas.SetActive(false);
    }

    [PunRPC]
    private void PlaySound(int winStatus)
    {
        switch ((TugOfWarMinigame.WinStatus)winStatus)
        {
            case TugOfWarMinigame.WinStatus.Player1:
                if(PhotonNetwork.IsMasterClient)
                {
                    AudioSFXReferences.PlayWin();
                }
                else
                {
                    AudioSFXReferences.PlayLose();
                }
            break;

            case TugOfWarMinigame.WinStatus.Player2:
                if (PhotonNetwork.IsMasterClient)
                {
                    AudioSFXReferences.PlayLose();
                }
                else
                {
                    AudioSFXReferences.PlayWin();
                }
                break;
            case TugOfWarMinigame.WinStatus.Tie:
                break;
            case TugOfWarMinigame.WinStatus.None:
                break;
            default:
                break;
        }
    }

    private void RandomWaitTime()
    {
        inPlay = false;
    }

}

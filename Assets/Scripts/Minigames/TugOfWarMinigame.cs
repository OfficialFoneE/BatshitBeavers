using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TugOfWarMinigame : MonoBehaviour
{

    public enum WinStatus { Player1, Player2, Tie, None }


    public delegate void OnFinishedStatus(WinStatus winStatus);
    public OnFinishedStatus onFinishedStatus;

    private WinStatus currentWinStatus = WinStatus.None;

    private enum TugOfWarWinCondition { Percentage, PressCount }

    [Space()]
    [SerializeField] private TugOfWarWinCondition tugOfWarWinCondition;
    [Space()]
    [SerializeField] private int startingPressCount = 10;
    [SerializeField] private float percentage = 0.8f;

    [Space()]
    [SerializeField] private int pressCount = 1000;

    private TugOfWar_Player1 player1;
    private TugOfWar_Player2 player2;

    private PhotonView photonView;

    private UnityEngine.UI.Slider slider;

    private void Awake()
    {
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();
        photonView = GetComponent<PhotonView>();
        player1 = GetComponentInChildren<TugOfWar_Player1>();
        player2 = GetComponentInChildren<TugOfWar_Player2>();
    }

    public void OnEnable()
    {
        currentWinStatus = WinStatus.None;
    }

    public void UpdateTugOfWar()
    {

        switch (tugOfWarWinCondition)
        {
            case TugOfWarWinCondition.Percentage:

                int count = player1.pressCount + player2.pressCount + startingPressCount;

                if (count != 0)
                {
                    slider.value = (player1.pressCount / (float)count) / percentage;
                }
                else
                {
                    slider.value = 0.5f;
                }

                break;
            case TugOfWarWinCondition.PressCount:

                int count2 = player1.pressCount + player2.pressCount;

                if (count2 != 0)
                {

                    slider.value = player1.pressCount / (float)count2; ;
                }

                break;
        }

        if (UpdateWinCondition())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("CallTugOfWarWinner", RpcTarget.AllViaServer, (int)currentWinStatus);
            }
        }
    }

    public bool UpdateWinCondition()
    {

        switch (player1.pressCount.CompareTo(player2.pressCount))
        {
            case -1:
                currentWinStatus = WinStatus.Player2;
                break;
            case 0:
                currentWinStatus = WinStatus.Tie;
                return false;
            case 1:
                currentWinStatus = WinStatus.Player1;
                break;
            default:
                currentWinStatus = WinStatus.None;
                return false;
        }


        switch (tugOfWarWinCondition)
        {
            case TugOfWarWinCondition.Percentage:

                int count = player1.pressCount + player2.pressCount + startingPressCount;

                switch (currentWinStatus)
                {
                    case WinStatus.Player1:

                        if (player1.pressCount / (float)count >= percentage)
                        {
                            return true;
                        }

                        break;

                    case WinStatus.Player2:

                        if (player2.pressCount / (float)count >= percentage)
                        {
                            return true;
                        }

                        break;
                }
                return false;

            case TugOfWarWinCondition.PressCount:

                if (player1.pressCount >= pressCount || player2.pressCount >= pressCount)
                {
                    return true;
                }
                return false;

            default:
                return false;
        }
    }

    [PunRPC]
    public void CallTugOfWarWinner(int value)
    {
        if (onFinishedStatus != null)
        {
            onFinishedStatus.Invoke((WinStatus)value);

        }

        this.gameObject.SetActive(false);
    }
}



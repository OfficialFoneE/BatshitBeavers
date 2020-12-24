using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public delegate void GeneralEventHandler();
    public static GeneralEventHandler OnGameStart;

    private static bool isPlayingGame = false;

    private static int joinRetryCount = 0;

    private static new PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    //short code 32757 is the code for max CCU reached. We need to impliment this if 20 people are more are trying to connect
    //32766 when gameID is already in use
    private void Start()
    {
        NetworkDebugCanvas.SetConnectionStatus("Disconnected");

        NetworkDebugCanvas.SetConnectingStatus("Connecting To Master Server");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {

        TitleScreen.ConnectedToServer = true;
        //enable Buttons;
        NetworkDebugCanvas.SetConnectionStatus("Connected To Master");

        NetworkDebugCanvas.SetConnectingStatus("Waiting");

    }

    public static void QueueForRandomMM()
    {
        NetworkDebugCanvas.SetConnectingStatus("Joining Random Open Room");

        GameManager.EnterQueue();

        PhotonNetwork.JoinRandomRoom();

    }

    public static void CreateLobby(string lobbyName)
    {
        NetworkDebugCanvas.SetConnectingStatus("Creating Random Room");

        GameManager.EnterQueue();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(lobbyName, roomOptions, TypedLobby.Default);

    }
    public static void JoinLobby(string lobbyName)
    {
        NetworkDebugCanvas.SetConnectingStatus("Joining Room");

        GameManager.EnterQueue();

        PhotonNetwork.JoinRoom(lobbyName);

    }
    public override void OnJoinedRoom()
    {
        NetworkDebugCanvas.SetConnectionStatus("Joined Room");
        //NetworkDebugCanvas.SetConnectingStatus("Creating New Room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        NetworkDebugCanvas.SetConnectionStatus("Failed To Join Room");

        NetworkDebugCanvas.SetConnectingStatus("Creating New Room");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(RandomUtil.GenerateRandomString(20), roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        NetworkDebugCanvas.SetConnectionStatus("Created Room");

        NetworkDebugCanvas.SetConnectingStatus("Waiting for Opponent");

        //AudioSFXReferences.PlayQueueingMusic();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        joinRetryCount++;

        if (joinRetryCount != 5)
        {

            OnJoinRandomFailed(0, "Retrying to join");
        }
        else
        {
            //TODO failed to create lobby 5 times, disconnecting
            PhotonNetwork.Disconnect();
            joinRetryCount = 0;
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            isPlayingGame = true;

            photonView.RPC("StartGame", RpcTarget.AllViaServer);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //if(isPlayingGame)
        //{
        //    //TODO: message player has left the game

        //    //SceneManager.LoadScene(0);
        //    //Application.Quit();

        //    EndingScreen.Win();

        //    isPlayingGame = false;
        //}
        //else
        //{
        //    NetworkDebugCanvas.SetConnectionStatus("Created Room");

        //    NetworkDebugCanvas.SetConnectingStatus("Waiting for Opponent");

        //    isPlayingGame = false;
        //}
        EndingScreen.Win();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        TitleScreen.ConnectedToServer = true;

        switch (cause)
        {
            case DisconnectCause.None:
                break;
            case DisconnectCause.ExceptionOnConnect:
                break;
            case DisconnectCause.Exception:
                break;
            case DisconnectCause.ServerTimeout:
                break;
            case DisconnectCause.ClientTimeout:
                break;
            case DisconnectCause.DisconnectByServerLogic:
                //quit game
                break;
            case DisconnectCause.DisconnectByServerReasonUnknown:
                break;
            case DisconnectCause.InvalidAuthentication:
                break;
            case DisconnectCause.CustomAuthenticationFailed:
                break;
            case DisconnectCause.AuthenticationTicketExpired:
                break;
            case DisconnectCause.MaxCcuReached:
                break;
            case DisconnectCause.InvalidRegion:
                break;
            case DisconnectCause.OperationNotAllowedInCurrentState:
                break;
            case DisconnectCause.DisconnectByClientLogic:
                break;
            case DisconnectCause.DisconnectByOperationLimit:
                break;
            case DisconnectCause.DisconnectByDisconnectMessage:
                break;
            default:
                break;
        }

        if(!EndingScreen.finished)
        {
            EndingScreen.Win();
        }

        //SceneManager.LoadScene(0);
        //OnEnable();
        //TODO: message disconnetion error and maybe reconnect based on error
    }

    [PunRPC]
    public void StartGame()
    {
        if(OnGameStart != null)
        {
            OnGameStart.Invoke();
        }
    }

}

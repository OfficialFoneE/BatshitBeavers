using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{

    public static GameObject logImage;
    public static GameObject fishImage;
    public static GameObject beaverImage;

    public static TeamIcons player1Icons;
    public static TeamIcons player2Icons;

    private Animator animator;

    private static PhotonView photonView;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        player1Icons = transform.Find("Team1Header").GetComponentInChildren<TeamIcons>();
        player2Icons = transform.Find("Team2Header").GetComponentInChildren<TeamIcons>();
    }

    private void Start()
    {
        NetworkManager.OnGameStart += OnGameStart;

        animator.SetBool("Enabled", false);
    }

    private void OnGameStart()
    {
        animator.SetBool("Enabled", true);
    }

    public static void SetPlayer1Logs(int count)
    {
        photonView.RPC("SetPlayer1LogsNet", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetPlayer1LogsNet(int count)
    {
        player1Icons.bufferedLogImage.count = count;
    }

    public static void SetPlayer1Fish(int count)
    {
        photonView.RPC("SetPlayer1FishNet", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetPlayer1FishNet(int count)
    {
        player1Icons.bufferedFishImages.count = count;
    }
    public static void SetPlayer1Beavers(int count)
    {
        photonView.RPC("SetPlayer1BeaversNet", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetPlayer1BeaversNet(int count)
    {
        player1Icons.bufferedBeaverImages.count = count;
    }

    public static void SetPlayer2Logs(int count)
    {
        photonView.RPC("SetPlayer2LogsNet", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetPlayer2LogsNet(int count)
    {
        player2Icons.bufferedLogImage.count = count;
        Debug.Log("Logs was called");
    }

    public static void SetPlayer2Fish(int count)
    {
        photonView.RPC("SetPlayer2FishNet", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetPlayer2FishNet(int count)
    {
        player2Icons.bufferedFishImages.count = count;
    }
    public static void SetPlayer2Beavers(int count)
    {
        photonView.RPC("SetPlayer2BeaversNet", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetPlayer2BeaversNet(int count)
    {
        player2Icons.bufferedBeaverImages.count = count;
    }

    public static int GetPlayer1Logs()
    {
        return player1Icons.bufferedLogImage.count;
    }

    public static int GetPlayer2Logs()
    {
        return player2Icons.bufferedLogImage.count;
    }
    public static int GetPlayer1Fish()
    {
        return player1Icons.bufferedFishImages.count;
    }

    public static int GetPlayer2Fish()
    {
        return player2Icons.bufferedFishImages.count;
    }
    public static int GetPlayer1Beavers()
    {
        return player1Icons.bufferedBeaverImages.count;
    }

    public static int GetPlayer2Beavers()
    {
        return player2Icons.bufferedBeaverImages.count;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void OnResourcesLoad()
    {
        logImage = Resources.Load<GameObject>("GameCanvas/LogImage");
        fishImage = Resources.Load<GameObject>("GameCanvas/FishImage");
        beaverImage = Resources.Load<GameObject>("GameCanvas/BeaverImage");
    }

}

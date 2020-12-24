using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSide : MonoBehaviour
{
    public static int[] hitCosts = { 2, 3, 4, 5 };
    public static int[] fishCosts = { 2, 3, 4, 5 };

    public static List<BeaverHit> freeBeaverHits = new List<BeaverHit>();
    public static List<BeaverHit> usedBeaverHits = new List<BeaverHit>();
    public static List<BeaverFishing> freeBeaverFishing = new List<BeaverFishing>();
    public static List<BeaverFishing> usedBeaverFishing = new List<BeaverFishing>();

    public static List<Dam> freeBeaverDams = new List<Dam>();
    public static List<Dam> usedBeaverDams = new List<Dam>();

    private static PhotonView photonView;

    private void Awake()
    {
        freeBeaverHits.AddRange(GetComponentsInChildren<BeaverHit>(true));
        freeBeaverFishing.AddRange(GetComponentsInChildren<BeaverFishing>(true));
        freeBeaverDams.AddRange(GetComponentsInChildren<Dam>(true));

        for (int i = 0; i < freeBeaverDams.Count; i++)
        {
            freeBeaverDams[i].OnMaximum += CheckIfPlayerHasWon;
        }

        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        NetworkManager.OnGameStart += OnGameStart;

        for (int i = 0; i < freeBeaverHits.Count; i++)
        {
            freeBeaverHits[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < freeBeaverFishing.Count; i++)
        {
            freeBeaverFishing[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < freeBeaverDams.Count; i++)
        {
            freeBeaverDams[i].gameObject.SetActive(false);
        }
    }

    private void OnGameStart()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        for (int i = 0; i < usedBeaverHits.Count; i++)
        {
            usedBeaverHits[i].gameObject.SetActive(false);
            freeBeaverHits.Add(usedBeaverHits[i]);
        }
        for (int i = 0; i < usedBeaverFishing.Count; i++)
        {
            freeBeaverFishing[i].gameObject.SetActive(false);
            freeBeaverFishing.Add(usedBeaverFishing[i]);
        }
        for (int i = 0; i < usedBeaverDams.Count; i++)
        {
            usedBeaverDams[i].gameObject.SetActive(false);
            freeBeaverDams.Add(usedBeaverDams[i]);
        }

        usedBeaverHits.Clear();
        usedBeaverFishing.Clear();
        usedBeaverDams.Clear();

        if (!PhotonNetwork.IsMasterClient)
        {
            SetFirstWoodBeaver();
            AddNewWoodBeaver();

            SetFirstFishBeaver();
            AddNewFishBeaver();

            AddNewDamBeaver();
        }
    }

    public static void SetFirstWoodBeaver()
    {
        var beaverHit = freeBeaverHits[0];

        beaverHit.keyCode = MinigameManager.GetRandomKeyCode();
        beaverHit.Buy();

        beaverHit.gameObject.SetActive(true);

        usedBeaverHits.Add(beaverHit);
        freeBeaverHits.RemoveAt(0);

        photonView.RPC("NetworkInstantiateFirstWoodBeaver1", RpcTarget.Others);
    }
    [PunRPC]
    public void NetworkInstantiateFirstWoodBeaver1()
    {
        if (freeBeaverHits.Count != 0)
        {
            var beaverHit = freeBeaverHits[0];

            beaverHit.OnBought();
            beaverHit.gameObject.SetActive(true);

            usedBeaverHits.Add(beaverHit);
            freeBeaverHits.RemoveAt(0);
        }
    }

    [PunRPC]
    public void NetworkInstantiateWoodBeaver1()
    {
        if (freeBeaverHits.Count != 0)
        {
            var beaverHit = freeBeaverHits[0];

            beaverHit.EnableOnNetwork();
            beaverHit.buildCost = hitCosts[usedBeaverHits.Count - 1];
            beaverHit.gameObject.SetActive(true);

            usedBeaverHits.Add(beaverHit);
            freeBeaverHits.RemoveAt(0);
        }
    }

    public static void AddNewWoodBeaver()
    {
        if (freeBeaverHits.Count > 0)
        {
            var beaverHit = freeBeaverHits[0];

            beaverHit.keyCode = MinigameManager.GetRandomKeyCode();
            beaverHit.buildCost = hitCosts[usedBeaverHits.Count - 1];
            beaverHit.gameObject.SetActive(true);

            usedBeaverHits.Add(beaverHit);
            freeBeaverHits.RemoveAt(0);

            photonView.RPC("NetworkInstantiateWoodBeaver1", RpcTarget.Others);
        }
    }

    public static void SetFirstFishBeaver()
    {
        var beaverFish = freeBeaverFishing[0];

        beaverFish.keyCode = MinigameManager.GetRandomKeyCode();
        beaverFish.Buy();

        beaverFish.gameObject.SetActive(true);

        usedBeaverFishing.Add(beaverFish);
        freeBeaverFishing.RemoveAt(0);

        photonView.RPC("NetworkInstantiateFirstFishBeaver1", RpcTarget.Others);
    }

    public static void AddNewFishBeaver()
    {
        if (freeBeaverFishing.Count > 0)
        {
            var beaverFish = freeBeaverFishing[0];

            beaverFish.keyCode = MinigameManager.GetRandomKeyCode();
            beaverFish.buildCost = fishCosts[usedBeaverFishing.Count - 1];
            beaverFish.gameObject.SetActive(true);

            usedBeaverFishing.Add(beaverFish);
            freeBeaverFishing.RemoveAt(0);

            photonView.RPC("NetworkInstantiateFishBeaver1", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void NetworkInstantiateFirstFishBeaver1()
    {
        if (freeBeaverFishing.Count != 0)
        {
            var beaverFish = freeBeaverFishing[0];

            beaverFish.OnBought();
            //beaverFish.buildCost = hitCosts[usedBeaverHits.Count - 1];
            beaverFish.gameObject.SetActive(true);

            usedBeaverFishing.Add(beaverFish);
            freeBeaverFishing.RemoveAt(0);
        }
    }

    [PunRPC]
    public void NetworkInstantiateFishBeaver1()
    {
        if (freeBeaverFishing.Count != 0)
        {
            var beaverFish = freeBeaverFishing[0];

            beaverFish.EnableOnNetwork();
            beaverFish.gameObject.SetActive(true);

            usedBeaverFishing.Add(beaverFish);
            freeBeaverFishing.RemoveAt(0);
        }
    }

    public static void AddNewDamBeaver()
    {
        if (freeBeaverDams.Count > 0)
        {
            var dam = freeBeaverDams[0];

            dam.keyCode = MinigameManager.GetRandomKeyCode();
            dam.gameObject.SetActive(true);

            usedBeaverDams.Add(dam);
            freeBeaverDams.RemoveAt(0);

            photonView.RPC("NetworkInstantiateDam", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void NetworkInstantiateDam()
    {
        if (freeBeaverDams.Count != 0)
        {
            var dam = freeBeaverDams[0];
            dam.OnNetworkEnable();
            //beaverFish.EnableOnNetwork();
            //beaverFish.buildCost = hitCosts[usedBeaverHits.Count - 1];
            dam.gameObject.SetActive(true);

            usedBeaverDams.Add(dam);
            freeBeaverDams.RemoveAt(0);
        }
    }

    public void CheckIfPlayerHasWon()
    {
        if (freeBeaverDams.Count == 0)
        {
            bool hasWon = true;
            for (int i = 0; i < usedBeaverDams.Count; i++)
            {
                if(usedBeaverDams[i].state != 3)
                {
                    hasWon = false;
                }
            }

            if(hasWon)
            {
                EndingScreen.Player2Wins();
            }
        }
    }
}

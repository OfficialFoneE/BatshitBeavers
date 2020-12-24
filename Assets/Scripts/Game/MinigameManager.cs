using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{

    private static readonly KeyCode[] allowedKeys = { KeyCode.Tab, KeyCode.Space, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10,
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,KeyCode.Alpha7,KeyCode.Alpha8,KeyCode.Alpha9,
        KeyCode.Comma, KeyCode.Minus, KeyCode.Period, KeyCode.Slash, KeyCode.Semicolon, KeyCode.Equals, KeyCode.LeftBracket, KeyCode.RightBracket,
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
        KeyCode.Mouse0, KeyCode.Mouse1 };

    private static List<KeyCode> freeKeyCodes = new List<KeyCode>();

    private List<BeaverTugOfWar> beaverTugOfWars = new List<BeaverTugOfWar>();

    [SerializeField] public static float gameTime = 0;

    private float tickTimer = 20.0f;

    [SerializeField] private float tickDelay;

    private bool gameIsPlaying = false;

    public void Awake()
    {
        NetworkManager.OnGameStart += OnGameStart;

        beaverTugOfWars.AddRange(FindObjectsOfType<BeaverTugOfWar>());

    }

    private void OnEnable()
    {
        gameTime = 0;
    }

    private void Update()
    {
        if (gameIsPlaying)
        {
            gameTime += Time.deltaTime;

            if (PhotonNetwork.IsMasterClient)
            {

                tickTimer += Time.deltaTime;

                if (tickTimer >= tickDelay)
                {

                    CallTugOfWars();

                    tickTimer = 0;
                }
            }
        }
    }

    public void CallTugOfWars()
    {
        //float exp = (Mathf.Exp(1.5f * (gameTime / 60.0f) - 4.0f) + .1f) / 2.0f;

        float exp = (Mathf.Exp(2f * (gameTime / 60.0f) - 4.0f) + .3f) / 10.0f;

        for (int i = 0; i < beaverTugOfWars.Count; i++)
        {
            if (!beaverTugOfWars[i].inPlay && Random.value < beaverTugOfWars[i].propabilityToStart * exp)
            {
                beaverTugOfWars[i].photonView.RPC("EnableBeaverTugOfWar", RpcTarget.AllViaServer, (int)GetRandomKeyCode());

                beaverTugOfWars[i].inPlay = true;
            }
        }

    }

    public static void CreateNewLogForPlayer1(int photonViewID)
    {
        PhotonView.Find(photonViewID);
    }
    public static void CreateNewLogForPlayer2(int photonViewID)
    {
        PhotonView.Find(photonViewID);
    }

    public static KeyCode GetRandomKeyCode()
    {
        int index = Random.Range(0, freeKeyCodes.Count);
        var keyCode = freeKeyCodes[index];
        freeKeyCodes.RemoveAt(index);
        return keyCode;
    }

    public void OnGameStart()
    {
        freeKeyCodes.Clear();
        freeKeyCodes.AddRange(allowedKeys);


        //TugOfWarTest.SetActive(true);
        gameIsPlaying = true;
    }

}

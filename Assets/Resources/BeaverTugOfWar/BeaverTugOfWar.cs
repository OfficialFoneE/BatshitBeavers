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
    }

    [PunRPC]
    public void EnableBeaverTugOfWar(int keyCode)
    {
        Debug.Log("Fighting has been called");
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

    private void DisableBeaversFighting()
    {
        warningButton.SetActive(false);
        beaversFighting.SetActive(false);
        canvas.SetActive(false);
        inPlay = false;
    }

}

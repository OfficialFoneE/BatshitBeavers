﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TugOfWar_Player1 : MonoBehaviour, IPunObservable
{

    private TugOfWarMinigame minigame;

    private PhotonView photonView;

    [SerializeField] public KeyCode buttonToPress;

    public int _pressCount;
    public int pressCount
    {
        get
        {
            return _pressCount;
        }
        set
        {
            _pressCount = value;
            
        }
    }

    private void Awake()
    {
        minigame = GetComponentInParent<TugOfWarMinigame>();
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        _pressCount = 0;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(buttonToPress))
        {
            _pressCount++;
            AudioSFXReferences.PlayButtonClick();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.Serialize(ref _pressCount);
            minigame.UpdateTugOfWar();
        }
        else
        {
            stream.Serialize(ref _pressCount);
            minigame.UpdateTugOfWar();
            Debug.Log("Reading");
        }
    }
}

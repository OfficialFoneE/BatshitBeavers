using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{

    public GameObject TugOfWarTest;

    public void Awake()
    {
        NetworkManager.OnGameStart += OnGameStart;


    }

    public void OnGameStart()
    {
        TugOfWarTest.SetActive(true);
    }

}

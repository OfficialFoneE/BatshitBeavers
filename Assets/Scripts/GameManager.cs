﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    private static TitleScreen titleScreen;

    private static IntroductionCanvas introductionCanvas;

    private static RepeatingTileEnviroment repeatingTileEnviroment;

    private static InstructionCanvas instructionCanvas;

    private void Awake()
    {
        titleScreen = FindObjectOfType<TitleScreen>(true);
        introductionCanvas = FindObjectOfType<IntroductionCanvas>(true);
        repeatingTileEnviroment = FindObjectOfType<RepeatingTileEnviroment>(true);
        instructionCanvas = FindObjectOfType<InstructionCanvas>(true);
    }

    private void Start()
    {
        NetworkManager.OnGameStart += FoundQueue;
    }

    public static void EnableInstructionsCanvas()
    {
        titleScreen.gameObject.SetActive(false);
        instructionCanvas.gameObject.SetActive(true);
    }
    public static void EnableMainCanvas()
    {
        instructionCanvas.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(true);
    }

    public static void EnterQueue()
    {
        titleScreen.enabled = false;
        introductionCanvas.enabled = true;

        repeatingTileEnviroment.speed = repeatingTileEnviroment.queueSpeed;

        AudioSFXReferences.PlayQueueingMusic();
    }

    public static void WaitForQueue()
    {
    }

    public static void FoundQueue()
    {
        introductionCanvas.enabled = false;

        repeatingTileEnviroment.AlignWithEnviroment();

        CameraSceneMover.TravelToRiverEnviroment();

        AudioSFXReferences.PlayGameMusic();
    }

    public static void LeaveQueue()
    {
        titleScreen.enabled = true;
        introductionCanvas.enabled = false;

        repeatingTileEnviroment.speed = repeatingTileEnviroment.startSpeed;

        AudioSFXReferences.PlayMainMenuMusic();
    }

}

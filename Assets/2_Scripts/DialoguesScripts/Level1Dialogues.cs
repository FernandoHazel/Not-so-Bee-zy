﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Dialogues : MonoBehaviour
{
    [TextArea] public string[] FirstLevelDialogues;
    int currentDialogue;
    [SerializeField] GameObject dialogueImage = default;
    [SerializeField] UserInterface ui = default;
    bool showDialogue = true;
    [SerializeField] Player3D player;
    [SerializeField] CameraControl CameraControl = default;
    bool nextDialogueBool = false;
    void Start()
    {
        currentDialogue = 0;
        ui.pauseButton.interactable = false;
    }

    void Update()
    {
        if (GameManager.gameIsPaused || player.youWon)
        {
            showDialogue = false;
        }

        else
        {
            showDialogue = true;
        }
        StopDialogue();
        ActiveDialogues();
        MoveCamera();
    }

    void MoveCamera()
    {
        if (showDialogue == false)
        {
            return;
        }

        else 
        {
            if (currentDialogue == 1)
            {
                CameraControl.currentView = CameraControl.views[4];
            }
        }
    }

    void StopDialogue()
    {
        if (currentDialogue >= 4)
        {
            showDialogue = false;
            return;
        }
    }

    //Active the dialogues and stop the game or resumes it
    public void ActiveDialogues()
    {
        if (showDialogue)
        {
            //player.move = false;
            player.speed = 0;
            player.rotationSpeed = 0;
            dialogueImage.SetActive(true);
            ui.ShowText(FirstLevelDialogues[currentDialogue]);
            if (nextDialogueBool == true)
            {
                nextDialogueBool = false;
                currentDialogue++;
            }
        }
        else
        {
            //player.move = true;
            player.speed = player.speedInicial;
            player.rotationSpeed = player.rotationSpeedInicial;
            ui.pauseButton.interactable = true;
            dialogueImage.SetActive(false);
            this.enabled = false;
        }
    }

    public void NextDialogueBool()
    {
        nextDialogueBool = true;
    }
}

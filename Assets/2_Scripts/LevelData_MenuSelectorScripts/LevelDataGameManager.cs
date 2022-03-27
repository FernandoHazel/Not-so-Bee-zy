using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataGameManager : MonoBehaviour
{

    [SerializeField] LevelData levelData; //el scriptableobject que guarda la info
    public int levelIndex; //en que nivel estamos

    [SerializeField] Player3D playerScript;

    [SerializeField] UserInterface ui;

    [SerializeField] GameObject[] honeyCombs;

    private void Awake()
    {
        levelData.LoadData();
    }

    private void Start()
    {
        levelData.SetCurrentLevel(levelIndex); //actualizar currentlevel

        for (int i = 0; i < levelData.GetHoneycombs(levelIndex); i++) //para prender en la ui la cantidad de honeycombs que ya se tiene
        {
            ui.UpdateHoneyComb(i);
        }

        playerScript.honeyComb = levelData.GetHoneycombs(levelIndex);

    }
    private void Update()
    {
        if (playerScript.youWon == true) 
        {
            if ((levelIndex + 1) <= levelData.levelInfo.Length - 1) 
            {
                levelData.SetUnlocked(levelIndex + 1);
                levelData.SetCurrentLevel(levelIndex + 1); //actualizar currentlevel
            }
            levelData.SetRecordTime(levelIndex,ui.timer);
            levelData.SaveData();
        }

    }

}

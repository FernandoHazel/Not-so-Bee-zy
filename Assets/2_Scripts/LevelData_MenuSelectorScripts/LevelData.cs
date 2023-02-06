using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 0)]

public class LevelData : ScriptableObject
{
    public LevelInfo[] levelInfo;
    public int currentLevelGlobal;
    public int honeycombsGlobal;
    [SerializeField] SkinInfo[] skinInfo;
    public int skinSelected;

    //Store, honeycombs, skins
    public int GetHoneycombsGlobal()
    {
        return honeycombsGlobal;
    }
    public void AddHoneycombsGlobal()
    {
        honeycombsGlobal++;
    }
    public void SpendHoneycombsGlobal(int precio) //cuando se compra
    {
        honeycombsGlobal -= precio;
    }

    public int GetSkinSelected() 
    {
        return skinSelected;
    }

    public void SetSkinSelected(int newSelection) 
    {
        skinSelected = newSelection;
    }
    public bool GetSkinUnlocked(int index) //saber si un skin esta unlocked o no
    {
        return skinInfo[index].skinUnlocked;
    }

    public void SetSkinUnlocked(int index) //cambiar el estado de unlocked de un skin
    {
        skinInfo[index].skinUnlocked = true;
    }

    //Level selector
    public int GetCurrentLevel() //saber cual es el nivel actual
    {
        if(currentLevelGlobal >= levelInfo.Length - 1)
        currentLevelGlobal = levelInfo.Length - 1;

        return currentLevelGlobal;
    }

    public void SetCurrentLevel(int index) //cambiar el nuevo currentlevel
    {
        currentLevelGlobal = index;
    }

    public bool GetUnlocked(int index) //saber si un nivel esta unlocked o no
    {
        return levelInfo[index].unlocked;
    }

    public void SetUnlocked(int index) //cambiar el estado de unlocked
    {
        levelInfo[index].unlocked = true;
    }
    public int GetHoneycombs(int index) 
    {
        int resultado = 0;
        if (index <= levelInfo.Length -1)
        {
            for (int x = 0; x < levelInfo[index].specificHoneycombs.Length; x++)
            {
                if (levelInfo[index].specificHoneycombs[x] == true) 
                {
                    resultado++;
                }
            }
        }
        return resultado;
    }
    public bool GetSpecificHoneycombs(int index, int honeycombIndex) //obtener especificamente cuales honeycombs se agarraron
    {
        if(index > levelInfo.Length -1)
        index = levelInfo.Length -1;
        
        return levelInfo[index].specificHoneycombs[honeycombIndex];
    }

    public void SetSpecificHoneycombs(int index, int honeycombIndex) //cambiar cual honeycomb ya se agarró
    {
        levelInfo[index].specificHoneycombs[honeycombIndex] = true;
    }

    public float GetRecordTime(int index) //saber cual es el record time que tiene
    {
        return levelInfo[index].recordTime;
    }

    public void SetRecordTime(int index, float newRecord) //cambiar su tiempo record
    {
        if (levelInfo[index].recordTime == 0) 
        {
            levelInfo[index].recordTime = newRecord;
        }
        else if (newRecord < levelInfo[index].recordTime) 
        {
            levelInfo[index].recordTime = newRecord;
        }
    }


    public void SaveData() 
    {
        for (int i = 0; i < levelInfo.Length; i ++) //para guardar unlocked
        {
            PlayerPrefs.SetInt("LevelUnlocked"+i, levelInfo[i].unlocked ? 1 : 0);
        }

        for (int i = 0; i < levelInfo.Length; i++) //para guardar recortime
        {
            PlayerPrefs.SetFloat("LevelRecordtime" + i, levelInfo[i].recordTime);
        }

        for (int i = 0; i < levelInfo.Length; i++) //para guardar especificamente cual honeycomb se agarró
        {
            for (int x = 0; x < levelInfo[i].specificHoneycombs.Length; x++)
            {
                PlayerPrefs.SetInt("Specific Level" + i + "Honeycomb" + x, levelInfo[i].specificHoneycombs[x] ? 1 : 0);
            }
        }

        for (int i = 0; i < skinInfo.Length; i++) //para guardar unlocked de skins
        {
            PlayerPrefs.SetInt("SkinUnlocked" + i, skinInfo[i].skinUnlocked ? 1 : 0);
        }

        PlayerPrefs.SetInt("CurrentLevelGlobal", currentLevelGlobal);
        PlayerPrefs.SetInt("HoneycombsGlobal", honeycombsGlobal);
        PlayerPrefs.SetInt("SkinSelected", skinSelected);

    }

    public void LoadData() 
    {
        for (int i = 0; i < levelInfo.Length; i++) //para cargar unlocked
        {
            levelInfo[i].unlocked = PlayerPrefs.GetInt("LevelUnlocked" + i) > 0 ? true : false;
        }

        for (int i = 0; i < levelInfo.Length; i++) //para cargar recordtime
        {
            levelInfo[i].recordTime = PlayerPrefs.GetFloat("LevelRecordtime" + i);
        }

        for (int i = 0; i < levelInfo.Length; i++) //para cargar especificamente cual honeycomb se agarró
        {
            for (int x = 0; x < levelInfo[i].specificHoneycombs.Length; x++)
            {
                levelInfo[i].specificHoneycombs[x] = PlayerPrefs.GetInt("Specific Level" + i + "Honeycomb" + x) > 0 ? true : false;
            }
        }

        for (int i = 0; i < skinInfo.Length; i++) //para cargar unlocked
        {
            skinInfo[i].skinUnlocked = PlayerPrefs.GetInt("SkinUnlocked" + i) > 0 ? true : false;
        }

        currentLevelGlobal = PlayerPrefs.GetInt("CurrentLevelGlobal");
        honeycombsGlobal = PlayerPrefs.GetInt("HoneycombsGlobal");
        skinSelected = PlayerPrefs.GetInt("SkinSelected");

    }

    public void ResetData() 
    {
        PlayerPrefs.DeleteAll();
    }

}

[Serializable] public class LevelInfo 
{
    public bool unlocked = false;
    public float recordTime;
    public bool[] specificHoneycombs;
}


[Serializable] public class SkinInfo
{
    public bool skinUnlocked = false;
}

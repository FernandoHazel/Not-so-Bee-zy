using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] LevelData levelData; //el scriptableobject que guarda la info
    [SerializeField] AudioClip selectSound;
    public void ModifyAudio(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    public void ResetData() 
    {
        AudioManager.Instance.PlaySfxOnce(selectSound);
        levelData.ResetData();
        levelData.LoadData();
        levelData.SaveData();
    }
}

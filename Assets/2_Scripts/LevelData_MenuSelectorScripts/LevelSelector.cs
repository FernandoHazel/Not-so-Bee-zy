﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventBus;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private InterstitialAds interstitialAds;
    [SerializeField] AudioClip selectSound, nextSound, previousSound;
    int currentSelectedLevel, maxLevels, honeycombCounter;
    [SerializeField] Button startButton, rightButton, leftButton;
    [SerializeField] GameObject warningPanel, levelViewer;
    [SerializeField] Image levelLock;
    public GameObject [] levels;
    TMPro.TextMeshProUGUI timerText;
    [SerializeField] LevelData levelData; //el scriptableobject que guarda la info
    [SerializeField] GameObject[] levelModels;
    public string privacyPolicyLink = "https://fhazel.com.mx/Resources/Fhazel-Politica-de-Privacidad.pdf";


    void Start()
    {
        maxLevels = levels.Length - 1;

        levelData.LoadData();
        levelData.SetUnlocked(0);
        levelData.SaveData();

        currentSelectedLevel = levelData.GetCurrentLevel();
        levels[currentSelectedLevel].gameObject.SetActive(true);
        levelModels[currentSelectedLevel].gameObject.SetActive(true);
        for (int i = 0; i < levels.Length; i++) //para cargar sus tiempo record
        {
            timerText = levels[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
            float time = levelData.GetRecordTime(i);
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("Record Time: {0:00}:{1:00}", minutes, seconds);

            honeycombCounter = levelData.GetHoneycombs(i);
            for (int x = 0; x < honeycombCounter; x++) 
            {
                levels[i].gameObject.transform.GetChild(x+2).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

        }
    }

    public void NextLevel() 
    {
        AudioManager.audioManager.PlaySfxOnce(nextSound);
        leftButton.interactable = true;
        if (currentSelectedLevel >= maxLevels)
        {
            //Change button selection to another button.
            GameEventBus.Publish(GameEventType.UNITERACTABLE);

            rightButton.interactable = false;
            return;
        }
        else
        {
            rightButton.interactable = true;
            levels[currentSelectedLevel].gameObject.SetActive(false);

            if(currentSelectedLevel <= maxLevels)
            currentSelectedLevel++;
            
            levels[currentSelectedLevel].gameObject.SetActive(true);
            levelModels[currentSelectedLevel - 1].gameObject.SetActive(false);
            levelModels[currentSelectedLevel].gameObject.SetActive(true);
            UpdateLock();
        }
    }

    public void PreviousLevel()
    {
        AudioManager.audioManager.PlaySfxOnce(previousSound);
        rightButton.interactable = true;
        if (currentSelectedLevel <= 0)
        {
            //Change te gamepad selection to another button
            GameEventBus.Publish(GameEventType.UNITERACTABLE);

            leftButton.interactable = false;
            return;
        }
        else 
        {
            leftButton.interactable = true;
            levels[currentSelectedLevel].gameObject.SetActive(false);
            currentSelectedLevel--;
            levels[currentSelectedLevel].gameObject.SetActive(true);
            levelModels[currentSelectedLevel + 1].gameObject.SetActive(false);
            levelModels[currentSelectedLevel].gameObject.SetActive(true);
            UpdateLock();
        }
    }

    public void StartButton(string SceneToLoad) 
    {
        //interstitialAds.ShowAd();
        StartCoroutine(DelayedLoad(SceneToLoad));
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(DelayedLoad(sceneName));
    }
    public void QuitGame()
    {
        Debug.Log("Exited game");
        Application.Quit();
    }
    void UpdateLock()
    {
        if (!levelData.GetUnlocked(currentSelectedLevel))
        {
            startButton.interactable = false;
            levelLock.enabled = true;
        }
        else
        {
            startButton.interactable = true;
            levelLock.enabled = false;
        }
    }

    public void NewGame(string sceneName)
    {
        levelData.ResetData();
        StartCoroutine(DelayedLoad(sceneName));
    }

    public void ShowWarning()
    {
        warningPanel.SetActive(true);
        levelViewer.SetActive(false);
    }

    public void HideWarning()
    {
        warningPanel.SetActive(false);
        levelViewer.SetActive(true);
    }

    IEnumerator DelayedLoad(string SceneToLoad)
    {
        AudioManager.audioManager.PlaySfxOnce(selectSound);
        yield return new WaitForSeconds(selectSound.length);

        if (SceneToLoad == "Nivel Oficial ")
        {
            SceneManager.LoadScene(SceneToLoad + currentSelectedLevel);
        }
        else
        {
            SceneManager.LoadScene(SceneToLoad);
        }
        
    }

    public void GoToPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyLink);
    }
}

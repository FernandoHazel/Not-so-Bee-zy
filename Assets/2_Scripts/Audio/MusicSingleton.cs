﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSingleton : MonoBehaviour
{

    public static MusicSingleton Instance;
    string sceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else 
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Store" && SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "Settings" && SceneManager.GetActiveScene().name != "Credits") 
        {
            Destroy(this.gameObject);
        }
    }
}

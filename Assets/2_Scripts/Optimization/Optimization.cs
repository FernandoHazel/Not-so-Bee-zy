using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimization : MonoBehaviour
{
    //We are using a Huawei Y9 screen resolution as defoult
    [Tooltip("This is the defoult screen resolution")]
    [SerializeField] private int defoultScreenWidth = 1080;
    [SerializeField] private int defoultScreenHeight =  2340;

    public static Optimization Instance;
    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        Application.targetFrameRate = 30;
        Screen.SetResolution(defoultScreenWidth, defoultScreenHeight, false);
    }
}

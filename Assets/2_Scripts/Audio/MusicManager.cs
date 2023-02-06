using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource hiveNSneak = default;
    [SerializeField] AudioSource hiveNSneakPause = default ;

    private void Start()
    {
        hiveNSneak.mute = false;
        hiveNSneakPause.mute = true;

    }
    private void Update()
    {
        if (GameManager.gameIsPaused)
        {
            hiveNSneak.mute = true;
            hiveNSneakPause.mute = false;

        }
        else 
        {
            hiveNSneak.mute = false;
            hiveNSneakPause.mute = true;
        
        }
    }

}

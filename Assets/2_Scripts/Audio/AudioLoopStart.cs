using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopStart : MonoBehaviour
{
    [SerializeField] private AudioClip audioToLoop;
    void Start()
    {
        AudioManager.audioManager.PlaySfxLoop3D(audioToLoop, gameObject.transform);
    }

}

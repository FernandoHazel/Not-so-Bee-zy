using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField] private AudioClip testAudio01, testAudio02;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.audioManager.PlaySfxLoop3D(testAudio01,gameObject.transform);
    }
}

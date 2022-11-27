using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    //[SerializeField] private AudioSource []audioChannelLoop;
    [SerializeField] private AudioSource[] audioChannelLoop3D;
    [SerializeField] private AudioSource[] audioChannelOnce3D;

    [SerializeField] private AudioSource[] audioChannelLoop;
    [SerializeField] private AudioSource[] audioChannelOnce;
    private void Awake() {
        audioManager = this;
    }
    public void PlaySfxLoop(AudioClip clip) 
    {
        foreach (var channel in audioChannelLoop) 
        {
            if (channel.clip == null) 
            {
                channel.clip = clip;
                channel.Play();
                break;
            }
        }
    }

    public void ClearLoopChannels()
    {
        foreach (var channel in audioChannelLoop) 
        {
            channel.clip = null;
        }
    }
    public void PlaySfxOnce(AudioClip clip)
    {
        foreach (var channel in audioChannelOnce)
        {
            if (channel.clip == null)
            {
                channel.clip = clip;
                channel.Play();
                StartCoroutine(ClearChannel(channel));
                break;
            }
        }
    }

    /* para escuchar los sonidos en un espacio 3d y que no se escuchen todos en el mismo plano (por ejemplo el zumbido de un enemigo
    que est� lejos no deber�a escucharse al inicio, o cuando te persiga, que el sonido se mueva con �l)
    */
    public void PlaySfxOnce3D(AudioClip clip, Transform sourcePosition)
    {
        for (int i = 0; i < audioChannelOnce3D.Length; i++)
        {
            if (audioChannelOnce3D[i].clip == null)
            {
                //pon el audiosource en la posici�n del objeto que lo emite
                audioChannelOnce3D[i].transform.position = sourcePosition.position;

                //para que el audiosource sea hijo del objeto que lo emite, por si se mueve, que tambi�n se mueva el audiosource
                audioChannelOnce3D[i].transform.parent = sourcePosition.transform;


                audioChannelOnce3D[i].clip = clip;
                audioChannelOnce3D[i].Play();
                
                break;
            }
            StartCoroutine(ClearChannel(audioChannelOnce3D[i]));
        }
    }

    public void PlaySfxLoop3D(AudioClip clip, Transform sourcePosition)
    {
        for (int i = 0; i < audioChannelLoop3D.Length; i++)
        {
            if (audioChannelLoop3D[i].clip == null)
            {
                //pon el audiosource en la posici�n del objeto que lo emite
                audioChannelLoop3D[i].transform.position = sourcePosition.position;

                //para que el audiosource sea hijo del objeto que lo emite, por si se mueve, que tambi�n se mueva el audiosource
                audioChannelLoop3D[i].transform.parent = sourcePosition.transform;


                audioChannelLoop3D[i].clip = clip;
                audioChannelLoop3D[i].Play();
                break;
            }
        }
    }
    IEnumerator ClearChannel(AudioSource channel)
    {
        yield return new WaitForSeconds(channel.clip.length);
        channel.clip = null;
    }
}

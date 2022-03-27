using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollem : MonoBehaviour
{
    //public ParticleSystem grabPollen;
    [SerializeField] AudioClip pollenSound;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            {
                //grabPollen.Play();
                Debug.Log("Grab");
            }
    }

}

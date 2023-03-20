using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pollem : MonoBehaviour
{
    [SerializeField] GameObject pollenModel;
    [SerializeField] AudioClip pollenSound;
    private Collider col;

    private void Awake() 
    {
        col = GetComponent<Collider>(); 
    }

    /* private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            AudioManager.audioManager.PlaySfxOnce(pollenSound);
            col.enabled = false;
            pollenModel.SetActive(false);

        }
    } */

}

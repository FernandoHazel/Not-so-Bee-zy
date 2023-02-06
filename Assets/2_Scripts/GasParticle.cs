using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasParticle : MonoBehaviour
{
    bool apagar = false;

    private void Update()
    {
        if (gameObject.GetComponent<BoxCollider>().enabled == false) 
        {
            if (apagar == false) 
            {
                apagar = true;
                StartCoroutine(Prender());
            }
        }
    }
    IEnumerator Prender() 
    {
        yield return new WaitForSeconds(5);
        apagar = false;
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<BoxCollider>().enabled = true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAB_Behavior : MonoBehaviour
{
    public static bool playerFell;
    private void OnEnable() 
    {
        //Si el jugador se cayó al precipicio el jugador no verá el botón en la pantalla
        //Igualmente si no se cargó ningún anuncio no aparecemos el botón
        if(playerFell || !AdsInitializer.AdReady)
        {
            gameObject.SetActive(false);
        }
    }
}

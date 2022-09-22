using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//WAYPOINTS ENEMIGO
public class Platform : MonoBehaviour
{
    public int estados = 0;
    public float speed = 10;

    public Transform[] waypoints;

    public float margenDeError = 0.5f;

    bool irAdelante = true;
    public int casos = 1;

    public float tiempoDeEspera = 2;
    public float tiempoDeEsperaInterno;

    [SerializeField] AudioClip woosh;

    void Start()
    {
        tiempoDeEsperaInterno = 0;
    }

    void MaquinaDeEstados(int casos)
    {
        switch (casos)
        {
            case 1:  //Mover en loop 
                transform.position = Vector3.LerpUnclamped(transform.position, waypoints[estados].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, waypoints[estados].position) < margenDeError)
                {
                    estados++;
                    tiempoDeEsperaInterno = tiempoDeEspera;
                    StartCoroutine(WaitToPlay());
                    if (estados >= waypoints.Length)
                    {
                        estados = 0;
                        tiempoDeEsperaInterno = tiempoDeEspera;
                    }
                }

                break;
            case 2:  //Mover ida y vuelta
                transform.position = Vector3.MoveTowards(transform.position, waypoints[estados].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, waypoints[estados].position) < margenDeError)
                {
                    if (irAdelante)
                    {
                        estados++;
                        if (estados >= waypoints.Length)
                        {
                            estados--;
                            irAdelante = false;
                        }
                    }
                    else
                    {
                        estados--;
                        if (estados < 0)
                        {
                            estados++;
                            irAdelante = true;
                        }
                    }
                }
                break;
            case 3:  //Mover ida una sola vez
                transform.position = Vector3.MoveTowards(transform.position, waypoints[estados].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, waypoints[estados].position) < margenDeError)
                {
                    estados++;
                    if (estados >= waypoints.Length)
                    {
                        estados--;
                        casos = 0;
                    }
                }
                break;
            case 4:  //Mover en reversa una sola vez
                transform.position = Vector3.MoveTowards(transform.position, waypoints[estados].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, waypoints[estados].position) < margenDeError)
                {
                    estados--;
                    if (estados < 0)
                    {
                        estados++;
                        casos = 0;
                    }
                }
                break;
        }
    }
    void FixedUpdate()
    {

        tiempoDeEsperaInterno -= Time.deltaTime;

        if (tiempoDeEsperaInterno > 0)
        {
            return;
        }
        MaquinaDeEstados(casos);
    }

    IEnumerator WaitToPlay() 
    {
        yield return new WaitForSeconds(.9f);
        AudioManager.audioManager.PlaySfxOnce3D(woosh, transform);
    }
}

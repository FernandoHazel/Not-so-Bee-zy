using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WAYPOINTS ENEMIGO
public class Enemy : MonoBehaviour
{
    public int estados = 0;
    public float speed= 1.4f;
    float speedInicial;
    public float maxSpeed = 1.8f;
    public Transform[] waypoints;
    public float margenDeError = 0.5f;
    bool irAdelante = true;
    public int casos = 1;

    public Material enemyMaterial;
    public Light visorLight;
    public Color startColor;
    public Color endColor;
    public float rotationSpeed = 5;

    public Player3D player3DScript;

    public Transform player;
    Vector3 direccion;
    bool check = false;

    public Transform playerLastPlace;
    bool lastPlaceCheck = false;

    public EnemyView enemyViewScript;


    void Start()
    {
        speedInicial = speed;
        enemyMaterial = GetComponent<MeshRenderer>().material;
        enemyMaterial.color = startColor;
        visorLight.color = startColor;
        transform.position = waypoints[0].position; //Al iniciar, posicionarse en el primer waypoint
    }

    void MaquinaDeEstados(int casos)
    {
        switch (casos)
        {
            case 1:  //Mover en loop 

                transform.position = Vector3.MoveTowards(transform.position, waypoints[estados].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, waypoints[estados].position) < margenDeError)
                {
                    estados++;
                    if (estados >= waypoints.Length)
                    {
                        estados = 0;
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
        }
    }

    void RotarEnemigoWaypoint()
    {
        Vector3 relativePos = waypoints[estados].position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
    }

    void RotarEnemigoPlayer()
    {
        Vector3 relativePos = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
    }

    void RotarEnemigoLastPlace()
    {
        Vector3 relativePos = playerLastPlace.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
    }
    void Sight() 
    {

        direccion = player.position - transform.position;
        Debug.DrawRay(transform.position, direccion, Color.green, 0.1f);

        Ray rayito = new Ray(transform.position, direccion);
        RaycastHit infoDelRayito;

        if (Physics.Raycast(rayito, out infoDelRayito))
        {
            if ((infoDelRayito.collider.tag == "Player") && (enemyViewScript.enemySees == true)&&(player3DScript.youWon==false))
            {
                speed = maxSpeed;
                RotarEnemigoPlayer();
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                enemyMaterial.color = endColor;
                visorLight.color = endColor;
                //transform.forward = new Vector3(direccion.x, 0, direccion.z);
                check = false;
                playerLastPlace.position = player.position;
                lastPlaceCheck = true;
                return;
            }

            if (lastPlaceCheck == true)
            {
                RotarEnemigoLastPlace();
                transform.position = Vector3.MoveTowards(transform.position, playerLastPlace.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, playerLastPlace.position) < margenDeError)
                {
                    lastPlaceCheck = false;
                }
            }

            else
            {

                if (check == false)
                {
                    enemyMaterial.color = startColor;
                    visorLight.color = startColor;
                    speed = speedInicial;
                    int waypointCercano = 0;
                    float distanciaW = 0;
                    for (int i = 0; i < waypoints.Length; i++)
                    {
                        if ((distanciaW) > (Vector3.Distance(transform.position, waypoints[i].position)) || (distanciaW == 0))
                        {
                            distanciaW = (Vector3.Distance(transform.position, waypoints[i].position));
                            waypointCercano = i;
                        }
                    }
                    estados = waypointCercano;
                    check = true;
                }
                MaquinaDeEstados(casos);
                RotarEnemigoWaypoint();
            }
        }

    }

    void Update()
    {
        
        if (player3DScript.youWon == true)
        {
            return;
        }
        if (player3DScript.youLost == true)
        {
            transform.position = waypoints[0].position;
            lastPlaceCheck = false;
            enemyViewScript.enemySees = false;
            return;
        }

        else
        {
            Sight();

        }
        //transform.forward = new Vector3((waypoints[estados].position.x)-transform.position.x, 0, (waypoints[estados].position.z)-transform.position.z);

    }
}

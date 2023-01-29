using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//WAYPOINTS ENEMIGO
public class EnemyNav : MonoBehaviour
{
    public int estados = 0;


    public float speed = 2;
    public float maxSpeed = 2.5f;
    public float rotationSpeed = 5;


    public Transform[] waypoints;
    float margenDeError = 2;
    bool irAdelante = true;
    public int casos = 1;
    int waypointCercano = 0;

    public Material enemyMaterial;
    public Light visorLight;
    public Light bodyLight;
    public Color startColor;
    public Color endColor;


    public Player3D player3DScript;

    public Transform player;
    Vector3 direccion;
    bool check = false;

    public Transform playerLastPlace;
    bool lastPlaceCheck = false;
    //public Transform[] lastPlaceWaypoints; //en caso de que se necesite rotar para buscar al jugador

    public EnemyView enemyViewScript;

    NavMeshAgent agent;
    Vector3 posInicial;

    bool empezarLookForPlayer = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyMaterial = GetComponent<MeshRenderer>().material;
        enemyMaterial.color = startColor;
        visorLight.color = startColor;
        bodyLight.color = startColor;
        posInicial = transform.position;
        //transform.position = waypoints[0].position; //Al iniciar, posicionarse en el primer waypoint

    }

    void MaquinaDeEstados(int casos)
    {
        switch (casos)
        {
            case 1:  //Mover en loop 

                agent.destination = (waypoints[estados].position);
                agent.updateRotation = true;
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
                agent.destination = (waypoints[estados].position);
                agent.updateRotation = true;
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

            case 3:  //Rotar de lado a lado
                if (transform.position != posInicial)
                {
                    agent.destination = posInicial;
                    agent.updateRotation = true;
                    if (Vector3.Distance(transform.position, posInicial) < margenDeError)
                    {
                        Vector3 relativePos = waypoints[estados].position - transform.position;
                        Quaternion rotation = Quaternion.LookRotation(relativePos);
                        Quaternion current = transform.localRotation;
                        transform.localRotation = Quaternion.Lerp(current, rotation, Time.deltaTime * (rotationSpeed/2));

                        if (Quaternion.Angle(rotation, current) <= 3)
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
                    }

                }
                break;
            case 4:
                agent.destination = (waypoints[estados].position);
                agent.updateRotation = false;
                if (Vector3.Distance(transform.position, waypoints[estados].position) < margenDeError)
                {
                    estados++;
                    if (estados >= waypoints.Length)
                    {
                        transform.position = posInicial;
                        estados = 0;
                    }

                }

                break;

                /*case 4:  //Rotar de lado a lado

                    Vector3 relativePos2 = lastPlaceWaypoints[estados].position - transform.position;
                    Quaternion rotation2 = Quaternion.LookRotation(relativePos2);
                    Quaternion current2 = transform.localRotation;
                    transform.localRotation = Quaternion.Lerp(current2, rotation2, Time.deltaTime * (rotationSpeed/2));
                    if (Quaternion.Angle(rotation2, current2) <= 12)
                    {
                        estados++;
                        if (estados >= lastPlaceWaypoints.Length) 
                        {
                        estados--;
                        lastPlaceCheck = false;
                        }
                    }
                    break;   */


        }
    }

    void RotarEnemigoPlayer()
    {
        Vector3 relativePos = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
    }
    void Sight()
    {

        direccion = player.position - transform.position;

        Ray rayito = new Ray(transform.position, direccion);
        RaycastHit infoDelRayito;

        if (Physics.Raycast(rayito, out infoDelRayito))
        {
            if ((infoDelRayito.collider.tag == "Player") && (enemyViewScript.enemySees == true) && (player3DScript.youWon == false))
            {
                agent.speed = maxSpeed;
                agent.destination = player.position;
                agent.updateRotation = true;

                RotarEnemigoPlayer();

                enemyMaterial.color = endColor;
                visorLight.color = endColor;
                bodyLight.color = endColor;
                check = false;

                playerLastPlace.position = new Vector3(player.position.x+.2f,player.position.y,player.position.z+.2f);
                playerLastPlace.rotation = player.rotation;
                lastPlaceCheck = true;

                return;
            }

            if (lastPlaceCheck == true)
            {
                agent.speed = maxSpeed;
                agent.destination = playerLastPlace.position;
                agent.updateRotation = true;

                if (Vector3.Distance(transform.position, playerLastPlace.position) < margenDeError)
                {
                    if (empezarLookForPlayer == false)
                    {
                        empezarLookForPlayer = true;
                        StartCoroutine(LookForPlayer());
                    }
                }
            }


            else
            {

                if (check == false)
                {
                    agent.speed = speed;
                    enemyMaterial.color = startColor;
                    visorLight.color = startColor;
                    bodyLight.color = startColor;

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

    }


    IEnumerator LookForPlayer()
    {
        yield return new WaitForSeconds(.4f);
        empezarLookForPlayer = false;
        lastPlaceCheck = false; // en caso de que se quiera regresar al comportamiento donde gira a los lados, se debe comentar esta linea y descomentar la de abajo "maquinadeestados"
        //MaquinaDeEstados(4);

    }

}

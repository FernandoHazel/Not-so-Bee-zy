using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WAYPOINTS ENEMIGO
public class Waypoints : MonoBehaviour
{
    public int estados = 0;
    public float speed = 10;
    public Transform[] waypoints;
    public float margenDeError = 0.5f;
    bool irAdelante = true;
    public int casos = 1;

    public Material visorMaterial;
    public Material enemyMaterial;
    public Light visorLight;
    public Color startColor;
    public Color endColor;
    public float rotationSpeed = 5;

    public Player3D player3DScript;

    void Start()
    {
        enemyMaterial = GetComponent<MeshRenderer>().material;
        visorMaterial.color = startColor;
        enemyMaterial.color = startColor;
        visorLight.color = startColor;
        transform.position = waypoints[0].position; //Al iniciar, posicionarse en el primer waypoint
        //Debug.Log("Elige uno de los cuatro movimientos escribiendo el numero en 'casos'");
        //Debug.Log("Tambien lo puede cambiar presionando los numeros en el teclado");
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

    void RotarEnemigo() 
    {
        Vector3 relativePos = waypoints[estados].position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
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
            return;
        }

        else 
        {

            RotarEnemigo();
            MaquinaDeEstados(casos);

        }
        //transform.forward = new Vector3((waypoints[estados].position.x)-transform.position.x, 0, (waypoints[estados].position.z)-transform.position.z);

    }
}

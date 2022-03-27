using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SIGHT ENEMIGO
public class Sight : MonoBehaviour
{
    public Waypoints wp;
    public Transform player; 
    Vector3 direccion;
    bool check = false;
    public float maxSpeed;

    public Player3D enemyVision;


    //bool parpadeoTime = false;
    //public GameObject vision;

    // Start is called before the first frame update
    void Start()
    {

    }

    void RotarEnemigo()
    {
        Vector3 relativePos = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * wp.rotationSpeed);
    }

    //void Parpadeo() 
    //{
        //vision.SetActive(!parpadeoTime);
    //}


    // Update is called once per frame
    void Update()
    {

        direccion = player.position - transform.position;
        Debug.DrawRay(transform.position, direccion, Color.green, 0.1f);

        Ray rayito = new Ray(transform.position, direccion);
        RaycastHit infoDelRayito;

        if (Physics.Raycast(rayito, out infoDelRayito))
        {
            if ((infoDelRayito.collider.tag == "Player")) //&& (enemyVision.enemyViewScript.enemySees == true)
            {
                //Parpadeo();
                //parpadeoTime = !parpadeoTime;

                wp.speed = maxSpeed;
                wp.enabled = false;
                transform.position = Vector3.MoveTowards(transform.position,player.position, wp.speed * Time.deltaTime);
                wp.visorMaterial.color = wp.endColor;
                wp.enemyMaterial.color = wp.endColor;
                wp.visorLight.color = wp.endColor;
                //transform.forward = new Vector3(direccion.x, 0, direccion.z);
                RotarEnemigo();
                check = false; 
            }


            else
            {
                //vision.SetActive(true);
                //wp.myMaterial.color = new Color32(188, 22, 24, 255);
                if (check == false) 
                {
                    wp.visorMaterial.color = wp.startColor;
                    wp.enemyMaterial.color = wp.startColor;
                    wp.visorLight.color = wp.startColor;
                    wp.speed = 2;
                    int waypointCercano = 0;
                    float distanciaW = 0;
                    for (int i = 0; i < wp.waypoints.Length; i++)
                    {
                        if ((distanciaW) > (Vector3.Distance(transform.position, wp.waypoints[i].position)) || (distanciaW == 0))
                        {
                            distanciaW = (Vector3.Distance(transform.position, wp.waypoints[i].position));
                            waypointCercano = i;
                        }
                    }
                    wp.estados = waypointCercano;
                    check = true;
                }
                wp.enabled = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterThievesTest : MonoBehaviour
{

    CharacterController controller;
    float speed = 5;
    Vector3 movement;
    float gravity = 60;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent < CharacterController>();
        

    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        movement.y = movement.y - (gravity * Time.deltaTime);
        controller.Move(movement * speed * Time.deltaTime);
        movement.y = 0;
    }
}

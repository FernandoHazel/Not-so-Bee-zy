using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCameraControl : MonoBehaviour
{
    //[SerializeField] Transform target;
    [SerializeField] private float speed = .125f;
    //[SerializeField] Vector3 inputVelocity = new Vector3(1, 1, 1);
    private float horizontalInput;
    private float fowardInput;
    //private InputMaster inputMaster;
    /* private void Awake() {
        inputMaster = new InputMaster();
    } */
    void LateUpdate()
    {
        //Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        fowardInput = Input.GetAxis("Vertical");

        //Movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime * fowardInput);
        //transform.Translate(Vector3.SmoothDamp(transform.position, inputMaster.Player.move.ReadValue<Vector2>(), ref inputVelocity, speed));

        transform.Translate(Vector3.left * speed * Time.deltaTime * horizontalInput * -1);
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime * -1);
        }
    }
}

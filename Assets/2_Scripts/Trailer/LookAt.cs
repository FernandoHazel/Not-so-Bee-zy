using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform ObjectToLook;
    [SerializeField] private float speed;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(ObjectToLook);
    }

}

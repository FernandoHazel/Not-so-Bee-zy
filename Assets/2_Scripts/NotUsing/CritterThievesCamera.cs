using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterThievesCamera : MonoBehaviour
{
    public Transform view;
    public float transitionSpeed = 1f;

    private void Start()
    {
    }


    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, view.position, (Time.deltaTime * transitionSpeed));
    }
}

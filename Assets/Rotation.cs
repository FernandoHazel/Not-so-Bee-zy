using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1;

    private void LateUpdate() {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}

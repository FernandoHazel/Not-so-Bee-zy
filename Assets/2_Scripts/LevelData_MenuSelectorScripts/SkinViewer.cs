using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinViewer : MonoBehaviour
{
    [SerializeField] float speed;
    private bool isActive;
    private void OnEnable() 
    {
        isActive = true;
    }
    private void OnDisable() 
    {
        isActive = false;
    }
    void Update()
    {
        if(isActive)
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed = 1f;
    public Transform currentView;

    private void Start()
    {
        currentView = views[0];
    }


    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, (Time.deltaTime * transitionSpeed));
        Vector3 currentAngle = new Vector3(
         Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));
        transform.eulerAngles = currentAngle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed = 1f;
    public Transform currentView;

    private bool canMove = false;

    private void Start()
    {
        currentView = views[0];
    }


    private void LateUpdate()
    {
        //We only moves the camera if it is not in the current view position
        CheckPosition();

        if (canMove)
        MoveCamera();
    }

    private void MoveCamera()
    {
        //Move the camera from its actual position to the new view position 
        //With a smooth motion
        transform.position = Vector3.Lerp(transform.position, currentView.position, (Time.deltaTime * transitionSpeed));
        Vector3 currentAngle = new Vector3(
         Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));
        transform.eulerAngles = currentAngle;
    }

    private void CheckPosition()
    {
        //When we reach the target we stop the camera movement
        if (transform.position == currentView.position)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }
}

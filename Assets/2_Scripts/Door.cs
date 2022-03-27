using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator door;
    public void OpenDoor() 
    {
        door.SetBool("CloseDoor",false);
        door.SetBool("OpenDoor",true);
    
    }

    public void CloseDoor()
    {
        door.SetBool("OpenDoor", false);
        door.SetBool("CloseDoor",true);

    }


}

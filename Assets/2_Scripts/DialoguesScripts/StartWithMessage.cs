using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class StartWithMessage : MonoBehaviour
{
    //If this object is in the scene, the level
    //Will start with a dialogue
    [Tooltip("Drag the Dialogue instance that will start")]
    [SerializeField] GameObject StarterDialogue;
    void Start()
    {
        StarterDialogue.GetComponent<Dialogues>().PublicStarterDialogue();
    }

}

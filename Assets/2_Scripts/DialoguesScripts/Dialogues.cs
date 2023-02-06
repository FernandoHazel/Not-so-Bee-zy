using UnityEngine;
using EventBus;

public class Dialogues : MonoBehaviour
{
    [SerializeField] private message[] dialogues;
    private InputMaster inputMaster;
    private int currentDialogue = 0;
    [SerializeField] UserInterface ui = default;
    bool showDialogue = true;
    [SerializeField] CameraControl CameraControl = default;
    private Transform defaultPosition;
    private bool canUpdate = false; //This is an identifier that limits only the desired instance to act

    #region //Set
    private void Awake() {
        inputMaster = new InputMaster();
    }
    private void OnEnable()
    {
        inputMaster.Enable();
    }
    private void OnDisable()
    {
        inputMaster.Disable();
    }
    #endregion

    //If the player enters and this object has not showed its dialogues
    //we activate them
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && currentDialogue == 0)
        {
            ActiveDialogue();
        }
    }
    //I create this method to avoid making public 
    //The ActiveDialogue method
    public void PublicStarterDialogue()
    {
        ActiveDialogue();
    }
    private void ActiveDialogue()
    {
        //Notify other classes that we are in dialogues
        GameEventBus.Publish(GameEventType.DIALOGUE);

        //Only the selected class can be updated
        canUpdate = true;

        //We set the actual view as the default so we can get back if the camera made a zoom
        defaultPosition = CameraControl.currentView;

        Player3D.inDialogue = true;
        showDialogue = true;

        //Here we write the message on the UI
        ui.ShowText(dialogues[currentDialogue].messageText);
    }
    void StopDialogues()
    {
        //Let the player to move again
        showDialogue = false;
        Player3D.inDialogue = false;

        //Notify other classes
        GameEventBus.Publish(GameEventType.NORMALGAME);
        GameEventBus.Publish(GameEventType.FINISHDIALOGUE);

        //Return the camera to the default position
        CameraControl.currentView = defaultPosition;
    }

    void Update()
    {
        //Next dialogue.
        if (inputMaster.Player.Action.triggered && showDialogue)
        {
            NextDialogue();
        }
    }

    //We create this separate method because we call it 
    //by input or by button depending on the the platform
    public void NextDialogue()
    {
        //If we run out of dialogues disable them and return to normal game
        if(currentDialogue >= dialogues.Length - 1) 
        {
            StopDialogues();
        }
        else if(canUpdate) //Only if this is the desired instance will be updated
        {
            //Advance to the next dialogue.
            currentDialogue++;
            ui.ShowText(dialogues[currentDialogue].messageText);

            //We check if the dialogue has zoom
            if (dialogues[currentDialogue].camPosition != null)
            {
                //If true we move the camera to that position
                CameraControl.currentView = dialogues[currentDialogue].camPosition;
            }
            else
            {
                //If not we set the camera on the default position
                CameraControl.currentView = defaultPosition;
            }
        }
    }

    //We make a new class because I need each element of
    //the list of dialogs besides the text check if something needs to be zoomed
    //or if it is the last message of a serie of messages
    [System.Serializable]
    public class message
    {
        [TextArea] public string messageText;
        public Transform camPosition; //Here we need a transform to put the camera on that position ONLY if it is not null
    }
}

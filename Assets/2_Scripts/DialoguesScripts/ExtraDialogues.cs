using UnityEngine;
using EventBus;

public class ExtraDialogues : MonoBehaviour
{
    [TextArea] public string[] extraDialogues;
    private InputMaster inputMaster;
    int currentDialogue;
    [SerializeField] GameObject dialogueImage = default;
    [SerializeField] UserInterface ui = default;
    bool showDialogue = false;
    [SerializeField] CameraControl CameraControl = default;

    bool wantMovement = false;
    private void Awake() 
    {
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
    private void OnTriggerEnter(Collider other)
    {
        //If the player enters the collider we activate the first one
        if (other.tag == "Player")
        {
            GameEventBus.Publish(GameEventType.DIALOGUE);
            ui.pauseButton.interactable = false;
            currentDialogue = 0;
            showDialogue = true;
            dialogueImage.SetActive(true);
            ui.ShowExtraText(extraDialogues[currentDialogue]);
        }
    }

    void Update()
    {
        //Next dialogue.
        if (inputMaster.Player.Action.triggered && showDialogue)
        {
            Debug.Log("Action button pressed");
            NextDialogue();
        }

        //In this dialogue we perform a little zoom in
        if (currentDialogue == 2 && wantMovement == true)
        {
            CameraControl.currentView = CameraControl.views[14];
        }
    }

    //We create this separate method because we call it 
    //by input or by button depending on the the platform
    public void NextDialogue()
    {
        //If we run out of dialogues disable them and return to normal game
        if(currentDialogue >= extraDialogues.Length - 1) 
        {
            showDialogue = false;
            Debug.Log("stop dialogue");

            //Allow the player to move again.
            GameEventBus.Publish(GameEventType.NORMALGAME);
            GameEventBus.Publish(GameEventType.FINISHDIALOGUE);
            dialogueImage.SetActive(false);
            ui.pauseButton.interactable = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
        }
        else
        {
            //Advance to the next dialogue.
            currentDialogue++;
            ui.ShowExtraText(extraDialogues[currentDialogue]);

            Debug.Log("Next");
        }
    }
}

using UnityEngine;
using EventBus;

public class TutorialDialogues : MonoBehaviour
{
    [TextArea] public string[] tutorialDialogues;
    private InputMaster inputMaster;
    int currentDialogue;
    [SerializeField] GameObject dialogueImage;
    [SerializeField] UserInterface ui = default;
    bool showDialogue = true;
    [SerializeField] CameraControl CameraControl = default;

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
    void Start()
    {
        //Start the tutorial dialogues
        GameEventBus.Publish(GameEventType.DIALOGUE);
        ui.pauseButton.interactable = false;
        currentDialogue = 0;
        showDialogue = true;
        dialogueImage.SetActive(true);
        ui.ShowExtraText(tutorialDialogues[currentDialogue]);
    }

    void Update()
    {
        //Next dialogue.
        if (inputMaster.Player.Action.triggered && showDialogue)
        {
            Debug.Log("Action button pressed");
            NextTutorialDialogue();
        }

        //In this dialogue we perform a little zoom in
        if (currentDialogue == 6)
        {
            CameraControl.currentView = CameraControl.views[1]; 
        }
    }

    //We create this separate method because we call it 
    //by input or by button depending on the the platform
    public void NextTutorialDialogue()
    {
        //If we run out of dialogues disable them and return to normal game
        if(currentDialogue >= tutorialDialogues.Length - 1) 
        {
            showDialogue = false;
            Debug.Log("stop dialogue");

            //Allow the player to move again.
            GameEventBus.Publish(GameEventType.NORMALGAME);
            GameEventBus.Publish(GameEventType.FINISHDIALOGUE);
            dialogueImage.SetActive(false);
            ui.pauseButton.interactable = true;
            this.enabled = false;
        }
        else
        {
            //Advance to the next dialogue.
            currentDialogue++;
            ui.ShowExtraText(tutorialDialogues[currentDialogue]);

            Debug.Log("Next");
        }
    }
}

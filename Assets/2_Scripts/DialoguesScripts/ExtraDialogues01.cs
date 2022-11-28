using UnityEngine;
using EventBus;

public class ExtraDialogues01 : MonoBehaviour
{
    [TextArea] public string[] extraDialogues;
    private InputMaster inputMaster;
    int currentDialogue;
    [SerializeField] GameObject dialogueImage = default;
    [SerializeField] UserInterface ui = default;
    bool showDialogue = false;
    [SerializeField] Player3D player = default;
    [SerializeField] CameraControl CameraControl = default;

    bool nextDialogueBool = false;
    [SerializeField] bool wantMovement = false;

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
        if (other.tag == "Player")
        {
            ui.pauseButton.interactable = false;
            currentDialogue = 0;
            showDialogue = true;
            ActiveDialogues();
            currentDialogue++;
        }
    }

    void Update()
    {
        if (inputMaster.Player.Action.triggered && showDialogue)
        {
            NextDialogueBool();
        }
        
        if (GameManager.gameIsPaused || player.youWon)
        {
            showDialogue = false;
        }

        else if (currentDialogue == 2 && wantMovement == true)
        {
            CameraControl.currentView = CameraControl.views[14];
        }
    }

    void StopDialogue()
    {
        if (showDialogue == false)
        {
            return;
        }
        else if (currentDialogue >= extraDialogues.Length)
        {
            showDialogue = false;
        }

    }

    //Active the dialogues and stop the game or resumes it
    public void ActiveDialogues()
    {
        if (showDialogue)
        {
            GameEventBus.Publish(GameEventType.DIALOGUE);
            dialogueImage.SetActive(true);
            ui.ShowExtraText01(extraDialogues[currentDialogue]);
            if (nextDialogueBool == true)
            {
                nextDialogueBool = false;
                currentDialogue++;
            }
        }
        else
        {
            GameEventBus.Publish(GameEventType.NORMALGAME);
            GameEventBus.Publish(GameEventType.FINISHDIALOGUE);
            dialogueImage.SetActive(false);
            ui.pauseButton.interactable = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
        }
    }

    public void NextDialogueBool()
    {
        nextDialogueBool = true;
        StopDialogue();
        ActiveDialogues();
    }
}

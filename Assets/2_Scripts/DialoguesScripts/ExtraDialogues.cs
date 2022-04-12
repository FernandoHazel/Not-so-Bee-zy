using UnityEngine;

public class ExtraDialogues : MonoBehaviour
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
    private void Awake() {
        inputMaster = new InputMaster();
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
            Debug.Log("stopdialogue");
        }

    }

    //Active the dialogues and stop the game or resumes it
    public void ActiveDialogues()
    {
        if (showDialogue)
        {
            Debug.Log("ActiveDialogues");
            //player.move = false;   //que el jugador no se pueda mover, pusimos su velocidad en 0
            player.speed = 0;
            player.rotationSpeed = 0;
            dialogueImage.SetActive(true);
            ui.ShowExtraText(extraDialogues[currentDialogue]);
            if (nextDialogueBool == true)
            {
                nextDialogueBool = false;
                currentDialogue++;
            }
        }
        else
        {
            //player.move = true;  //restaurar velocidad del jugador
            player.speed = player.speedInicial;
            player.rotationSpeed = player.rotationSpeedInicial;
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
        Debug.Log("ActiveDialogues");
    }
}

using UnityEngine;

public class TutorialDialogues : MonoBehaviour
{
    [TextArea] public string[] tutorialDialogues;
    private InputMaster inputMaster;
    int currentDialogue;
    [SerializeField] GameObject dialogueImage = default;
    [SerializeField] UserInterface ui = default;
    bool showDialogue = true;
    [SerializeField] Player3D player = default;
    [SerializeField] CameraControl CameraControl = default;

    bool nextDialogueBool = false;

    private void Awake() {
        inputMaster = new InputMaster();
    }
    void Start()
    {
        currentDialogue = 0;
        ui.pauseButton.interactable = false;
    }

    void Update()
    {
        //avanzamos los diálogos con el botón sur del control
        if (inputMaster.Menus.Select.triggered)
        {
            NextDialogueBool();
        }
        if (GameManager.gameIsPaused || player.youWon)
        {
            showDialogue = false;
        }

        else 
        {
            showDialogue = true;
        }
        StopDialogue();
        ActiveDialogues();
        MoveCamera();
    }

    void StopDialogue()
    {
        if (showDialogue == false)
        {
            return;
        }

        else 
        {
            //when the first dialogues ends
            if (currentDialogue == 5)
            {
                showDialogue = false;
            }
            //When the player collects all the pollen
            if (player.pollen == player.maxPollen)
            {
                showDialogue = true;
            }
            //when the second dialogue ends
            if (currentDialogue >= 7)
            {
                showDialogue = false;
                currentDialogue = 7;
            }
        }
    }

    void MoveCamera()
    {
        if (currentDialogue == 6)
        {
            CameraControl.currentView = CameraControl.views[1]; 
        }
    }
    //Active the dialogues and stop the game or resumes it
    public void ActiveDialogues()
    {
        if(showDialogue)
        {
            //player.move = false;   //que el jugador no se pueda mover, pusimos su velocidad en 0
            player.speed = 0;
            player.rotationSpeed = 0;
            dialogueImage.SetActive(true);
            ui.ShowText(tutorialDialogues[currentDialogue]);
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
            ui.pauseButton.interactable = true;
            dialogueImage.SetActive(false);
            this.enabled = false;
        }
    }

    public void NextDialogueBool() 
    {
        nextDialogueBool = true;
    }
}

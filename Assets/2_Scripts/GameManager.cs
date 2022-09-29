using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EventBus;

public class GameManager : MonoBehaviour
{
    //Singleton for this class
    public static GameManager gm;
    private TutorialDialogues tutorialDialogues;
    private ExtraDialogues extraDialogues;
    private InputMaster inputMaster;
    [SerializeField] AudioClip selectSound;
    public static bool gameIsPaused = false;
    bool canPause = false; // this bool is to avoid to pause the game during dialogues
    
    public string currentScene;
    public bool needFade = false;
    private PlayerInput playerInput;

    private void Awake() {
        //Singleton for this class
        if (gm != null && gm != this)
        {
            Destroy(this);
        }
        else
        {
            gm = this;
        }

        tutorialDialogues = new TutorialDialogues();
        extraDialogues = new ExtraDialogues();
        inputMaster = new InputMaster();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start() 
    {
        Application.targetFrameRate = 100;
        QualitySettings.vSyncCount = 1;
        currentScene = "Hello";
        
    }
    private void OnEnable()
    {
        inputMaster.Enable();
        GameEventBus.Subscribe(GameEventType.NORMALGAME, PlayerMap);
        GameEventBus.Subscribe(GameEventType.MENU, MenusMap);
        GameEventBus.Subscribe(GameEventType.FINISHDIALOGUE, FinishDialogue);
    }
    private void OnDisable()
    {
        inputMaster.Disable();
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, PlayerMap);
        GameEventBus.Unsubscribe(GameEventType.MENU, MenusMap);
        GameEventBus.Unsubscribe(GameEventType.FINISHDIALOGUE, FinishDialogue);
    }

    void FinishDialogue()
    {
        canPause = true;
    }

    //Switch action maps.
    private void PlayerMap()
    {
        playerInput.SwitchCurrentActionMap("Player");
        /* playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("Menus").Disable(); */
        Debug.Log("In Player action map");
    }
    private void MenusMap()
    {
        playerInput.SwitchCurrentActionMap("Menus");
        /* playerInput.actions.FindActionMap("Menus").Enable();
        playerInput.actions.FindActionMap("Player").Disable(); */
        Debug.Log("In Menus action map");
    }

    
    void Update()
    {
        //Pause.
        if (inputMaster.Player.Pause.triggered && canPause)
        {
            Debug.Log("Pause button pressed");

            AudioManager.audioManager.PlaySfxOnce(selectSound);
            gameIsPaused = !gameIsPaused;

            //Pause or resume game.
            if (gameIsPaused)
            {
                //Change action map to menu
                GameEventBus.Publish(GameEventType.MENU);
                GameEventBus.Publish(GameEventType.PAUSE);
                Time.timeScale = 0;
            }
            else
            {
                //Change action map to Player
                GameEventBus.Publish(GameEventType.NORMALGAME);
                Time.timeScale = 1;
            }
        }

        
    }


    //This separate methods for the pause are accessed by click or tap in the UI button.
    public void ResumeGame()
    {
        ChangePause();
    }

    public void ChangePause()
    {
        AudioManager.audioManager.PlaySfxOnce(selectSound);
        gameIsPaused = !gameIsPaused;

        //Pause or resume game.
        if (gameIsPaused)
        {
            //Change action map to menu
            GameEventBus.Publish(GameEventType.MENU);
            GameEventBus.Publish(GameEventType.PAUSE);
            Time.timeScale = 0;
        }
        else
        {
            //Change action map to Player
            GameEventBus.Publish(GameEventType.NORMALGAME);
            Time.timeScale = 1;
        }
    }

    public void LoadScene(string SceneToLoad)
    {
        StartCoroutine(DelayedLoad(SceneToLoad));
    }

    public void LoadSceneSimple(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void Retry()
    {
        //Without this, if we retry from a win or lost menu the game will pause the next time the scene loads
        if(gameIsPaused)
            ChangePause();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene01(string SceneToLoad)
    {
        AudioManager.audioManager.PlaySfxOnce(selectSound);
        needFade = true;
        SceneManager.LoadScene(SceneToLoad);
        gameIsPaused = false;
        currentScene = SceneToLoad;
    }
    public void ExitGame()
    {
        Debug.Log("Exited game");
        Application.Quit();
    }

    IEnumerator DelayedLoad(string SceneToLoad) 
    {
        AudioManager.audioManager.PlaySfxOnce(selectSound);
        yield return new WaitForSeconds(selectSound.length);
        needFade = true;
        SceneManager.LoadScene(SceneToLoad);
        gameIsPaused = false;
        currentScene = SceneToLoad;
    }

}

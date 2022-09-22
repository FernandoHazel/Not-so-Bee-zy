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
        Time.timeScale = 1;
    }
    private void OnEnable()
    {
        //inputMaster.Enable();
        GameEventBus.Subscribe(GameEventType.NORMALGAME, PlayerMap);
        GameEventBus.Subscribe(GameEventType.MENU, MenusMap);
    }
    private void OnDisable()
    {
        //inputMaster.Disable();
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, PlayerMap);
        GameEventBus.Unsubscribe(GameEventType.MENU, MenusMap);
    }

    //Switch action maps.
    private void PlayerMap()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Debug.Log("In Player action map");
    }
    private void MenusMap()
    {
        playerInput.SwitchCurrentActionMap("Menus");
        Debug.Log("In Menus action map");
    }

    
    void Update()
    {
        //Pause.
        if (inputMaster.Player.Pause.triggered)
        {
            Debug.Log("Pause button pressed");

            AudioManager.audioManager.PlaySfxOnce(selectSound);
            gameIsPaused = !gameIsPaused;

            //Pause or resume game.
            if (gameIsPaused)
            {
                //Change action map to menu
                GameEventBus.Publish(GameEventType.MENU);
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

    public void LoadScene(string SceneToLoad)
    {
        StartCoroutine(DelayedLoad(SceneToLoad));
    }

    public void LoadSceneSimple(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
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

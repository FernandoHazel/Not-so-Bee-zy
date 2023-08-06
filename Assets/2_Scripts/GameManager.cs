using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EventBus;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private InputMaster inputMaster;
    public static bool gameIsPaused = false;
    bool canPause = false; // this bool is to avoid to pause the game during dialogues
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip pauseMusic;
    [SerializeField] private InterstitialAds interstitialAds;
    

    
    public string currentScene;
    public bool needFade = false;
    private PlayerInput playerInput;

    private void Awake() {
        inputMaster = new InputMaster();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start() 
    {
        //Application.targetFrameRate = 100;
        //QualitySettings.vSyncCount = 1;
        currentScene = "";
        AudioManager.audioManager.PlaySfxLoop(gameMusic);
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
    }
    private void MenusMap()
    {
        playerInput.SwitchCurrentActionMap("Menus");
    }

    
    void Update()
    {
        //Pause.
        if (inputMaster.Player.Pause.triggered && canPause)
        {
            ChangePause();
        }
    }


    //This separate methods for the pause are accessed by click or tap in the UI button.
    public void ResumeGame()
    {
        AudioManager.audioManager.ClearLoopChannels();
        AudioManager.audioManager.PlaySfxOnce(selectSound);
        AudioManager.audioManager.PlaySfxLoop(gameMusic);
        ChangePause();
    }

    public void ChangePause()
    {
        gameIsPaused = !gameIsPaused;

        //Pause or resume game.
        if (gameIsPaused)
        {
            AudioManager.audioManager.ClearLoopChannels();
            AudioManager.audioManager.PlaySfxLoop(pauseMusic);
            //Change action map to menu
            GameEventBus.Publish(GameEventType.MENU);
            GameEventBus.Publish(GameEventType.PAUSE);
            Time.timeScale = 0;
        }
        else
        {
            AudioManager.audioManager.ClearLoopChannels();
            AudioManager.audioManager.PlaySfxOnce(selectSound);
            AudioManager.audioManager.PlaySfxLoop(gameMusic);
            //Change action map to Player
            GameEventBus.Publish(GameEventType.NORMALGAME);
            Time.timeScale = 1;
        }
    }

    public void LoadScene(string SceneToLoad)
    {
        if(gameIsPaused)
            ChangePause();

            //show an ad
        interstitialAds.ShowAd();

        StartCoroutine(DelayedLoad(SceneToLoad));
    }

    public void Retry()
    {
        //Without this, if we retry from a win or lost menu the game will pause the next time the scene loads
        if(gameIsPaused)
            ChangePause();

            //show an ad
        interstitialAds.ShowAd();
            
        StartCoroutine(DelayedLoad(SceneManager.GetActiveScene().name));
    }

    public void ExitGame()
    {
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

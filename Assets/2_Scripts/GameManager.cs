using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip selectSound;
    public static bool gameIsPaused = false;
    
    public string currentScene;

    public bool needFade = false;

    void Start() 
    {
        Application.targetFrameRate = 100;
        QualitySettings.vSyncCount = 1;
        currentScene = "Hello";
        Time.timeScale = 1;
    }

    
    void Update()
    {
        if (gameIsPaused)
        {
            PauseGame();
        }

        else
        {
            Resume();
        }

    }
    
    public void Resume() 
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ChangePause() 
    {
        AudioManager.Instance.PlaySfxOnce(selectSound);
        gameIsPaused = !gameIsPaused;
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
        AudioManager.Instance.PlaySfxOnce(selectSound);
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
        AudioManager.Instance.PlaySfxOnce(selectSound);
        yield return new WaitForSeconds(selectSound.length);
        needFade = true;
        SceneManager.LoadScene(SceneToLoad);
        gameIsPaused = false;
        currentScene = SceneToLoad;
    }

}

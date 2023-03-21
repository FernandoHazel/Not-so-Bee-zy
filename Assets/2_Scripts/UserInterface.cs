using UnityEngine;
using UnityEngine.UI;
using EventBus;
using TMPro;
public class UserInterface : MonoBehaviour
{
    //public PauseAnimationsManager pauseTrigger;
    public GameObject dialogeImage;
    public TextMeshProUGUI lazyText;
    public TextMeshProUGUI pollen;
    public GameObject youWon;
    public GameObject youLost;
    public Image [] honeyCombs;
    public Color honeyCombColor;

    public GameObject pauseText;

    public TMPro.TextMeshProUGUI timerText;

    public float timer;

    bool cuenta = false;

    [SerializeField] public Button pauseButton;
    [SerializeField] private GameObject joystick;

    private void Start()
    {
        youWon.SetActive(false);
        youLost.SetActive(false);
    }

    private void OnEnable() 
    {
        GameEventBus.Subscribe(GameEventType.PAUSE, StopTimer);
        GameEventBus.Subscribe(GameEventType.LOST, YouLost);
        GameEventBus.Subscribe(GameEventType.WIN, YouWon);
        GameEventBus.Subscribe(GameEventType.FINISHDIALOGUE, ShutDownText);
        GameEventBus.Subscribe(GameEventType.NORMALGAME, StartTimer);
        GameEventBus.Subscribe(GameEventType.DIALOGUE, DisableJoystick);
        RewardedAdsButton.rewarded += SecondChance;
    }
    private void OnDisable() 
    {
        GameEventBus.Unsubscribe(GameEventType.PAUSE, StopTimer);
        GameEventBus.Unsubscribe(GameEventType.LOST, YouLost);
        GameEventBus.Unsubscribe(GameEventType.WIN, YouWon);
        GameEventBus.Unsubscribe(GameEventType.FINISHDIALOGUE, ShutDownText);
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, StartTimer);
        GameEventBus.Unsubscribe(GameEventType.DIALOGUE, DisableJoystick);
        RewardedAdsButton.rewarded -= SecondChance;
    }

    private void SecondChance()
    {
        //Close the "you lost" panel and let continue playing
        GameEventBus.Publish(GameEventType.FINISHDIALOGUE);
        GameEventBus.Publish(GameEventType.NORMALGAME);
    }
    public void UpdatePollen(int num, int maxNum)
    {
        pollen.text = "Pollen: " + num + "/" + maxNum;
    }

    private void StartTimer()
    {
        cuenta = true;
    }
    private void StopTimer()
    {
        cuenta = false;
    }

    public void UpdateHoneyComb(int num)
    {
        honeyCombs[num].color = new Color32(255, 255, 255, 255);
    }

    public void ResetHoneyComb()
    {
        for (int i = 0; i < honeyCombs.Length; i++)
        {
            honeyCombs[i].color = honeyCombColor;
        }
    }
    public void YouWon() 
    {
        cuenta = false;
        youWon.SetActive(true);
    
    }

    public void YouLost() 
    {
        youLost.SetActive(true);
    }

    private void DisplayTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("Time:  {0:00}:{1:00}",minutes,seconds);

    }

    private void Update()
    {
        //If pause is active we display the pause text
        if (GameManager.gameIsPaused)
        {
            pauseText.SetActive(true);
        }
        else
        {
            pauseText.SetActive(false);
        }

        if (cuenta == true)
        {
            timer += Time.deltaTime;
            DisplayTimer(timer);
        }
    }
    public void ShowText(string Text)  //Muestra el recuadro y el texto que le mandemos
    {
        dialogeImage.SetActive(true);
        lazyText.text = Text;
        pauseButton.interactable = false;
    }
    void DisableJoystick()
    {
        joystick.SetActive(false);
    }
    public void ShutDownText()
    {
        youLost.SetActive(false);
        dialogeImage.SetActive(false);
        pauseButton.interactable = true;
        joystick.SetActive(true);
    }
}

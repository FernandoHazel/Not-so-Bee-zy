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

    public GameObject extraImage;
    public TextMeshProUGUI extraLazyText;
    public GameObject extraImage01;
    public TextMeshProUGUI extraLazyText01;

    public GameObject youWon;
    public GameObject youLost;
    public Image [] honeyCombs;
    public Color honeyCombColor;

    public GameObject pauseText;

    public TMPro.TextMeshProUGUI timerText;

    public float timer;

    bool cuenta = false;

    [SerializeField] public Button pauseButton;

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
        GameEventBus.Subscribe(GameEventType.FINISHDIALOGUE, StartTimer);
        GameEventBus.Subscribe(GameEventType.NORMALGAME, StartTimer);
    }
    private void OnDisable() 
    {
        GameEventBus.Unsubscribe(GameEventType.PAUSE, StopTimer);
        GameEventBus.Unsubscribe(GameEventType.LOST, YouLost);
        GameEventBus.Unsubscribe(GameEventType.WIN, YouWon);
        GameEventBus.Unsubscribe(GameEventType.FINISHDIALOGUE, StartTimer);
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, StartTimer);
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
    }
    public void ShowExtraText(string Text)  //Muestra el recuadro y el texto que le mandemos
    {
        extraImage.SetActive(true);
        extraLazyText.text = Text;
    }
    public void ShowExtraText01(string Text)  //Muestra el recuadro y el texto que le mandemos
    {
        extraImage01.SetActive(true);
        extraLazyText01.text = Text;
    }
    public void ShutDownText()
    {
        dialogeImage.SetActive(false);
    }
}

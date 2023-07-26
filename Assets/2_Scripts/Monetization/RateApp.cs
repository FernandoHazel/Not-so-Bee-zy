using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class RateApp : MonoBehaviour
{
    [SerializeField] GameObject rateTheApp;

    public string reviewURL = "https://play.google.com/store/apps/details?id=com.Fhazel.Notsobeezy&reviewId=0";

    //Subscribe when the player wins a level
    private void OnEnable() 
    {
        GameEventBus.Subscribe(GameEventType.WIN, CompleteLevel);
    }
    private void OnDisable() 
    {
        GameEventBus.Unsubscribe(GameEventType.WIN, CompleteLevel);
    }
    private void Start() {
        rateTheApp.SetActive(false);
    }

    private void CompleteLevel()
    {
        //When the player wins we add 1
        LevelData.levelsCompleted += 1;
        Debug.Log("level complete added");

        //If the player have won 3 levels a message to rate the app will pop up
        if (LevelData.levelsCompleted % 3 == 0)
        {
            //Pop up to rate the app
            rateTheApp.SetActive(true);
        }
    }

    public void rateApp() 
    {
        //Redirect to the rate app link
        Application.OpenURL(reviewURL);
    }

    public void dontRateApp() 
    {
        rateTheApp.SetActive(false);
    }
}

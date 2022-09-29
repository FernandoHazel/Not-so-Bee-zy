using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EventBus;

public class GamepadMenuManager : MonoBehaviour
{
    
    [Header("Pause menu")]
    [SerializeField]
    GameObject resumeButton;

    [Header("You won menu")]
    [SerializeField]
    GameObject nextLevelButton;
    
    [Header("You lost menu")]
    [SerializeField]
    GameObject retryButton;
    
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.NORMALGAME, ClearSelection);
        GameEventBus.Subscribe(GameEventType.PAUSE, PauseMenu);
        GameEventBus.Subscribe(GameEventType.WIN, YouWonMenu);
        GameEventBus.Subscribe(GameEventType.LOST, YouLostMenu);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, ClearSelection);
        GameEventBus.Unsubscribe(GameEventType.PAUSE, PauseMenu);
        GameEventBus.Unsubscribe(GameEventType.WIN, YouWonMenu);
        GameEventBus.Unsubscribe(GameEventType.LOST, YouLostMenu);
    }
    
    void PauseMenu()
    {
        //Set a selected button
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }
    void YouWonMenu()
    {
        //Set a selected button
        EventSystem.current.SetSelectedGameObject(nextLevelButton);
    }
    void YouLostMenu()
    {
        //Set a selected button
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    void ClearSelection()
    {
        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);
    }
}

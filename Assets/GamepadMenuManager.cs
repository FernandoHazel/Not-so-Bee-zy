using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EventBus;

public class GamepadMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject resumeButton;
    
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.NORMALGAME, ClearSelection);
        GameEventBus.Subscribe(GameEventType.PAUSE, SelectButton);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, ClearSelection);
        GameEventBus.Unsubscribe(GameEventType.PAUSE, SelectButton);
    }
    
    void SelectButton()
    {
        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);
        //Set a selected button
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    void ClearSelection()
    {
        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);
    }
}

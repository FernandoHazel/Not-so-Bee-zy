using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EventBus;

public class GamepadButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject Button;

    private void OnEnable() {
        GameEventBus.Subscribe(GameEventType.UNITERACTABLE, SelectButton);
    }

    private void OnDisable() {
        GameEventBus.Unsubscribe(GameEventType.UNITERACTABLE, SelectButton);
    }
    private void Start() {
        SelectButton();
    }
    
    void SelectButton()
    {
        //Set a selected button
        EventSystem.current.SetSelectedGameObject(Button);
    }
}


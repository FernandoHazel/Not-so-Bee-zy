using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EventBus;

public class GamepadButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject Button, secondaryButton;

    private void OnEnable() {
        GameEventBus.Subscribe(GameEventType.UNITERACTABLE, SelectButton);
        SelectButton();
    }

    private void OnDisable() {
        GameEventBus.Unsubscribe(GameEventType.UNITERACTABLE, SelectButton);

        if (secondaryButton != null)
        {
            GameEventBus.Publish(GameEventType.UNITERACTABLE);
        }
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


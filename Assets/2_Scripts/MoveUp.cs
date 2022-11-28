using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class MoveUp : MonoBehaviour
{
    private float speed;

    private void OnEnable() {
        GameEventBus.Subscribe(GameEventType.FINISHDIALOGUE, ChangeSpeed);
    }
    private void OnDisable() {
        GameEventBus.Unsubscribe(GameEventType.FINISHDIALOGUE, ChangeSpeed);
    }

    private void Start() {
        speed = 0;
    }

    private void Update() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void ChangeSpeed()
    {
        speed = 250;
    }
}

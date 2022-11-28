using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventBus;

public class MusicSingleton : MonoBehaviour
{
    public static MusicSingleton Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.NORMALGAME, DestroyMenuMusic);
        GameEventBus.Subscribe(GameEventType.DIALOGUE, DestroyMenuMusic);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, DestroyMenuMusic);
        GameEventBus.Unsubscribe(GameEventType.DIALOGUE, DestroyMenuMusic);
    }

    void DestroyMenuMusic()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlock : MonoBehaviour
{
    [SerializeField] int actualLevel;
    static public int unlockedLevel;

    private void Start() 
    {
        Debug.Log(unlockedLevel);
        actualLevel++;
    }

    public void UnlockLevel()
    {
        if (unlockedLevel < actualLevel)
        {
            unlockedLevel = actualLevel;
            actualLevel++;
        }
    }
}

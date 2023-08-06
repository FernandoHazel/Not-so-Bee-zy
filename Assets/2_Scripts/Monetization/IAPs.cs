using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPs : MonoBehaviour
{
    public static bool ads = true;

    //This happens when the user buy the no ads package
    public void RemoveAds()
    {
        ads = false;
    }
}

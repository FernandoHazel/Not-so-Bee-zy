using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : MonoBehaviour
{
    private void Start() {
        if(IAPs.ads == false)
        {
            gameObject.SetActive(false);
        }
    }
}

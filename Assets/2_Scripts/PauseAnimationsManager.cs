using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimationsManager : MonoBehaviour
{
   Animator pauseAnimator = default; //we take the animator of the pauseScreen
   public void PauseAnimator()
   {
       pauseAnimator.SetTrigger("Play"); //we set a trigger to swhich between pause animations
   }
}

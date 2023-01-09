using UnityEngine;
public class FadeInFadeOut : MonoBehaviour
{

    public Animator animator;

    public GameManager gm;

    private void Start()
    {
        if (gm.needFade == true) 
        {
            animator.SetTrigger("FadeOut");
        }
    }
}

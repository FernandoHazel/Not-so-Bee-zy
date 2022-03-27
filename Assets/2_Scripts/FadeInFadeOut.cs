using UnityEngine;
public class FadeInFadeOut : MonoBehaviour
{

    public Animator animator;

    public GameManager gm;

    private void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    private void Update()
    {
        if (gm.needFade == true) 
        {
            animator.SetTrigger("FadeOut");
        }
    }
}

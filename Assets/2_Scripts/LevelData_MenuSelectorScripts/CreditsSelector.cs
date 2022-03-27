using UnityEngine;
using UnityEngine.UI;

public class CreditsSelector : MonoBehaviour
{
    [SerializeField] Button rightButton;
    [SerializeField] Button leftButton;

    [SerializeField] AudioClip nextSound, previousSound;

    public GameObject[] creditUi;

    int currentCredit;
    int maxCredit;

    void Start()
    {
        maxCredit = creditUi.Length - 1;
        currentCredit = 0;
        creditUi[currentCredit].gameObject.SetActive(true);

    }

    private void Update()
    {
        if (currentCredit == 0)
        {
            leftButton.interactable = false;
        }

        if (currentCredit >= maxCredit)
        {
            rightButton.interactable = false;
        }
    }
    public void NextSkin()
    {
        AudioManager.Instance.PlaySfxOnce(nextSound);
        leftButton.interactable = true;
        if (currentCredit>= maxCredit)
        {
            rightButton.interactable = false;
            return;
        }
        else
        {
            creditUi[currentCredit].gameObject.SetActive(false);
            currentCredit++;
            creditUi[currentCredit].gameObject.SetActive(true);
            rightButton.interactable = true;
        }
    }
    public void PreviousSkin()
    {
        AudioManager.Instance.PlaySfxOnce(previousSound);
        rightButton.interactable = true;
        if ((currentCredit) <= 0 )
        {
            leftButton.interactable = false;
            return;
        }
        else
        {
            creditUi[currentCredit].gameObject.SetActive(false);
            currentCredit--;
            creditUi[currentCredit].gameObject.SetActive(true);
            leftButton.interactable = true;
        }
    }
}

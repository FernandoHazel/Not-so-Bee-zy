using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] AudioClip buySound, selectSound, nextSound, previousSound;
    int currentSkin, maxSkins;

    [SerializeField] Button buyButton, selectButton, selectedButton, rightButton, leftButton;

    [SerializeField] GameObject skinViewer;
    [SerializeField] GameObject [] skinViewerSkins;
    GameObject currentSkinViewer;

    public GameObject[] skinsUi;
    public int[] skinsCost;

    [SerializeField] LevelData levelData; //el scriptableobject que guarda la info
    [SerializeField] TMPro.TextMeshProUGUI honeycombsGlobal;

    void Start()
    {
        maxSkins = skinsUi.Length - 1;
        levelData.LoadData();
        levelData.SetSkinUnlocked(0);
        levelData.SaveData();

        honeycombsGlobal.text = ": " + levelData.GetHoneycombsGlobal();
        currentSkin = levelData.GetSkinSelected();

        skinsUi[currentSkin].gameObject.SetActive(true);

        currentSkinViewer = Instantiate(skinViewerSkins[currentSkin], skinViewer.transform.position, skinViewer.transform.rotation);
        currentSkinViewer.transform.parent = skinViewer.gameObject.transform;

        for (int i = 0; i < skinsUi.Length; i++) //para cargar sus precios
        {
            TMPro.TextMeshProUGUI costText = skinsUi[i].gameObject.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            costText.text = ": " + skinsCost[i];
        }

    }
    private void Update()
    {
        CheckStatus();
        CheckIfMarcel();

        if (currentSkin == 0)
        {
            leftButton.interactable = false;
        }

        if (currentSkin >= maxSkins)
        {
            rightButton.interactable = false;
        }
    }

    public void NextSkin()
    {
        AudioManager.Instance.PlaySfxOnce(nextSound);
        leftButton.interactable = true;
        if (currentSkin >= maxSkins)
        {
            rightButton.interactable = false;
            return;
        }
        else
        {
            Destroy(skinViewer.transform.GetChild(0).gameObject);
            skinsUi[currentSkin].gameObject.SetActive(false);
            currentSkin++;
            skinsUi[currentSkin].gameObject.SetActive(true);
            currentSkinViewer = Instantiate(skinViewerSkins[currentSkin], skinViewer.transform.position, skinViewer.transform.rotation);
            currentSkinViewer.transform.parent = skinViewer.gameObject.transform;
            rightButton.interactable = true;
        }
    }

    public void PreviousSkin()
    {
        AudioManager.Instance.PlaySfxOnce(previousSound);
        rightButton.interactable = true;
        if (currentSkin <= 0)
        {
            leftButton.interactable = false;
            return;
        }
        else
        {
            Destroy(skinViewer.transform.GetChild(0).gameObject);
            skinsUi[currentSkin].gameObject.SetActive(false);
            currentSkin--;
            skinsUi[currentSkin].gameObject.SetActive(true);
            currentSkinViewer = Instantiate(skinViewerSkins[currentSkin], skinViewer.transform.position, skinViewer.transform.rotation);
            currentSkinViewer.transform.parent = skinViewer.gameObject.transform;
            leftButton.interactable = true;
        }
    }

    public void BuySkin() 
    {
        AudioManager.Instance.PlaySfxOnce(buySound);
        levelData.SpendHoneycombsGlobal(skinsCost[currentSkin]);
        levelData.SetSkinUnlocked(currentSkin);
        levelData.SetSkinSelected(currentSkin);
        levelData.SaveData();
        honeycombsGlobal.text = ": " + levelData.GetHoneycombsGlobal();
    }

    public void SelectSkin() 
    {
        AudioManager.Instance.PlaySfxOnce(selectSound);
        levelData.SetSkinSelected(currentSkin);
        levelData.SaveData();
    }

    private void CheckStatus() 
    {
        if (levelData.GetSkinUnlocked(currentSkin))
        {
            if (currentSkin == levelData.GetSkinSelected())
            {
                skinsUi[currentSkin].transform.GetChild(3).gameObject.SetActive(false);
                skinsUi[currentSkin].transform.GetChild(0).gameObject.SetActive(false);
                selectedButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
            }
            else
            {
                skinsUi[currentSkin].transform.GetChild(3).gameObject.SetActive(false);
                skinsUi[currentSkin].transform.GetChild(0).gameObject.SetActive(false);
                selectedButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
            }
        }

        else
        {
            selectedButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);

            if (skinsCost[currentSkin] > levelData.honeycombsGlobal)
            {
                buyButton.interactable = false;
            }
            else
            {
                buyButton.interactable = true;
            }

        }
    }

    private void CheckIfMarcel() 
    {

        if (currentSkin == 2)
        {
            skinViewer.transform.position = new Vector3(0.54f,1.19f , -7.9f);
        }

        else 
        {
            skinViewer.transform.position = new Vector3(0.54f,1f,-7.9f);
        }

    }
}
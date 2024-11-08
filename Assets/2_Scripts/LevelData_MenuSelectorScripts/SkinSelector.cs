using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventBus;

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

    [SerializeField] LevelData levelData;
    [SerializeField] TMPro.TextMeshProUGUI honeycombsGlobal;

    private Vector3 defaultPosition;

    void Start()
    {
        defaultPosition = skinViewer.transform.position;

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
    }

    public void NextSkin()
    {
        AudioManager.audioManager.PlaySfxOnce(nextSound);
        leftButton.interactable = true;
        if (currentSkin >= maxSkins)
        {
            //Change to the default button
            GameEventBus.Publish(GameEventType.UNITERACTABLE);

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
        AudioManager.audioManager.PlaySfxOnce(previousSound);
        rightButton.interactable = true;
        if (currentSkin <= 0)
        {
            //Change to the default button
            GameEventBus.Publish(GameEventType.UNITERACTABLE);
            
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
        GameEventBus.Publish(GameEventType.UNITERACTABLE);
        AudioManager.audioManager.PlaySfxOnce(buySound);
        levelData.SpendHoneycombsGlobal(skinsCost[currentSkin]);
        levelData.SetSkinUnlocked(currentSkin);
        levelData.SetSkinSelected(currentSkin);
        levelData.SaveData();
        honeycombsGlobal.text = ": " + levelData.GetHoneycombsGlobal();
    }

    public void SelectSkin() 
    {
        GameEventBus.Publish(GameEventType.UNITERACTABLE);
        AudioManager.audioManager.PlaySfxOnce(selectSound);
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
            skinViewer.transform.position = defaultPosition + new Vector3(0, 0.2f ,0);
        }

        else 
        {
            skinViewer.transform.position = defaultPosition;
        }

    }
}
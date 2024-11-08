using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PollensCanvas : MonoBehaviour
{
    private TextMeshProUGUI text;
    [Tooltip("Set the desired fot size of the number")]
    Camera cam;

    private void Awake() 
    {
        //Cahe the camera
        cam = Camera.main;
        text = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable() 
    {
        //start the coroutine to disable the canvas
        StartCoroutine(DestroyPointsText());
    }

    public void DisplayPollen()
    {
        //set the pollen quantity
        text.text = Player3D.pollen.ToString();

        //Perform a little animation
        transform.localScale = new Vector3(.2f,.2f,0);
        transform.DOScale(1, .2f);
    }


    IEnumerator DestroyPointsText()
    {
        //Luego de un segundo desactivamos el canvas
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Limpia los objetos generados por DOTween
        DOTween.Clear(true);
    }

}
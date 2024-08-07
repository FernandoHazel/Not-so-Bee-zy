﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventBus;


public class Player3D : MonoBehaviour
{
    public static bool inDialogue;
    WaitForSeconds waitGasWFS = new WaitForSeconds(.5f);
    WaitForSeconds waitHoneyWFS = new WaitForSeconds(.1f);
    //This timer is to let the pater collect an item before ger affected by gas
    WaitForSeconds beforeGasWFS = new WaitForSeconds(.3f);

    [HideInInspector] public int maxPollen = 10;
    [Header("Pollen and HoneyComb")]
    [SerializeField] ParticleSystem grabPollen, grabHC;
    public static int pollen = 0;
    [HideInInspector] public int honeyComb = 0;
    List<GameObject> pollenList = new List<GameObject>(), honeyCombList = new List<GameObject>();
    public GameObject honeyParticles;

    [Header("Sound")]
    [SerializeField] AudioSource trapedInHoney;
    [SerializeField] AudioClip pollenSound, honeyCombClip, greyHoneyCombClip, levelCompletedSound, dieSound, inGas,buzzing;
    

    [HideInInspector] public CharacterController controller;
    [SerializeField] private GameObject playerPrefab;
    private InputMaster inputMaster;
    [Header("Movement variables")]
    public float speed = 3, gravity = 1f, rotationSpeed = 5;
    public float speedInicial, rotationSpeedInicial;
    public Transform cam;
    Vector3 movement, posInicial;
    private bool move = false;

    public UserInterface UserInterfaceScript;
    [HideInInspector] public bool youLost = false, youWon = false;
    bool inverted = false;

    public CameraControl cameraControl;
    public Door doorScript;

    [SerializeField] float invertedControlsTime = 3;
    [SerializeField] float honeyTime = 3;

    bool slowHoney = false;
    public GameObject gasParticles;
    [SerializeField] Image gasClock = default;
    [SerializeField] Image gasGoldBg;
    [SerializeField] GameObject gasClockCanvas;

    [SerializeField] LevelData levelData;
    [SerializeField] GameObject [] skins;
    GameObject localSkin;
    
    private int controlDirection;
    private int camCheckInterval = 15;

    private void Awake() 
    {
        controller = GetComponent<CharacterController>();
        inputMaster = new InputMaster();
    }
    void Start()
    {
        pollen = 0;
        if (levelData.GetSkinSelected() != 2) 
        {
            AudioManager.audioManager.PlaySfxLoop3D(buzzing, transform);
        }
        controlDirection = 1;
        move = true;
        speedInicial = speed;
        rotationSpeedInicial = rotationSpeed;
        posInicial = transform.position;

        UserInterfaceScript.UpdatePollen(pollen,maxPollen);
        UserInterfaceScript.ResetHoneyComb();

        honeyParticles.GetComponent<ParticleSystem>().Stop();
        gasParticles.GetComponent<ParticleSystem>().Stop();
        gasClock.fillAmount = 0;
        gasGoldBg.enabled = false;

        localSkin = Instantiate(skins[levelData.GetSkinSelected()], transform.position, transform.rotation);
        localSkin.transform.parent = gameObject.transform;

    }
    private void OnEnable() {
        inputMaster.Enable();
        GameEventBus.Subscribe(GameEventType.DIALOGUE, FreezePlayer);
        GameEventBus.Subscribe(GameEventType.LOST, FreezePlayer);
        GameEventBus.Subscribe(GameEventType.WIN, FreezePlayer);
        GameEventBus.Subscribe(GameEventType.NORMALGAME, ReturnToGame);
        RewardedAdsButton.rewarded += SecondChance;
    }
    private void OnDisable() {
        inputMaster.Disable();
        GameEventBus.Unsubscribe(GameEventType.DIALOGUE, FreezePlayer);
        GameEventBus.Unsubscribe(GameEventType.LOST, FreezePlayer);
        GameEventBus.Unsubscribe(GameEventType.WIN, FreezePlayer);
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, ReturnToGame);
        RewardedAdsButton.rewarded -= SecondChance;
    }

    private void SecondChance () 
    {
        youLost = false;
    }

    void Movement()
    {
        if (move == true)
        {
            //Get the input and invert the direction if the player is poisoned.
            movement = new Vector3(inputMaster.Player.move.ReadValue<Vector2>().x * controlDirection, 0f, inputMaster.Player.move.ReadValue<Vector2>().y * controlDirection).normalized;

            //Set the camera position and rotation.
            Vector3 camF = cam.forward;
            Vector3 camR = cam.right;

            camF.y = 0;
            camR.y = 0;

            camF = camF.normalized;
            camR = camR.normalized;

            movement.y = movement.y - (gravity * Time.deltaTime);

            //Move the player.
            controller.Move((camF * movement.z + camR * movement.x) * speed * Time.deltaTime);
            controller.Move(new Vector3(0, movement.y, 0) * speed * Time.deltaTime);

            movement.y = 0; // reset y, so the player doesnt look to the floor

            if ((movement != Vector3.zero))
            {
                PlayerRotation(camF, camR);
            }
        }
    }

    void FreezePlayer()
    {
        inDialogue = true;
        move = false;
    }

    void ReturnToGame()
    {
        inDialogue = false;
        move = true;
    }

    void InvertedEffect()
    {
        gasClock.fillAmount -= 1 / invertedControlsTime * Time.deltaTime *.4f;
        invertedControlsTime -= Time.deltaTime;  //We start a count down for the inverted controls
        //When time is over we go back to normal controls and disable gas effect
        if (invertedControlsTime <= 0)
        {
            controlDirection = 1;
            gasGoldBg.enabled = false;
            inverted = false;
            invertedControlsTime = 3;
            gasParticles.GetComponent<ParticleSystem>().Stop();
            gasClockCanvas.transform.SetParent(playerPrefab.transform);
            gasParticles.transform.SetParent(playerPrefab.transform);

            return;
        }
    }

    void PlayerRotation(Vector3 camF, Vector3 camR)
    {
        
        Vector3 relativePos = (camF * movement.z + camR * movement.x);
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
    }

    void Die() 
    {
        AudioManager.audioManager.PlaySfxOnce(dieSound);
        GameEventBus.Publish(GameEventType.LOST);
        GameEventBus.Publish(GameEventType.PAUSE);
    }

    void GrabPollen(GameObject go) 
    {
        //Aumentamos la cantidad global de polen
        pollen++;

        grabPollen.Play();
        go.SetActive(false);
        pollenList.Add(go);
        if (pollen == maxPollen)
        {
            doorScript.OpenDoor();
            AudioManager.audioManager.PlaySfxOnce(levelCompletedSound);
        }
        AudioManager.audioManager.PlaySfxOnce(pollenSound);
    }

    void GrabHoneyComb(GameObject go)
    {
        grabHC.Play();
        honeyComb++;
        go.SetActive(false);
        honeyCombList.Add(go);
        AudioManager.audioManager.PlaySfxOnce(honeyCombClip);
    }

    void GrabHoneyCombTransparent(GameObject go) 
    {
        go.SetActive(false);
        honeyCombList.Add(go);
        AudioManager.audioManager.PlaySfxOnce(greyHoneyCombClip);
    }

    void HoneyTrail()
    {
        //AudioManager.Instance.PlaySfxLoop3D(trapedInHoney, transform);
        honeyTime -= Time.deltaTime;
        if (honeyTime <= 0)
        {
            trapedInHoney.Stop();
            speed = speedInicial;
            slowHoney = false;
            honeyParticles.GetComponent<ParticleSystem>().Stop();
            honeyParticles.transform.SetParent(playerPrefab.transform);
            return;
        }
    }

    //TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        
        
        //Here we invert the controls, set a timer in 5 and active the main character gas effect
        if (other.gameObject.CompareTag("Gas") && inverted == false)
        {
            //Desactivamos la trampa de humo
            other.gameObject.GetComponent<ParticleSystem>().Stop();
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(TimerBeforGasEffect());
        }

        if (other.gameObject.CompareTag("Pollen"))
        {
            GrabPollen(other.gameObject);
            UserInterfaceScript.UpdatePollen(pollen, maxPollen);
        }

        if (other.gameObject.CompareTag("HoneyComb"))
        {
            if (other.gameObject.GetComponent<HoneyComb>().CheckHoneyhomb() == true)
            {
                GrabHoneyCombTransparent(other.gameObject);
            }

            else 
            {
                GrabHoneyComb(other.gameObject);
                UserInterfaceScript.UpdateHoneyComb(honeyComb - 1);
                other.gameObject.GetComponent<HoneyComb>().UpdateInLevelData();
            }

        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            AudioManager.audioManager.PlaySfxOnce(levelCompletedSound);
            GameEventBus.Publish(GameEventType.WIN);
            youWon = true;
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
            if (youWon == false)
            {
                //Informamos al botón de anuncio que el jugador no murión cayendo al precipicio
                RewardedAdsButton.playerFell = false;
                Die();
            }
        }

        if (other.gameObject.CompareTag("Precipice"))
        {
            if (youWon == false)
            {
                //Informamos al botón de anuncio que el jugador murión cayendo al precipicio
                RewardedAdsButton.playerFell = true;
                Die();
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Honey"))
        {
            if (slowHoney == false)
            {
                honeyParticles.transform.SetParent(transform);
                honeyParticles.transform.localPosition = new Vector3(0,-0.3f,0);
                StartCoroutine(WaitHoney());
            }
        }

        if (other.gameObject.CompareTag("Platformer"))
        {
            if (other.gameObject.GetComponent<Platform>().tiempoDeEsperaInterno >0)
            {
                move = true;
            }
            else 
            {
                move = false;
                transform.parent = other.gameObject.transform;
            }
        }

        //Como este es un proceso costoso lo hago en un intervalo de frames

        if(Time.frameCount % camCheckInterval == 0)
        {
            if (other.gameObject.CompareTag("Floor") && !inDialogue)
            {
                //Nos aseguramos que mientras el jugador esté en un collider la cámara se acomode
                //para evitar que la cámara no lo siga
                cameraControl.currentView = cameraControl.views[(int)other.gameObject.GetComponent<FloorCam>().floorNumber];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Platformer"))
        {
            move = true;
            transform.parent = null;
        }
    }

    //COROUTINES
    IEnumerator TimerBeforGasEffect()
    {
        yield return beforeGasWFS;
        //Emparentamos temporalmente el objeto del reloj y las partículas de humo
        gasClockCanvas.transform.SetParent(transform);
        gasParticles.transform.SetParent(transform);
        gasClockCanvas.transform.localPosition = new Vector3(0,.5f,0);
        gasParticles.transform.localPosition = new Vector3(0,0,0);

        //Invertimos los controles
        inverted = true;
        controlDirection = -1;

        
            StartCoroutine(WaitGas());
    }
    IEnumerator WaitGas()
    {
        AudioManager.audioManager.PlaySfxOnce(inGas);
        gasClock.fillAmount = 1;
        gasGoldBg.enabled = true;
        gasParticles.GetComponent<ParticleSystem>().Play();
        yield return waitGasWFS;
        controlDirection = -1;
        invertedControlsTime = 2.5f;
    }

    IEnumerator WaitHoney()
    {
        yield return waitHoneyWFS;
        slowHoney = true;
        honeyTime = 1.2f;
        speed = 1.3f;
        trapedInHoney.Play();
        honeyParticles.GetComponent<ParticleSystem>().Play();
    }

    //UPDATES
    void Update()
    {
        if (youWon == true)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!inDialogue)
        {
            Movement();
        }
        

        if (youWon == true)
        {
            return;
        }

        if (youLost == true)
        {
            return;
        }

        if (slowHoney)
        {
            HoneyTrail();
        }

        if (inverted)
        {
            InvertedEffect();
        }
    }

    private void LateUpdate() 
    {
        //Si estamos invertidos hacemos que el círculo siga a la cámara
        if(inverted)
        {
            gasClock.transform.LookAt(cam);
            gasGoldBg.transform.LookAt(cam);
        }
    }
}

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

    [SerializeField] ParticleSystem grabPollen, grabHC;
    [SerializeField] AudioClip pollenSound, honeyCombClip, greyHoneyCombClip, levelCompletedSound, dieSound, inGas,buzzing;
    [SerializeField] AudioSource trapedInHoney;

    public CharacterController controller;
    [SerializeField] private GameObject playerPrefab;
    private InputMaster inputMaster;
    public float speed = 3, gravity = 1f, rotationSpeed = 5;
    public float speedInicial, rotationSpeedInicial;
    public Transform cam;
    Vector3 movement, posInicial;
    public bool move = false;

    public int pollen = 0, honeyComb = 0, maxPollen = 10;
    List<GameObject> pollenList = new List<GameObject>(), honeyCombList = new List<GameObject>();

    public UserInterface UserInterfaceScript;
    public bool youLost = false, youWon = false;
    bool inverted = false;

    public CameraControl cameraControl;
    public Door doorScript;

    [SerializeField] float invertedControlsTime = 3;
    [SerializeField] float honeyTime = 3;

    bool slowHoney = false;

    public GameObject honeyParticles;   
    public GameObject gasParticles;
    [SerializeField] Image gasClock = default;
    [SerializeField] Image gasGoldBg;
    [SerializeField] GameObject gasClockCanvas;

    [SerializeField] LevelData levelData;
    [SerializeField] GameObject [] skins;
    GameObject localSkin;
    
    private int controlDirection;

    private void Awake() 
    {
        controller = GetComponent<CharacterController>();
        inputMaster = new InputMaster();
    }
    void Start()
    {
        
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
    }
    private void OnDisable() {
        inputMaster.Disable();
        GameEventBus.Unsubscribe(GameEventType.DIALOGUE, FreezePlayer);
        GameEventBus.Unsubscribe(GameEventType.LOST, FreezePlayer);
        GameEventBus.Unsubscribe(GameEventType.WIN, FreezePlayer);
        GameEventBus.Unsubscribe(GameEventType.NORMALGAME, ReturnToGame);
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

        //ESTO TIENE QUE SER EN ALGUN MOMENTO RESTART LA ESCENA, EN VEZ DE RESETEAR CADA ELEMENTO...TAL VEZ PRIMERO UN MENU DE PERDISTE, LUEGO RESTART ESCENA
        gasParticles.GetComponent<ParticleSystem>().Stop();
        honeyParticles.GetComponent<ParticleSystem>().Stop();
        gasClock.fillAmount = 0;

        slowHoney = false;
        trapedInHoney.Stop();

        speed = speedInicial;

        for (int i = 0; i < pollenList.Count; i++) 
        {
            pollenList[i].SetActive(true);
        }
        pollenList.Clear();

        for (int i = 0; i < honeyCombList.Count; i++)
        {
            honeyCombList[i].SetActive(true);
        }
        honeyCombList.Clear();


        controller.enabled = false;
        //transform.position = posInicial;
        controller.enabled = true;
        pollen = 0;
        honeyComb = 0;
        UserInterfaceScript.UpdatePollen(pollen,maxPollen);
        UserInterfaceScript.ResetHoneyComb();

        doorScript.CloseDoor();

        GameEventBus.Publish(GameEventType.LOST);

        transform.parent = null;
        youLost = true;
        cameraControl.currentView = cameraControl.views[0];
    }

    void GrabPollen(GameObject go) 
    {
        grabPollen.Play();
        pollen++;
        go.SetActive(false);
        pollenList.Add(go);
        if (pollen == maxPollen)
        {
            doorScript.OpenDoor();
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
        if (other.gameObject.CompareTag("Floor"))
        {
            cameraControl.currentView = cameraControl.views[(int)other.gameObject.GetComponent<FloorCam>().floorNumber];
        }
        
        //Here we invert the controls, set a timer in 5 and active the main character gas effect
        if (other.gameObject.CompareTag("Gas") && inverted == false)
        {
            //Emparentamos temporalmente el objeto del reloj y las partículas de humo
            gasClockCanvas.transform.SetParent(transform);
            gasParticles.transform.SetParent(transform);
            gasClockCanvas.transform.localPosition = new Vector3(0,.5f,0);
            gasParticles.transform.localPosition = new Vector3(0,0,0);

            //Invertimos los controles
            inverted = true;
            controlDirection = -1;

            //Desactivamos la trampa de humo
            other.gameObject.GetComponent<ParticleSystem>().Stop();
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(WaitGas());
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
            AudioManager.audioManager.PlaySfxOnce3D(levelCompletedSound,transform);
            GameEventBus.Publish(GameEventType.WIN);
            youWon = true;
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
            if (youWon == false)
            {
                AudioManager.audioManager.PlaySfxOnce(dieSound);
                UserInterfaceScript.timer = 0;
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

        if (youLost == true)
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

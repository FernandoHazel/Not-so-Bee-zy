using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//JUGADOR 1
public class Player3D : MonoBehaviour
{
    [SerializeField] ParticleSystem grabPollen, grabHC;
    [SerializeField] AudioClip pollenSound, honeyCombClip, greyHoneyCombClip, levelCompletedSound, dieSound, inGas,buzzing;
    [SerializeField] AudioSource trapedInHoney;

    public CharacterController controller;
    private InputMaster inputMaster;
    private TutorialDialogues tutorialDialogues;
    private ExtraDialogues extraDialogues;
    private GameManager gm;
    public float speed = 3, gravity = 1f, rotationSpeed = 5;
    public float speedInicial, rotationSpeedInicial;
    public Transform cam;
    Vector3 movement, posInicial;
    public bool move = false;

    public int pollen = 0, honeyComb = 0, maxPollen = 10;
    List<GameObject> pollenList = new List<GameObject>(), honeyCombList = new List<GameObject>();

    public UserInterface UserInterfaceScript;
    public bool youLost = false, youWon = false;

    public CameraControl cameraControl;
    public Door doorScript;

    float invertedControlsTime = 0, honeyTime = 0;

    bool invertedControls = false, slowHoney = false;

    public ParticleSystem honeyParticles;   //This variable is just a main character Honey effect
    public ParticleSystem gasParticles;    //This variable is just a main character gas effect when poisoned

    [SerializeField] Image gasClock = default;
    [SerializeField] Image gasGoldBg;

    [SerializeField] LevelData levelData;
    [SerializeField] GameObject [] skins;
    GameObject localSkin;
    // START
    private void Awake() {
        inputMaster = new InputMaster();
        tutorialDialogues = new TutorialDialogues();
        extraDialogues = new ExtraDialogues();
        gm = new GameManager();
    }
    void Start()
    {
        
        if (levelData.GetSkinSelected() != 2) 
        {
            AudioManager.Instance.PlaySfxLoop3D(buzzing, transform);
        }
        move = true;
        speedInicial = speed;
        rotationSpeedInicial = rotationSpeed;
        posInicial = transform.position;

        UserInterfaceScript.UpdatePollen(pollen,maxPollen);
        UserInterfaceScript.ResetHoneyComb();

        honeyParticles.Stop();
        gasParticles.Stop();
        gasClock.fillAmount = 0;
        //gasGold.enabled = false;
        gasGoldBg.enabled = false;

        localSkin = Instantiate(skins[levelData.GetSkinSelected()], transform.position, transform.rotation);
        localSkin.transform.parent = gameObject.transform;

    }
    private void OnEnable() {
        inputMaster.Enable();
    }
    private void OnDisable() {
        inputMaster.Disable();
    }
    //FUNCTIONS
    void Movement(){
        if (move == true)
        {
            //movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            movement = new Vector3(inputMaster.Player.move.ReadValue<Vector2>().x, 0f, inputMaster.Player.move.ReadValue<Vector2>().y);

            Vector3 camF = cam.forward;
            Vector3 camR = cam.right;


            camF.y = 0;
            camR.y = 0;

            camF = camF.normalized;
            camR = camR.normalized;

            movement.y = movement.y - (gravity * Time.deltaTime);
            controller.Move((camF * movement.z + camR * movement.x) * speed * Time.deltaTime); ////movement.y era movement.z
            controller.Move(new Vector3(0, movement.y, 0) * speed * Time.deltaTime); //gravity applied to movement
            movement.y = 0; // reset y, so the player doesnt look to the floor

            if ((movement != Vector3.zero))
            {
                PlayerRotation(camF, camR);
            }
        }
    }

    void InvertedMovement() 
    {
        if (move == true) 
        {
            //movement = new Vector3(Input.GetAxisRaw("Horizontal_inv"), 0f, Input.GetAxisRaw("Vertical_inv"));
            movement = new Vector3(-inputMaster.Player.move.ReadValue<Vector2>().x, 0f, -inputMaster.Player.move.ReadValue<Vector2>().y);

            Vector3 camF = cam.forward; //Puedo invertir estos valores
            Vector3 camR = cam.right;

            camF.y = 0;
            camR.y = 0;

            camF = camF.normalized;
            camR = camR.normalized;

            movement.y = movement.y - (gravity * Time.deltaTime);

            controller.Move((camF * movement.z + camR * movement.x) * speed * Time.deltaTime);
            controller.Move(new Vector3(0, movement.y, 0) * speed * Time.deltaTime); //gravity applied to movement

            movement.y = 0; // reset y, so the player doesnt look to the floor

            if ((movement != Vector3.zero))
            {
                PlayerRotation(camF, camR);
            }
        }
    }
    void InvertedEffect(){
        gasClock.fillAmount -= 1 / invertedControlsTime * Time.deltaTime *.4f;
        invertedControlsTime -= Time.deltaTime;  //We start a count down for the inverted controls
        //When time is over we go back to normal controls and disable gas effect
        if (invertedControlsTime <= 0)
        {
            invertedControls = false;
            gasParticles.Stop();
            //gasGold.enabled = false;
            gasGoldBg.enabled = false;
            return;
        }
        InvertedMovement();
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
        gasParticles.Stop();
        honeyParticles.Stop();
        gasClock.fillAmount = 0;

        slowHoney = false;
        trapedInHoney.Stop();

        invertedControls = false;

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
        transform.position = posInicial;
        controller.enabled = true;
        pollen = 0;
        honeyComb = 0;
        UserInterfaceScript.UpdatePollen(pollen,maxPollen);
        UserInterfaceScript.ResetHoneyComb();

        doorScript.CloseDoor();

        UserInterfaceScript.YouLost();

        transform.parent = null;
        youLost = true;
        cameraControl.currentView = cameraControl.views[0];
    }

    void Ganaste() 
    {
        UserInterfaceScript.YouWon();
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
        AudioManager.Instance.PlaySfxOnce(pollenSound);
    }

    void GrabHoneyComb(GameObject go)
    {
        grabHC.Play();
        honeyComb++;
        go.SetActive(false);
        honeyCombList.Add(go);
        AudioManager.Instance.PlaySfxOnce(honeyCombClip);
    }

    void GrabHoneyCombTransparent(GameObject go) 
    {
        go.SetActive(false);
        honeyCombList.Add(go);
        AudioManager.Instance.PlaySfxOnce(greyHoneyCombClip);
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
            honeyParticles.Stop();
            return;
        }
    }

    //TRIGGERS
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Pollen")
        {
            GrabPollen(other.gameObject);
            UserInterfaceScript.UpdatePollen(pollen, maxPollen);
        }

        if (other.gameObject.tag == "HoneyComb")
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

        if (other.gameObject.tag == "Checkpoint")
        {
            AudioManager.Instance.PlaySfxOnce3D(levelCompletedSound,transform);
            Ganaste();
            youWon = true;
        }


        if (other.gameObject.tag == "Enemy")
        {
            if (youWon == false)
            {
                AudioManager.Instance.PlaySfxOnce(dieSound);
                Die();
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {

        //Here we invert the controls, set a timer in 5 and active the main character gas effect
        if (other.gameObject.tag == "Gas")
        {
            if (invertedControls == true)
            {
                other.gameObject.GetComponent<ParticleSystem>().Stop();
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(WaitGasInstantly());
            }
            else
            {
                other.gameObject.GetComponent<ParticleSystem>().Stop();
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(WaitGas());
            }
        }

        if (other.gameObject.tag == "Honey")
        {
            if (slowHoney == false)
            {
                StartCoroutine(WaitHoney());
            }
        }

        if (other.gameObject.tag == "Platformer")
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

        if (other.gameObject.tag == "Floor")
        {
            cameraControl.currentView = cameraControl.views[(int)other.gameObject.GetComponent<FloorCam>().floorNumber];
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Platformer")
        {
            move = true;
            transform.parent = null;
        }
    }

    //COROUTINES
    IEnumerator WaitGas()
    {
        AudioManager.Instance.PlaySfxOnce(inGas);
        gasClock.fillAmount = 1;
        //gasGold.enabled = true;
        gasGoldBg.enabled = true;
        gasParticles.Play();
        yield return new WaitForSeconds(.5f);
        invertedControls = true;
        invertedControlsTime = 2.5f;
    }
    IEnumerator WaitGasInstantly()
    {
        AudioManager.Instance.PlaySfxOnce(inGas);
        gasClock.fillAmount = 1;
        //gasGold.enabled = true;
        gasGoldBg.enabled = true;
        gasParticles.Play();
        yield return null;
        invertedControls = true;
        invertedControlsTime = 2.5f;
    }

    IEnumerator WaitHoney()
    {
        yield return new WaitForSeconds(.1f);
        slowHoney = true;
        honeyTime = 1.2f;
        speed = 1.3f;
        trapedInHoney.Play();
        honeyParticles.Play();
    }

    //UPDATES
    void Update()
    {
        if (inputMaster.Player.Pause.triggered)
        {
            gm.ChangePause();
        }
        //avanzamos los diálogos con el botón sur del control
        if (inputMaster.Menus.Select.ReadValue<float>() == 1)
        {
            tutorialDialogues.NextDialogueBool();
            extraDialogues.NextDialogueBool();
        }
        gasClock.transform.LookAt(cam);
        //gasGold.transform.LookAt(cam);
        gasGoldBg.transform.LookAt(cam);

        if (youWon == true)
        {
            return;
        }

        if (youLost == true)
        {
                UserInterfaceScript.timer = 0;
                return;
        }
    }

    private void FixedUpdate()
    {

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

        //We use the normal control system unless the boolean is true, if it is true we invert the controls
        if (invertedControls)
        {
            InvertedEffect();
        }
        else{
            Movement();
        }

        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.EventSystems;

public class JoystickPosition : MonoBehaviour
{
    Touch touch;

    //Si este booleano es verdadero se puede reacomodar el joystick
    private bool canSetJoystick;

    //Tomamos las imágenes del joystick para aparecerlo y desaparecerlo
    [SerializeField] Image JoystickBackgroundImage;
    [SerializeField] Image joystickImage;
    [SerializeField] GameObject Joystick;
    private OnScreenStick onScreenStick;

    //Creamos un eventData para activar el joystic
    PointerEventData eventData;

    private void Start() 
    {
        onScreenStick = Joystick.GetComponent<OnScreenStick>(); // Obtener el componente OnScreenStick

        canSetJoystick = true;

        //Cuando inicia el nivel el joystick está activo para que el jugador entienda
        //Que tiene que usarlo
        JoystickBackgroundImage.enabled = true;
        joystickImage.enabled = true;
    }
    private void Update() 
    {
        //En el momento en que haya un toque en la pantalla
        if(Input.touchCount > 0 && !GameManager.gameIsPaused)
        {
            //Tomamos el primer toque
            touch = Input.GetTouch(0);

            //Avisamos al OnScreenStick que fué activado para moverse
            eventData = new PointerEventData(EventSystem.current);
            eventData.position = touch.position;

            //Al inicio del touch movemos el joystick y reestablecemos el contador
            if(touch.phase == TouchPhase.Began)
            {
                //Aparecemos el joystick y lo posicionamos en el lugar del touch
                SetJoystick();
            }

            if(touch.phase == TouchPhase.Moved)
            {
                eventData.position = touch.position;
                onScreenStick.OnDrag(eventData);
            }
            
            //Si el dedo fué levantado podemos volver a colocar el joystick
            if(touch.phase == TouchPhase.Ended)
            {
                eventData.position = touch.position;
                onScreenStick.OnPointerUp(eventData);
                canSetJoystick = true;
            }
        }
        else
        {
            DisableJoystick();
        }
        
    }

    private void SetJoystick()
    {
        if(canSetJoystick)
        {
            //Reseteamos el contador
            //hideTime = 1;

            //Activamos los objetos en la UI
            JoystickBackgroundImage.enabled = true;
            joystickImage.enabled = true;

            //Cambiamos la posición del joystick a donde fué el touch
            gameObject.transform.position = touch.position;

            //Ya no permitimos que se vuelva a reacomodar el joystick
            canSetJoystick = false;

            // Llamar al método OnPointerDown del componente OnScreenStick
            onScreenStick.OnPointerDown(eventData);
        }
    }

    private void DisableJoystick()
    {
        JoystickBackgroundImage.enabled = false;
        joystickImage.enabled = false;
        canSetJoystick = true;
    }
}

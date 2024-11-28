using System.Collections;
using System.Collections.Generic;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManger : MonoBehaviour
{

    public Canvas MenuCanvas;
    public Canvas TutorialCanvas;

    public EventSystem eventSystem;

    void Start()
    {
        MenuCanvas.enabled = true;
        TutorialCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            return; 
        }
        if(eventSystem.currentSelectedGameObject.name == "iniciar"){
            Debug.Log("Iniciar");
        }
        if(eventSystem.currentSelectedGameObject.name == "salir"){
            // terminate the application
            Application.Quit();            
        }
        if(eventSystem.currentSelectedGameObject.name == "tutorial"){
            //Debug.Log("Tutorial");
            MenuCanvas.enabled = false;
            TutorialCanvas.enabled = true;
        }
        if(eventSystem.currentSelectedGameObject.name == "cerrar"){
            //Debug.Log("Volver");
            MenuCanvas.enabled = true;
            TutorialCanvas.enabled = false;
        }
    }

    
}

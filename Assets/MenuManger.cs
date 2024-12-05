using System.Collections;
using System.Collections.Generic;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManger : MonoBehaviour
{

    public Canvas MenuCanvas;
    public Canvas TutorialCanvas;

    public EventSystem eventSystem;
    private bool isTutorialOpened = false; 

    void Start()
    {
        MenuCanvas.enabled = true;
        TutorialCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(eventSystem.currentSelectedGameObject.name == null)
        {
            return; 
        }
        // by name 
        if(eventSystem.currentSelectedGameObject.name == "iniciar" ){
            // cargar una escena
            SceneManager.LoadScene("SampleScene");
        }
        else if( eventSystem.currentSelectedGameObject.name ==  "tutorial" ){
            TutorialCanvas.enabled = true;
        }
        else if( eventSystem.currentSelectedGameObject.name ==  "cerrar" ){
            TutorialCanvas.enabled = false;
        }

        
    }

    
}

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
    private bool isTutorialOpened = false; 

    void Start()
    {
        MenuCanvas.enabled = true;
        TutorialCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(OVRInput.GetDown(OVRInput.Button.One)){
            Debug.Log("Iniciar");
        }
        if (OVRInput.GetDown(OVRInput.Button.Two) && !isTutorialOpened)
        { 
        
            Application.Quit();            
        }
        if(OVRInput.GetDown(OVRInput.Button.Two) && isTutorialOpened)
        {
            MenuCanvas.enabled=true;
            TutorialCanvas.enabled=false;
            isTutorialOpened=false;
        }
        if(OVRInput.GetDown(OVRInput.Button.Three)){
            //Debug.Log("Tutorial");
            MenuCanvas.enabled = false;
            TutorialCanvas.enabled = true;
        }
        
    }

    
}

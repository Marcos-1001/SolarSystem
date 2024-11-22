using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionManager : MonoBehaviour
{
    // Start is called before the first frame update
    // XR ray interactor

    public XRRayInteractor xrRayInteractor;
    public float rayLength = 100000f;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(xrRayInteractor.transform.position, xrRayInteractor.transform.forward * rayLength, Color.red);
        if (xrRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {

            

        }

       

    }
}

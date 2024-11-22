using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius; 
    public Vector3 velocity ;
    public float mass ;
    public bool isRemovable = false;
    public GameObject selectionHighlight;
    private XRGrabInteractable grabInteractable;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    


    Rigidbody rb;
    // collider
    private SphereCollider sphereCollider;


    
    public void Initialize(Vector3 position, Vector3 initialVelocity, float radius, Color color){
        velocity = initialVelocity;

        transform.position = position;
        this.radius = radius;
        mass = 5.5f * (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);;
        
        // Set the initial properties
        
        rb.mass = mass;      
        rb.velocity = velocity;
        rb.position = transform.position;        

        // add a tag

        //gameObject.tag = "Planet";

        // Set up SphereCollider
        
        sphereCollider.radius = 1;
        sphereCollider.isTrigger = true;
        // Initialize sphere collider position 
        sphereCollider.center = Vector3.zero;


        // Assign to Rigidbody if available
        // Initialize the planet with position, velocity, and mass
        meshFilter = gameObject.AddComponent<MeshFilter>();
        
        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        // URP
        meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
        {
            // set a random color 
            color = new Color(color .r, color .g, color .b, 1.0f)
        };        

        transform.localScale = Vector3.one * radius;

        // set the scale of the selection highlight to be slightly larger than the planet

        selectionHighlight.transform.localScale = Vector3.one * 1.85f;

        
    }

    void Awake(){
        
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();  // Add Rigidbody if missing
        }       
        rb.useGravity = false;

        sphereCollider = gameObject.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            Debug.Log("Added SphereCollider to " + gameObject.name);
        }

        // Set up XRGrabInteractable
        grabInteractable = gameObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
            Debug.Log("Added XRGrabInteractable to " + gameObject.name);
        }
        
                
        

        

        // Create selection highlight
        selectionHighlight = new GameObject("SelectionHighlight");
        selectionHighlight.transform.parent = transform;        

        selectionHighlight.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        selectionHighlight.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"));

        selectionHighlight.GetComponent<MeshRenderer>().material.color = new Color(0, 0, .2f, 0);
        
        
        


        

        // keep the cube invisible
        selectionHighlight.GetComponent<MeshRenderer>().enabled = false;        
        //selectionHighlight.GetComponent<BoxCollider>().enabled = false;

        // Add listeners for select events
        
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
        Debug.Log("Planet initialized: " + gameObject.name);
        
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        
        
        selectionHighlight.GetComponent<MeshRenderer>().enabled = true;
        
        


        Debug.Log("Hover started on planet: " );

    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        selectionHighlight.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("Hover ended on planet: " );
    }

   
    
    public void UpdateVelocity(Planet[] planets, float timeStep){
        
        for (int i = 0; i < planets.Length; i++){
            if (planets[i] != this){
                if (planets[i].rb == null){
                    Debug.Log("Rigidbody not found for planet " + i);

                } 
                if (rb == null){
                    Debug.Log("Rigidbody not found for planet " + i);
                }


                float sqrDst = (planets[i].rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (planets[i].rb.position - rb.position).normalized;
                //Debug.Log("ForceDir: " + forceDir + " SqrDst: " + sqrDst + " Mass: " + mass + " Planets[i].mass: " + planets[i].mass + " G: " + Universe.G); 
                if (sqrDst > 0.001f) { 
                    Vector3 force = forceDir * Universe.G  * mass * planets[i].mass / sqrDst ;
                    velocity += force/mass  * timeStep;
                    //Debug.Log("Velocity: " + velocity + " Force: " + force + " TimeStep: " + timeStep);
                }       
            }
        }
        

    }

    public void UpdatePosition(float timeStep){ 
        // Update position based on new velocity
        if (rb != null) {
            //Debug.Log(velocity* timeStep);
            rb.MovePosition(rb.position + velocity * timeStep );
            transform.position = rb.position;            
        }
    }


    




    public Rigidbody Rigidbody => rb;
    public Vector3 Position => rb.position;
    



    

}

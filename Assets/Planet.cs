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
    public Color color;
    public bool isRemovable = false;
    public string name; 
    public GameObject selectionHighlight;
    //private XRGrabInteractable grabInteractable;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    //public GameObject vfx_explosion 
    
    private TrailRenderer trailRenderer;

    //private GameObject explosion; 
    Rigidbody rb;
    // collider
    public SphereCollider sphereCollider;


    
    public void Initialize(Vector3 position, Vector3 initialVelocity, float radius, Color color){
        velocity = initialVelocity;
        this.color = color;
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

        meshFilter = gameObject.AddComponent<MeshFilter>();
        
        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        // URP

        



        
        // use an existing material in assets
        meshRenderer.material = Resources.Load<Material>("PlanetMaterial");

        meshRenderer.material.color = color;

        transform.localScale = Vector3.one * radius;



        selectionHighlight.transform.localScale = Vector3.one * 1.85f;
        // generate a random name 
        for (int i = 0; i < 10; i++){
            if (i % 2 == 0){
                name += Random.Range(0, 9).ToString();
            }
            else{
                name += (char)Random.Range(65, 90);
            }
        }

        trailRenderer.startColor = new Color(color.r, color.g, color.b, 0.5f);
        trailRenderer.endColor = new Color(color.r, color.g, color.b, 0.0f);
        
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

        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
        
        trailRenderer.startWidth = 0.5f;
        trailRenderer.endWidth = 0.0f;
        trailRenderer.time = 10.0f;

                
        

        

        // Create selection highlight
        selectionHighlight = new GameObject("SelectionHighlight");
        selectionHighlight.transform.parent = transform;        

        selectionHighlight.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        selectionHighlight.AddComponent<MeshRenderer>().material = Resources.Load<Material>("TransparentBox");
        selectionHighlight.GetComponent<MeshRenderer>().material.color = new Color(0.1f, 0.1f, 0.1f, 0.2f);
        
        
        


        

        // keep the cube invisible
        selectionHighlight.GetComponent<MeshRenderer>().enabled = false;        
        //selectionHighlight.GetComponent<BoxCollider>().enabled = false;

        // Add listeners for select events
        
        //grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        //grabInteractable.hoverExited.AddListener(OnHoverExited);



        
        Debug.Log("Planet initialized: " + gameObject.name);
        
    }
    public void selectionHighlight_activate()
    {
        //explosion = Instantiate(vfx_explosion, transform.position, Quaternion.identity);
        //explosion.transform.localScale = Vector3.one * radius * 20;

        selectionHighlight.GetComponent<MeshRenderer>().enabled = true;        
    }
    public void selectionHighlight_deactivate()
    {
        

        selectionHighlight.GetComponent<MeshRenderer>().enabled = false;
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

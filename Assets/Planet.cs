using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius; 
    public Vector3 velocity ;
    public float mass ;
    
    
    Rigidbody rb;
    

    public void Initialize(Vector3 position, Vector3 initialVelocity, float mass, float radius){
        velocity = initialVelocity;

        transform.position = position;
        this.radius = radius;
        this.mass = 5.5f * (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);;

        // Assign to Rigidbody if available
        
    }

    void Awake(){
        
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();  // Add Rigidbody if missing
        }       
        rb.useGravity = false;
        rb.mass = 5.5f * (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);
                
        // Set the initial properties
        
        mass = rb.mass;      
        rb.velocity = velocity;
        rb.position = transform.position;

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
                Debug.Log("ForceDir: " + forceDir + " SqrDst: " + sqrDst + " Mass: " + mass + " Planets[i].mass: " + planets[i].mass + " G: " + Universe.G); 
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
            Debug.Log(velocity* timeStep);
            rb.MovePosition(rb.position + velocity * timeStep );
        }
    }

    




    public Rigidbody Rigidbody{
        get{
            return rb;
        }
    }

    public Vector3 Position{
        get{
            return rb.position;
        }
    }
    



    

}
